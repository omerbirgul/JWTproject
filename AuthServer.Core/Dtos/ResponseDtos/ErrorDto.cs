namespace AuthServer.Core.Dtos.ResponseDtos;

public class ErrorDto
{
    public List<String> Errors { get;  private set; } = new();


    public ErrorDto(){}

    public ErrorDto(string error)
    {
        Errors.Add(error);
    }

    public ErrorDto(List<string> errors)
    {
        Errors = errors ?? new List<string>();
    }
}
