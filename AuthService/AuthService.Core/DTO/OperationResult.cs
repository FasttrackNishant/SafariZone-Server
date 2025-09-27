namespace AuthService.Core.DTO;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }  // for failures
    public T? Data { get; set; }               // optional result data
    public bool IsNew => Data != null && Success;
}