namespace Shared.DTOs;

public class SearchTodoParametersDto
{
    public string?  Username { get; set; }
    public int?   UserId { get; set; }
    public bool?   CompletedStatus { get; set; }
    public string?  TitleContains { get; set; }

    public SearchTodoParametersDto(string? username, int? userId, bool? completedStatus, string? titleContains)
    {
        Username = username;
        UserId = userId;
        CompletedStatus = completedStatus;
        TitleContains = titleContains;
    }
}