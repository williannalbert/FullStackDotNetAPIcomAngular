using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IUserPersist userPersist)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersist = userPersist;
        }
        public async Task<SignInResult> CheckUserPassowrdAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                    .SingleOrDefaultAsync(u => u.UserName.ToLower() == userUpdateDto.UserName.ToLower());
                //passado o falso para não bloquear usuario caso senha esteja errada
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar password. Erro: {ex.Message}");
            }
        }

        public async Task<UserDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userReturn = _mapper.Map<UserDto>(user);
                    return userReturn;
                }

                return null;
            }
            catch (Exception ex)
            {
                var t = ex.InnerException.Message;
                throw new Exception($"Erro ao criar conta. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(username);
                if (user == null)
                    return null;

                var userUpdate = _mapper.Map<UserUpdateDto>(user);
                return userUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao retornar usuário por username. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDto.UserName);
                if (user == null)
                    return null;

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userPersist.Update<User>(user);
                if(await _userPersist.SaveChangesAsysnc())
                {
                    var userRetorno = await _userPersist.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName.ToLower() == username.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se usuário existe. Erro: {ex.Message}");
            }
        }
    }
}
