using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.TodoModel;
using src.Models.ProjectModel;
using src.Models.CommentModel;
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
            
                Todo newTodo = new Todo { Title = todoFormBody.Title, Project = project, username = todoFormBody.username, index = todoFormBody.index };
                context.Todos.Add(newTodo);
                context.SaveChanges();
                return newTodo;
        }
        return new BadRequestObjectResult("project not found");
    }

    [HttpPatch("{id}")]
    public ActionResult<Todo> Patch(int id, [FromBody] TodoDto TodoFormBody)
    {
        Todo? todo = context.Todos?.Find(id);
        if (todo == null)
        {
            return new BadRequestObjectResult("Todo does not exisit");
        }
        todo.Title = TodoFormBody.Title;
        todo.Status = TodoFormBody.Status;
        todo.Context = TodoFormBody.Context;
        context.Todos?.Update(todo);
        context.SaveChanges();
        return todo;
    }

    // [HttpPatch("status/{id}")]
    // public ActionResult<IQueryable<Todo>> PatchStatus(int id, [FromBody] TodoDto TodoFormBody)
    // {
    //     Todo? Todo = context.Todos?.Find(id);
    //     if (Todo == null)
    //     {
    //         return new BadRequestObjectResult("Todo does not exisit");
    //     }
    //     Todo.Title = TodoFormBody.Title;
    //     Todo.Status = TodoFormBody.Status;
    //     context.Todos?.Update(Todo);
    //     context.SaveChanges();
    //     return Ok(context.Todos.Where(todo => todo.ProjectId == TodoFormBody.ProjectId));
    // }
    // [HttpPatch("context/{id}")]
    // public ActionResult<IQueryable<Todo>> PatchContext(int id, [FromBody] TodoDto TodoFormBody)
    // {
    //     Todo? Todo = context.Todos?.Find(id);
    //     if (Todo == null)
    //     {
    //         return new BadRequestObjectResult("Todo does not exisit");
    //     }
    //     Todo.Context = TodoFormBody.Context;
    //     context.Todos?.Update(Todo);
    //     context.SaveChanges();
    //     return Ok(context.Todos.Where(todo => todo.ProjectId == TodoFormBody.ProjectId));
    // }
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

    // [HttpGet("alltodocomments")]
    // public ActionResult<IQueryable<Comment>> GetAllTodoComments(int todoId) {
    //    Todo? Todo = context.Todos?.Find(todoId);
    //     if (Todo != null)
    //     {
    //         return Ok(context.Comments.Where(com => com.TodoId == todoId));
    //     } 

    //         return new BadRequestObjectResult("Todo does not exisit");
    // }

    // PUT: api/TaskItems/5/position
    [HttpPut("{id}/position")]
    public async Task<ActionResult<Todo>> UpdateTaskPosition(int id, [FromBody] TodoDto request)
    {
        var taskItem = await context.Todos.FindAsync(id);
        if (taskItem == null)
        {
            return NotFound();
        }

        // int oldPosition = taskItem.index;
        // int newPosition = request.index;

        // if (request.Status == taskItem.Status)
        // {

        //     if (oldPosition < newPosition)
        //     {
        //         // Move tasks between old and new position up
        //         var tasksToMoveUp = context.Todos.Where(t => t.index > oldPosition && t.index <= newPosition);
        //         foreach (var task in tasksToMoveUp)
        //         {
        //             task.index--;
        //         }
        //     }
        //     else if (oldPosition > newPosition)
        //     {
        //         // Move tasks between new and old position down
        //         var tasksToMoveDown = context.Todos.Where(t => t.index >= newPosition && t.index < oldPosition);
        //         foreach (var task in tasksToMoveDown)
        //         {
        //             task.index++;
        //         }
        //     }
        // }else {
        //     var tasksToMoveUp = context.Todos.Where(t => (t.index > oldPosition) && t.Status == taskItem.Status );
        //         foreach (var task in tasksToMoveUp)
        //         {
        //             task.index--;
        //         }
        // }

        taskItem.index = request.index;
        taskItem.Status = request.Status;
        await context.SaveChangesAsync();

        return taskItem;
    }

}