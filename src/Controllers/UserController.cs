using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.UserModel;

namespace src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly DataContext context;

    public UserController(DataContext context) {
        this.context = context;
    }

    [HttpGet]
    public DbSet<User>? Get() {
        return context.Users;
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id) {
        User? user = context.Users?.Find(id);
        if(user == null) {
            return new BadRequestObjectResult("user does not exsist");
        }
        return user; 
    }
    [HttpPost] 
    public ActionResult<User> Post(User userFormBody) {
        if(context.Users?.FirstOrDefault(user => user.Email == userFormBody.Email) == null) {
            User newUser = new User { Username = userFormBody.Username, Email = userFormBody.Email, Password = userFormBody.Password };
            context.Users?.Add(newUser);
            context.SaveChanges();
            return newUser;
        }
        return new BadRequestObjectResult("user with that email exist already exisit");
    }

    [HttpPatch("{id}")]
    public string Patch(int id, [FromBody] User userFormBody) {
        User? user = context.Users?.Find(id);
        if(user == null){
            return "user does not exisit";
        }
        user.Username = userFormBody.Username;
        context.Users?.Update(user);
        context.SaveChanges();
        return "user updated";
    }


    [HttpDelete("{id}")]
    public string Delete(int id) {
        User? user = context.Users?.Find(id);
        if(user == null){
            return "user does not exisit";
        }
        context.Users?.Remove(user);
        context.SaveChanges();
        return "user deleted";
    }
}