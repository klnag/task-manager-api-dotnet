using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Entities;

namespace src.Controllers;

[ApiController]
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
    public string Post([FromBody] Project projectFormBody)
    {
        if(context.Projects?.FirstOrDefault(project => project.name == projectFormBody.name) == null) {
            Project project = new() { name=projectFormBody.name, UserId=projectFormBody.UserId };
            context.Projects?.Add(project);
            context.SaveChanges();
            return "project created";
        }
        return "project exist";
    }

    [HttpPatch("{id}")]
    public string Patch(int id, [FromBody] Project projectFormBody)
    {
        Project? project = context.Projects?.Find(id);
        if (project == null)
        {
            return "project does not exisit";
        }
        project.name = projectFormBody.name;
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
}