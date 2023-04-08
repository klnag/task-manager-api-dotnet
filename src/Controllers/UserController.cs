using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.UserModel;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly DataContext context;
    private readonly IConfiguration _configuration;

    public UserController(IConfiguration configuration,DataContext context) {
        this._configuration = configuration;
        this.context = context;
    }

    private string userCreateJwt(string id) {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, id)
        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
        SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        JwtSecurityToken token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: cred);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // [HttpGet]
    // public DbSet<User>? Get() {
    //     return context.Users;
    // }

    [HttpGet(), Authorize()]
    public ActionResult<User> GetInfo() {
        string id = User.Identity.Name;
        User? user = context.Users?.Find(int.Parse(id));
        if(user == null) {
            return new BadRequestObjectResult("user does not exsist");
        }
        return user; 
    }
    [HttpPost] 
    public ActionResult<string> Post(UserDto userFormBody) {
        if(context.Users?.FirstOrDefault(user => user.Email == userFormBody.Email) == null) {
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(userFormBody.Password);
            User newUser = new User { Username = userFormBody.Username, Email = userFormBody.Email, Password = hashPassword };
            context.Users?.Add(newUser);
            context.SaveChanges();
            string token = userCreateJwt(newUser.Id+"");
            return token;
        }
        return new BadRequestObjectResult("user with that email exist already exisit");
    }

    [HttpPost("login")]
    public string post(UserDto userFormBody) {
        User user = context.Users?.FirstOrDefault(user => user.Email == userFormBody.Email);
        if(user == null) {
            return "email does not exist";
        }
        if(!BCrypt.Net.BCrypt.Verify(userFormBody.Password, user.Password)){
            return "Password is incorect";
        }
        return userCreateJwt(user.Id+"");
    }

    [HttpPatch, Authorize]
    public string Patch([FromBody] User userFormBody) {
        string id = User.Identity.Name;
        User? user = context.Users?.Find(int.Parse(id));
        if(user == null){
            return "user does not exisit";
        }
        user.Username = userFormBody.Username;
        context.Users?.Update(user);
        context.SaveChanges();
        return "user updated";
    }


    [HttpDelete(), Authorize]
    public string Delete() {
        string id = User.Identity.Name;
        User? user = context.Users?.Find(int.Parse(id));
        if(user == null){
            return "user does not exisit";
        }
        context.Users?.Remove(user);
        context.SaveChanges();
        return "user deleted";
    }
}