namespace AuthServer.Core.Dtos;

public class ErrorDto
{
    public List<String> Errors { get; private set; }
    public bool IsShow { get; private set; }


    public ErrorDto()
    {
        Errors = new List<string>();
    }

    public ErrorDto(string errors, bool isShow)
    {
        Errors.Add(errors);
        IsShow = isShow;
    }

    public ErrorDto(List<string> errors, bool isIsShow)
    {
        Errors = errors;
        IsShow = isIsShow;
    }
}
