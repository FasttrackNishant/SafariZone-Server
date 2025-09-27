using SafariZone.Server.Common.Enums;

namespace SafariZone.Server.Common.Response;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    private ApiResponse(int statusCode, bool success, string message, T? data = default)
    {
        StatusCode = statusCode;
        Success = success;
        Message = message;
        Data = data;
    }

    // --- ✅ Helpers for Success ---
    public static ApiResponse<T> Ok(T data, string message = "Success") =>
        new((int)ApiStatusCode.Ok, true, message, data);

    public static ApiResponse<T> Created(T? data, string message = "Resource created") =>
        new((int)ApiStatusCode.Created, true, message, data);

    // --- ❌ Helpers for Failures ---
    public static ApiResponse<T> BadRequest(string message = "Bad request") =>
        new((int)ApiStatusCode.BadRequest, false, message);

    public static ApiResponse<T> Unauthorized(string message = "Unauthorized") =>
        new((int)ApiStatusCode.Unauthorized, false, message);

    public static ApiResponse<T> Forbidden(string message = "Forbidden") =>
        new((int)ApiStatusCode.Forbidden, false, message);

    public static ApiResponse<T> NotFound(string message = "Not found") =>
        new((int)ApiStatusCode.NotFound, false, message);

    public static ApiResponse<T> Conflict(string message = "Conflict") =>
        new((int)ApiStatusCode.Conflict, false, message);

    public static ApiResponse<T> Fail(string message = "Internal server error") =>
        new((int)ApiStatusCode.InternalServerError, false, message);
}