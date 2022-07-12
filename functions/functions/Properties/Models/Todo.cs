using System;

namespace functions.Models;

public class Todo
{
    public string Id { get; set; } = Guid.NewGuid().ToString("n");
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}