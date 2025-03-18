namespace VideoConferenceApp.Shared.Authentication.Responses
{
    public record LoginUserResponse(string JwtToken) : ServiceResponses<string>;
}
