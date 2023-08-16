using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyNet.Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using SkyNet.Core.DTOs.User;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace SkyNet.Core.Services
{
    public class UserService : DbContextOptionsBuilder
    {
        private readonly EmailService _emailService;

        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, EmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<ServiceResponse> LoginUserAsync(UserLoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User or password incorrect",
                };
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "User successfully logged in"
                };
            }
            if (result.IsNotAllowed)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Confirm your email"
                };
            }
            if (result.IsLockedOut)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User os locked. Connect with your site administrator"
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "User or password incorrect"
            };
        }
        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<ServiceResponse> GetAllAsync()
        {
            //List<AppUser> users = await _userManager.Users.ToListAsync();
            //List<UsersDTO> mappedUsers = new List<UsersDTO>();

            //foreach (var user in users)
            //{
            //    var userDto = _mapper.Map<AppUser, UsersDTO>(user);
            //    var roles = await _userManager.GetRolesAsync(user);
            //    userDto.Role = roles.FirstOrDefault() ?? string.Empty;
            //    mappedUsers.Add(userDto);
            //}


            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UsersDTO> mappedUsers = users.Select(u => _mapper.Map<AppUser, UsersDTO>(u)).ToList();
            for (int i = 0; i < users.Count; i++)
            {
                mappedUsers[i].Role = (await _userManager.GetRolesAsync(users[i])).FirstOrDefault()!;
            }

            return new ServiceResponse
            {
                Success = true,
                Message = "All users loaded",
                PayLoad = mappedUsers
            };
        }
        public async Task<ServiceResponse> GetUserByIdAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User or password incorrect",
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var mappedUser = _mapper.Map<AppUser, UpdateUserPasswordDTO>(user);
            mappedUser.Role = roles[0];
            return new ServiceResponse
            {
                Success = true,
                Message = "User successfully loaded",
                PayLoad = mappedUser,
            };
        }
        public async Task<ServiceResponse> UpdateUserAsync(UpdateUserDTO modelUser)
        {
            //AppUser user = await _userManager.FindByEmailAsync(modelUser.Email);
            var user = await _userManager.FindByIdAsync(modelUser.ID);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User not found",
                };
            }
            if (user.Email != modelUser.Email)
            {
                user.EmailConfirmed = false;
                var confirmationResut = await _userManager.UpdateAsync(user);
            }
            AppUser updatedUser = _mapper.Map<AppUser>(user);
            updatedUser.UserName = modelUser.Email;
            updatedUser.Email = modelUser.Email;
            updatedUser.FirstName = modelUser.FirstName;
            updatedUser.LastName = modelUser.LastName;
            updatedUser.PhoneNumber = modelUser.PhoneNumber;

            var result = await _userManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "User seccessfully updated",
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Error user updated",
                };
            }
        }
        public async Task<ServiceResponse> UpdatePasswordAsync(UpdatePasswordDTO modelPassword)
        {
            //AppUser user = await _userManager.FindByEmailAsync(modelPassword.Email);
            var user = await _userManager.FindByIdAsync(modelPassword.ID);
            if (user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User or password incorrect",
                };
            }
            IdentityResult result = await _userManager.ChangePasswordAsync(user, modelPassword.OldPassword, modelPassword.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Password successfully updated",
                };
            }
            List<IdentityError> errorList = result.Errors.ToList();
            string errors = "";
            foreach (var error in errorList)
            {
                errors = errors + error.Description.ToList();
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "Error password updated",
                PayLoad = errors,
            };
        }
        public async Task<ServiceResponse> CreateUser(CreateUserDTO model)
        {
            AppUser existUser = await _userManager.FindByEmailAsync(model.Email);
            if (existUser != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User already exists",
                };
            }

            AppUser mappedUser = _mapper.Map<CreateUserDTO, AppUser>(model);
            mappedUser.UserName = model.Email;
            IdentityResult res = await _userManager.CreateAsync(mappedUser, model.Password);

            if (res.Succeeded)
            {
                IdentityResult createrole = await _userManager.AddToRoleAsync(mappedUser, model.Role);

                await SendConfirmationEmailAsync(mappedUser);

                //string subject = "New Registration";
                //string body = "Thank you for registering. We require that you validate your registration email to ensure that the email address you entered was correct. This protects against unwanted spam and malicious abuse.";
                //await _emailService.SendEmail(model.Email, subject, body);

                return new ServiceResponse
                {
                    Success = true,
                    Message = "User Created"
                };
            }
            List<IdentityError> errorList = res.Errors.ToList();
            string errors = "";
            foreach (var error in errorList)
            {
                errors = errors + error.Description.ToList();
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "Error",
                PayLoad = errors,
            };
        }
        public async Task SendConfirmationEmailAsync(AppUser appUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["HostSettings:URL"]}/Dashboard/confirmEmail?userId={appUser.Id}&token={validEmailToken}";

            string emailBody = $"<h1>Thank you for registering. Confirm your email please</h1><a href='{url}'>Confirm now</a>";
            await _emailService.SendEmail(appUser.Email!, "Email confirmation", emailBody);
        }
        public async Task<ServiceResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User not found",
                };
            }
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            var result = await _userManager.ConfirmEmailAsync(user, normalToken);
            if (result.Succeeded)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "User successfully confirmed",
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "User not confirmed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }
    }
}