using System;

namespace Api.DTOs.Updates;

public class LoginForUpdateDTO
{
    public bool IsUsingSavedAuth { get; set; } 
    public string? SavedUsernameAndPasswordId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
