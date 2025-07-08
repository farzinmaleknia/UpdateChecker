namespace Api.Models;

public class SecurityQuestion
{
  public int Id { get; set; }
  public string Question { get; set; }
  public string Answer { get; set; } = string.Empty;
}
