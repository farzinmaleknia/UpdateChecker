using System;
using Api.Models.ResultClass;

namespace Api.Services.Updates;

public class UpdateService : IUpdateService
{
    public async Task<ResultClass<Update>> FetchAllUpdates()
    {
        var result = new ResultClass<Update>();

        var res = new Update(){WebContent = "new contet from repository"};

        result.Data = res;
        result.IsSuccess = true;

        return result;
    }
}
