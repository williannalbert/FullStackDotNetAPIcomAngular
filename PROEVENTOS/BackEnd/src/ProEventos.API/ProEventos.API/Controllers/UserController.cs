using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using System.Security.Claims;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        public IUserService _userService { get; }
        public ITokenService _tokenService { get; }
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                //user abaixo não se refere a model mas ao claim de controller base
                //método abaixo está criado em Extensions
                var username = User.GetUserName();
                var user = await _userService.GetUserByUserNameAsync(username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar usuario. Erro: " + ex.Message);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _userService.UserExists(userDto.Username))
                    return BadRequest("Usuário já cadastrado");

                var user = _userService.CreateAccountAsync(userDto);
                if(user != null)
                    return Ok(new
                    {
                        userName = user.Result.UserName,
                        primeiroNome = user.Result.PrimeiroNome,
                        token = _tokenService.CreateToken(user.Result).Result
                    });

                return BadRequest("Usuário não cadastrado. Tente novamente mais tarde");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao registrar usuario. Erro: " + ex.Message);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(userLoginDto.Username);
                if(user == null)
                    return Unauthorized("Usuário ou senha não encontrado");

                var result = await _userService.CheckUserPassowrdAsync(user, userLoginDto.Password);
                if (!result.Succeeded)
                    return Unauthorized();

                return Ok(new
                {
                    username = user.UserName,
                    primeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao realizar login de usuario. Erro: " + ex.Message);
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                if(userUpdateDto.UserName != User.GetUserName())
                {
                    return Unauthorized("Usuário inválido");
                }
                //retornando usuario baseado no token
                var user = await _userService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null)
                    return Unauthorized("Usuário inválido");

                var userReturn = _userService.UpdateAccount(userUpdateDto);
                if (userReturn == null)
                    return NoContent();

                return Ok(new {
                    username = userReturn.Result.UserName,
                    primeiroNome = userReturn.Result.PrimeiroNome,
                    token = _tokenService.CreateToken(userReturn.Result).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar usuario. Erro: " + ex.Message);
            }
        }

    }
}
