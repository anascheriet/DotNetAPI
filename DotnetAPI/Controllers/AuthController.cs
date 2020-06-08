using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DotnetAPI.Dto;
using DotnetAPI.Model;
using DotnetAPI.Data;

namespace DotnetAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IGestionRepository _repo;

        private readonly IConfiguration _config;

        public AuthController(UserManager<AppUser> userManager, 
        IMapper mapper, RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IConfiguration config,
        IGestionRepository repo )
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var roles = new List<AppRole> {
                new AppRole{Name= "Prof"},
                new AppRole{Name = "Student"}
            };

            foreach(var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

            var user = _mapper.Map<AppUser>(userForRegisterDto);

          var result =   await _userManager.CreateAsync(user, userForRegisterDto.password);
          if(result.Succeeded)
          {
              if(user.Status =="prof" || user.Status == "Prof")
              {
                await _userManager.AddToRoleAsync(user, roles[0].Name); 
              }
              else{
                  await _userManager.AddToRoleAsync(user, roles[1].Name); 
              }
              
              return Ok("Registred Successfully");
          }
          return BadRequest(result.Errors);

        }
        
    [HttpPost("login")]
       public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var user = await _userManager.FindByNameAsync(userForLogin.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLogin.password,false);

            if(result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(
                    new {
                        token = GenerateToken(user, roles)
                    }
                    );
            }

            return BadRequest("Username or password are incorrect !");

        }

        [HttpGet("Users")]
        public async Task<IActionResult> getUsers()
        {
            var users = await _repo.GetUsers();
            if(users == null)return Ok("Error, Users not found !\n Please register and try again!");
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }
        [HttpGet("Users/{id}")]
        public async Task<IActionResult> getUser(int id)
        {
            var user = await _repo.GetUser(id);
            if(user == null)return Ok("Error, User with the id "+id+" doesn't exist !");
            var userToReturn = _mapper.Map<UserForListDto>(user);
            return Ok(userToReturn);
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> deleteUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var classes = await _repo.GetClasses(user.Id);
            foreach(var c in classes)
            {
                _repo.Delete(c);
            }
            _repo.Delete(user);
            await _repo.Save();
            return Ok("Deleted "+user.FName+" "+user.LName+" Successfully !");
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> editUser(UserForEditDto userForEditDto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _mapper.Map(userForEditDto,user);

            await _repo.Save();
            return Ok("Modified Successfully !");
        }

          private string GenerateToken(AppUser user, IList<string> roles){

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)     
                };

                foreach(var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(1)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        } 


    }

   
}