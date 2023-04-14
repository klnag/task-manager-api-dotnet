using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Helpers;
using src.Models.CommentModel;
using src.Models.TodoModel;
using src.Models.ProjectModel;
using Microsoft.AspNetCore.Authorization;

namespace src.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class CommentController : ControllerBase {
    private readonly DataContext context;

    public CommentController(DataContext context) {
        this.context = context;
    }

    [HttpGet()]
    public DbSet<Comment>? Get() {
        return context.Comments;
    }

    [HttpPost]
    public ActionResult<Comment> Post([FromBody] CommentDto CommentFormBody)
    {
        Todo todo = context.Todos.FirstOrDefault(u => u.Id == CommentFormBody.TodoId);
        if (todo != null)
        {
            
                Comment newComment = new Comment { Context = CommentFormBody.Context, UserId = int.Parse(User.Identity.Name), TodoId = CommentFormBody.TodoId, UserName = CommentFormBody.UserName };
                context.Comments.Add(newComment);
                context.SaveChanges();
                return newComment;
        }
        return new BadRequestObjectResult("todo not found");
    }

    // [HttpPatch("{id}")]
    // public string Patch(int id, [FromBody] CommentDto CommentFormBody)
    // {
    //     Comment? Comment = context.Comments?.Find(id);
    //     if (Comment == null)
    //     {
    //         return "Comment does not exisit";
    //     }
    //     Comment.Title = CommentFormBody.Title;
    //     Comment.Status = CommentFormBody.Status;
    //     context.Comments?.Update(Comment);
    //     context.SaveChanges();
    //     return "Comment updated";
    // }

    // [HttpPatch("status/{id}")]
    // public ActionResult<IQueryable<Comment>> PatchStatus(int id, [FromBody] CommentDto CommentFormBody)
    // {
    //     Comment? Comment = context.Comments?.Find(id);
    //     if (Comment == null)
    //     {
    //         return new BadRequestObjectResult("Comment does not exisit");
    //     }
    //     Comment.Title = CommentFormBody.Title;
    //     Comment.Status = CommentFormBody.Status;
    //     context.Comments?.Update(Comment);
    //     context.SaveChanges();
    //     return Ok(context.Comments.Where(Comment => Comment.ProjectId == CommentFormBody.ProjectId));
    // }

    // [HttpDelete("{id}")]
    // public string Delete(int id)
    // {
    //     Comment? Comment = context.Comments?.Find(id);
    //     if (Comment == null)
    //     {
    //         return "Comment does not exisit";
    //     }
    //     context.Comments?.Remove(Comment);
    //     context.SaveChanges();
    //     return "Comment deleted";
    // }
}