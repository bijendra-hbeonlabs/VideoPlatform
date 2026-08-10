namespace VideoDisplayPlatform.Contracts.Dtos;

public record PagedResult<T>
{
    public List<T> Items { get; init; } = new();
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}

public record ApiResponse<T>
{
    public bool Success { get; init; } = true;
    public T? Data { get; init; }
    public ApiError? Error { get; init; }
    public string TraceId { get; init; } = Guid.NewGuid().ToString();

    public static ApiResponse<T> Ok(T data) => new() { Success = true, Data = data };
    public static ApiResponse<T> Fail(string code, string message) => new() { Success = false, Error = new ApiError(code, message) };
}

public record ApiResponse
{
    public bool Success { get; init; } = true;
    public ApiError? Error { get; init; }
    public string TraceId { get; init; } = Guid.NewGuid().ToString();

    public static ApiResponse Ok() => new() { Success = true };
    public static ApiResponse Fail(string code, string message) => new() { Success = false, Error = new ApiError(code, message) };
}

public record ApiError(string Code, string Message);

public record PagedQuery(int PageIndex = 1, int PageSize = 20, string? Search = null, string? SortBy = null, bool IsDescending = false);
