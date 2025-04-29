using System;

namespace Api.Models.ResultClass;

public class ResultClass<T>
{
    public string Message {get; set;}
    public bool IsSuccess {get; set;}
    public T? Data {get; set;}
    public int StatusCode {get; set;}
}
