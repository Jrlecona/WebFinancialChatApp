﻿namespace ChatApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}