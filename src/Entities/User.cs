using System.ComponentModel.DataAnnotations;

namespace src.Entities;

public class User {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Username { get; set; }
    public List<Project>? Projects { get; set; }
}