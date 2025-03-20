namespace VideoConferenceApp.Shared
{
    public abstract record ServiceResponse<T>(bool IsSuccess = false, string? Message = null, T? Data = default);
}
