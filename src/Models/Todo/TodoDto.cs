using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using src.Models.ProjectModel;
namespace src.Models.TodoModel;

public class TodoDto {
    public string? Title { get; set; }
    public int ProjectId { get; set; }
    public string Status { get; set; } = "TODO";
}