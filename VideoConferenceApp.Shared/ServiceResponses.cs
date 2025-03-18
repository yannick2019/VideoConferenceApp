namespace VideoConferenceApp.Shared
{
    public abstract record ServiceResponses<T>(bool IsSuccess = false, string? Message = null, T? Data = default);
}
