using link_building.Dtos;
using link_building.Dtos.User;
using link_building.Ef_Core;
using link_building.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace link_building.Services.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {


        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            UserEntity user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Invalid Password";
                
            }
            else
            {
                user.LastLoggedIn = DateTime.Now;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                response.Data = CreateToken(user);
                response.Message = "Logged in Successfully";
                response.isAuthorized = true;
            }
            return response;
            


        }

        public async Task<ServiceResponse<int>> Register(UserEntity user, string Password)
        {

            ServiceResponse<int> response = new ServiceResponse<int>();
            
            if (await UserExists(user.Username)) {
                response.Success = false;
                response.Message = "User already Exits";
                return response;
            }

            CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = "Admin";
            user.IsAuthorized = true;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(u=> u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA512() )
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password,byte[] passwordHash,byte[] passwordSalt)
        {
            using(var hmac= new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private string CreateToken(UserEntity user)
        {


            List<Claim> cLaims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };


            SymmetricSecurityKey key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
                );


            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(cLaims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);


         
        }

        public async Task<ServiceResponse<string>> ForgotPassword(UserPasswordUpdateDto request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            if (await UserExists(request.username))
            {
                var user= await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(request.username.ToLower()));
                if (VerifyPasswordHash(request.password, user.PasswordHash, user.PasswordSalt))
                {
                    CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    response.Data = $"A new password has been set from {request.password} to {request.NewPassword}";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid Password";
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "User Doesn't Exists";
                return response;
            }
            return response;

        }

        public async Task<ServiceResponse<string>> UpdateRole(UserRoleUpdateDto request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            if (await UserExists(request.Username))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(request.Username.ToLower()));
                if (VerifyPasswordHash(request.UserPassword, user.PasswordHash, user.PasswordSalt))
                {
                    user.Role = request.NewRole;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid Password";
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "User Doesn't Exists";
                return response;
            }
            return response;


        }
    }
}
