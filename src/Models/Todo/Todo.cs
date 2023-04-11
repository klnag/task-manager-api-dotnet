using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using src.Models.ProjectModel;
namespace src.Models.TodoModel;

public class Todo {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public int ProjectId { get; set; }
    [JsonIgnore]
    public Project Project { get; set; }
}