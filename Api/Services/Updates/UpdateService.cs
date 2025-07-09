
namespace Api.Services.Updates;

public class UpdateService : IUpdateService
{
  private readonly IUpdateLogin _updateLogin;
  private readonly IUpdateVerification _updateVerification;

  public UpdateService(IUpdateLogin updateLogin, IUpdateVerification updateVerification)
  {
    _updateLogin = updateLogin;
    _updateVerification = updateVerification;
  }

  public Task<ResultClass<Update>> LoginForUpdate(LoginForUpdateDTO request) => _updateLogin.LoginForUpdate(request);

  public Task<ResultClass<Update>> VerificationForUpdate(VerificationForUpdateDTO request) => _updateVerification.VerificationForUpdate(request);

}
