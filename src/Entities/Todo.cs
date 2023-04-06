using System.ComponentModel.DataAnnotations;

namespace src.Entities;

public class Todo {
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}