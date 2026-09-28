using System.Net.Http.Headers;
using System.Text;

namespace MAUI_QuestBoard.DataAccess;

// Calls our own QuestBoardWebService API to verify credentials via HTTP Basic Authentication
public class AuthWebService
{
    // Use a single BaseAddress for all platforms, HTTP only
    private const string BaseAddress = "https://localhost:44372";

    public async Task<bool> AuthenticateAsync(string email, string password)
    {
        try
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(BaseAddress),
                Timeout = TimeSpan.FromSeconds(10)
            };

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}:{password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var response = await client.GetAsync("api/auth/verify");

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            // Service unreachable (not running, wrong port, no network, etc.)
            return false;
        }
    }
}
