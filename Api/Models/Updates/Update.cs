using Api.Services.Enums;

namespace Api.Models;

public class Update
{
    public UpdateSteps UpdateStep { get; set; }
    public string SessionId { get; set; }

}
