using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.TodoModel;
using src.Models.ProjectModel;

namespace src.Controllers;

[ApiController]
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
    public string Post(int id, string title)
    {
        Project project = context.Projects.FirstOrDefault(u => u.Id == id);
        if (project != null)
        {
            if (context.Todos.FirstOrDefault(proj => proj.Title == title) == null)
            {
                Todo newTodo = new Todo { Title = title, Project = project };
                context.Todos.Add(newTodo);
                context.SaveChanges();
                return "Todo created" + newTodo.Id;
            }
            return "Todo already exists";
        }
        return "project not found";
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