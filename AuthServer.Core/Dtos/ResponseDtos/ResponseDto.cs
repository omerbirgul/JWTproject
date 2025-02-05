using System.Net;
using System.Text.Json.Serialization;

namespace AuthServer.Core.Dtos.ResponseDtos;

public class ResponseDto<T> where T : class
{
    public T? Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public ErrorDto? Error { get; set; }
    [JsonIgnore] public bool IsSuccessful => Error == null;
    [JsonIgnore] public bool IsFail => !IsSuccessful;
    [JsonIgnore] public string? UrlAsCreated { get; set; }


    public static ResponseDto<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ResponseDto<T>() { Data = data, StatusCode = statusCode};
    }

    public static ResponseDto<T> Success(HttpStatusCode statusCode = HttpStatusCode.NoContent)
    {
        return new ResponseDto<T>() {StatusCode = statusCode};
    }

    public static ResponseDto<T> SuccessAsCreated(T data, string urlAsCreated)
    {
        return new ResponseDto<T>() { Data = data, StatusCode = HttpStatusCode.Created, UrlAsCreated = urlAsCreated};
    }

    public static ResponseDto<T> Fail(ErrorDto errorDto, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ResponseDto<T>() { Error = errorDto, StatusCode = statusCode};
    }

    public static ResponseDto<T> Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var errorDto = new ErrorDto(errorMessage);
        return new ResponseDto<T>() { Error = errorDto, StatusCode = statusCode};
    }
}