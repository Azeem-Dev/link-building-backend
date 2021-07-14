using link_building.Dtos;
using link_building.Dtos.User;
using link_building.Ef_Core;
using link_building.Models.User;
using link_building.Services.AuthRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace link_building.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public AuthController(IAuthRepository authRepository,IHttpContextAccessor httpContextAccessor,DataContext context)
        {
            _authRepo = authRepository;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }



        [HttpPost("Register")]

        public async Task<IActionResult> Register(UserRegisterDto request)
        {

            ServiceResponse<int> response = await _authRepo.Register(new UserEntity()
            {
                Username = request.Username,
                Email=request.Email
            }, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(UserLoginDto request)
        {

            ServiceResponse<string> response = await _authRepo.Login(
                request.Username, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(UserPasswordUpdateDto request)
        {
            return Ok(await _authRepo.ForgotPassword(request));
        }

        [HttpPut("UserRoleUpdate")]
        public async Task<IActionResult> UpdateRole(UserRoleUpdateDto request)
        {
            return Ok(await _authRepo.UpdateRole(request));
        }


    }
}
