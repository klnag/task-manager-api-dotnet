using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.TodoModel;
using src.Models.ProjectModel;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class TodoController : ControllerBase {
    private readonly DataContext context;

    public TodoController(DataContext context) {
        this.context = context;
    }

    [HttpGet()]
    public DbSet<Todo>? Get() {
        return context.Todos;
    }

    [HttpPost]
    public ActionResult<Todo> Post([FromBody] TodoDto todoFormBody)
    {
        Project project = context.Projects.FirstOrDefault(u => u.Id == todoFormBody.ProjectId);
        if (project != null)
        {
            if (context.Todos.FirstOrDefault(proj => proj.Title == todoFormBody.Title) == null)
            {
                Todo newTodo = new Todo { Title = todoFormBody.Title, Project = project };
                context.Todos.Add(newTodo);
                context.SaveChanges();
                return newTodo;
            }
            return new BadRequestObjectResult("Todo already exists");
        }
        return new BadRequestObjectResult("project not found");
    }

    [HttpPatch("{id}")]
    public string Patch(int id, [FromBody] Todo TodoFormBody)
    {
        Todo? Todo = context.Todos?.Find(id);
        if (Todo == null)
        {
            return "Todo does not exisit";
        }
    Todo.Title = TodoFormBody.Title;
        context.Todos?.Update(Todo);
        context.SaveChanges();
        return "Todo updated";
    }


    [HttpDelete("{id}")]
    public string Delete(int id)
    {
        Todo? Todo = context.Todos?.Find(id);
        if (Todo == null)
        {
            return "Todo does not exisit";
        }
        context.Todos?.Remove(Todo);
        context.SaveChanges();
        return "Todo deleted";
    }
}