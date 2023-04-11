using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.ProjectModel;
using src.Models.UserModel;
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
    public string Post(int id, string name)
    {
        User user = context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            if (context.Projects.FirstOrDefault(proj => proj.Name == name) == null)
            {
                Project newProject = new Project { Name = name, User = user };
                context.Projects.Add(newProject);
                context.SaveChanges();
                return "project created" + newProject.Id;
            }
            return "project already exists";
        }
        return "user not found";
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
    public IQueryable<Project> getAllProjectOfUser() {
        int id = int.Parse(User.Identity.Name);
        IQueryable<Project> allUserProjects = context.Projects.Where(p => p.UserId == id);
        return allUserProjects;
    }
}