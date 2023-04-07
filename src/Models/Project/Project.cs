using System.ComponentModel.DataAnnotations;
using src.Models.UserModel;
using src.Models.TodoModel;
namespace src.Models.ProjectModel;

public class Project {
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Todo>? Todos { get; set; }
}