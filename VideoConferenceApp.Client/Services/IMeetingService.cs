using System.Net.Http.Json;
using VideoConferenceApp.Client.Extensions;
using VideoConferenceApp.Shared.Meeting.Requests;
using VideoConferenceApp.Shared.Meeting.Responses;

namespace VideoConferenceApp.Client.Services
{
    public interface IMeetingService
    {
        Task<CreateMeetingResponse?> CreateMeeting(CreateMeetingRequest meeting);
        Task<GetMeetingsResponse?> GetMeetings(string hostId);
        Task<GetRecentMeetingsResponse?> GetRecentMeetings(string hostId);
    }

    public class MeetingService(IHttpExtension httpExtension) : IMeetingService
    {
        public async Task<CreateMeetingResponse?> CreateMeeting(CreateMeetingRequest meeting)
        {
            var result = await (await httpExtension.GetPrivateClient()).PostAsJsonAsync("meeting/create", meeting);
            return await result.Content.ReadFromJsonAsync<CreateMeetingResponse>();
        }

        public async Task<GetMeetingsResponse?> GetMeetings(string hostId)
        {
            var result = await (await httpExtension.GetPrivateClient()).GetAsync($"meeting/{hostId}");
            return await result.Content.ReadFromJsonAsync<GetMeetingsResponse>();
        }

        public async Task<GetRecentMeetingsResponse?> GetRecentMeetings(string hostId)
        {
            var result = await (await httpExtension.GetPrivateClient()).GetAsync($"meeting/recent/{hostId}");
            return await result.Content.ReadFromJsonAsync<GetRecentMeetingsResponse>();
        }
    }
}
