using System.ComponentModel.DataAnnotations;
using src.Models.ProjectModel;
namespace src.Models.TodoModel;

public class Todo {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}