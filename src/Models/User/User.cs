using System.ComponentModel.DataAnnotations;
using src.Models.ProjectModel;

namespace src.Models.UserModel;

public class User {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<Project>? Projects { get; set; }
}