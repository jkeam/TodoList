using System.ComponentModel.DataAnnotations;

namespace TodoList.Models;
public class Todo
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public bool IsDone { get; set; }
}