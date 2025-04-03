using System.Net.Http.Headers;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace VideoConferenceApp.Client.Extensions
{
    public interface IHttpExtension
    {
        HttpClient GetPublicClient();
        Task<HttpClient> GetPrivateClient();
    }

    public class HttpExtension(IHttpClientFactory httpClientFactory, 
        ILocalStorageService localStorageService, IConfiguration configuration) : IHttpExtension
    {
        HttpClient CreateClient() => httpClientFactory.CreateClient(configuration["HttpClient:Name"]!);

        public async Task<HttpClient> GetPrivateClient()
        {
            var client = CreateClient();
            string key = configuration["Token:Key"]!;
            if (key == null) return client;

            string token = await localStorageService.GetItemAsStringAsync(key!);
            if (key != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            return client;
        }

        public HttpClient GetPublicClient() => CreateClient();
    }
}
