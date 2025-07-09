using PuppeteerSharp;

namespace Api.Services.Updates;

public interface IUpdateService
{
  public Task<ResultClass<Update>> LoginForUpdate(LoginForUpdateDTO request);
  public Task<ResultClass<Update>> VerificationForUpdate(VerificationForUpdateDTO request);
}

public interface IUpdateLogin
{
  public Task<ResultClass<Update>> LoginForUpdate(LoginForUpdateDTO request);
}

public interface IUpdateVerification
{
  public Task<ResultClass<Update>> VerificationForUpdate(VerificationForUpdateDTO request);
}

public interface IPuppeteerSharpUtilities
{
  public Task TypeAndContinue(IPage page, string button, string? input = null, string? value = null, string? input2 = null, string? value2 = null);
}
