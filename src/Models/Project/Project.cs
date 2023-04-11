using System.ComponentModel.DataAnnotations;
using src.Models.UserModel;
using src.Models.TodoModel;
using System.Text.Json.Serialization;

namespace src.Models.ProjectModel;

public class Project {
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    [JsonIgnore]
    public ICollection<Todo>? Todos { get; set; }
}