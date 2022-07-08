namespace TodoList.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Todo
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public bool IsDone { get; set; }
}