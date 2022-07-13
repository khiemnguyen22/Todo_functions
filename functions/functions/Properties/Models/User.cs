using System;

namespace functions.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString("n");
    public string FirstName { get; set; };
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;
}