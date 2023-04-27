using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.ProjectModel;
using src.Models.UserModel;
using src.Models.TodoModel;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class ProjectController : ControllerBase {
    private readonly DataContext context;

    public ProjectController(DataContext context) {
        this.context = context;
    }

    [HttpGet()]
    public DbSet<Project>? Get() {
        return context.Projects;
    }

    [HttpPost]
    public ActionResult<Project> Post([FromBody] ProjectDto projectName)
    {
        User user = context.Users.FirstOrDefault(u => u.Id == int.Parse(User.Identity.Name));
        if (user != null)
        {
            if (context.Projects.FirstOrDefault(proj => proj.Name == projectName.Name) == null)
            {
                Project newProject = new Project { Name = projectName.Name, User =  user, ShareUsersId =  {user.Id}, ShareUsersUsername = { user.Username}  };
                context.Projects.Add(newProject);
                context.SaveChanges();
                return newProject;
            }
            return new BadRequestObjectResult("project already exists");
        }
        return new BadRequestObjectResult("user not found");
    }

    [HttpPatch("{id}")]
    public string Patch(int id, [FromBody] Project projectFormBody)
    {
        Project? project = context.Projects?.Find(id);
        if (project == null)
        {
            return "project does not exisit";
        }
        project.Name = projectFormBody.Name;
        context.Projects?.Update(project);
        context.SaveChanges();
        return "project updated";
    }


    [HttpDelete("{id}")]
    public string Delete(int id)
    {
        Project? project = context.Projects?.Find(id);
        if (project == null)
        {
            return "project does not exisit";
        }
        context.Projects?.Remove(project);
        context.SaveChanges();
        return "project deleted";
    }

    [HttpPost("userprojects")]
    public IQueryable<Project> getAllProjectOfUser(int userId) {
        // int id = int.Parse(User.Identity.Name);
        // Console.WriteLine(id);
        User user = context.Users.Find(userId);
        IQueryable<Project> allUserProjects = context.Projects.Where(p => p.ShareUsersId.Contains( user.Id));
        return allUserProjects;
    }

    [HttpPost("projecttodos")]
    public ActionResult<IQueryable<Todo>> getAllTasksOfPrject(int projectId) {
        int userId = int.Parse(User.Identity.Name);
        if (context.Users.Find(userId) != null)
        {
            IQueryable<Todo> allProjectTodos = context.Todos.Where(todo => todo.ProjectId == projectId);
            return Ok(allProjectTodos.OrderBy(t => t.index));
        }
        return new BadRequestObjectResult("User does not exist");
    }

    [HttpPatch("addUserIdToProject")]
    public string addUserIdToProjectPost(int projectId, string sharedUserEmail) {
        int userId = int.Parse(User.Identity.Name);
        User user = context.Users.Find(userId);
        User sharedUser = context.Users.FirstOrDefault(u => u.Email == sharedUserEmail);
        if(sharedUser == null) {
            return "couldt find email";
        }
        if(user != null) {
            Project project = context.Projects.Find(projectId);
            if(project != null) {
                if(project.UserId == user.Id) {
                    if(sharedUser != null) {
                        // var p = project.ShareUsersId.FirstOrDefault(p => p.Id == sharedUser.Id);
                        if(!project.ShareUsersId.Any(p => p == sharedUser.Id)) {
                            project.ShareUsersId.Add(sharedUser.Id);
                            project.ShareUsersUsername.Add(sharedUser.Username);
                            context.Projects.Update(project);
                            context.SaveChanges();
                            return "Done";
                        }
                        return "id alredy exist";
                    } else {
                        return "the shared user does not exist";
                    }
                } else {
                    return "Project does not belongs to this user";
                }
            } else {
                return "Project does not exist";
            }
        }
        return "User Does not exisit";
    }
}