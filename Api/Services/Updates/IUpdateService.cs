using System;
using Api.Models.ResultClass;

namespace Api.Services.Updates;

public interface IUpdateService
{
  public Task<ResultClass<Update>> LoginForUpdate(LoginForUpdateDTO request);
  public Task<ResultClass<Update>> VerificationForUpdate(VerificationForUpdateDTO request);
}
