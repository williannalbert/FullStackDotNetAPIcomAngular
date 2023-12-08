﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
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
                    return Ok(user.Result);

                return BadRequest("Usuário não cadastrado. Tente novamente mais tarde");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar usuario. Erro: " + ex.Message);
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
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar usuario. Erro: " + ex.Message);
            }
        }
    }
}
