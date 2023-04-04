using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Entities;

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
    public string Post([FromBody] User userFormBody) {
        if(context.Users?.FirstOrDefault(user => user.Username == userFormBody.Username) == null) {
            User newUser = new User { Username = userFormBody.Username };
            context.Users?.Add(newUser);
            context.SaveChanges();
            return "user created";
        }
        return "user already exisit";
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