namespace Models.DTOs;

public class ErrorDto
{
    public string? Message { get; set; }

    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}