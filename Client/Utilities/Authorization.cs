using LeverX.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public static class Authorization
    {
        private class LoginResult
        {
            public string? Message { get; set; }
            public string? Email { get; set; }
            public string? IdToken { get; set; }
            public bool Success { get; set; } = true;
        }


        private readonly static TimeSpan _tokenExpirationTime;
        private static string? _idToken;
        private static DateTime _tokenReceivingTime;
        private static readonly string _webApiKey;
        private static readonly string[] _credentials;

        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        static Authorization()
        {
            _tokenExpirationTime = new TimeSpan(0, 50, 0);
            _webApiKey = SecretManager.GetSecret("WEB_API_KEY") ??
                throw new Exception("Couldn't find enviromental variable WEB_API_KEY");
            string credentialsString = SecretManager.GetSecret("PEER_JOB_CREDENTIALS") ??
                throw new Exception("Couldn't find enviromental variable PEER_JOB_CREDENTIALS");
            _credentials = credentialsString.Split("#");
        }

        public static async Task<string> GetBearerToken()
        {
            await _semaphore.WaitAsync();
            string token;
            try
            {
                token = await TryUpdateToken();
            }
            finally
            {
                _semaphore.Release();
            }
            return token;
        }

        public static async Task AddAuthorizationHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetBearerToken()}");
        }

        private static async Task<string> TryUpdateToken()
        {
            if (_idToken == null || DateTime.Now - _tokenReceivingTime > _tokenExpirationTime)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post,
                        $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_webApiKey}");
                    msg.Content = new StringContent($"{{\"email\":\"{_credentials[0]}\",\"password\":\"{_credentials[1]}\"" +
                                                    $",\"returnSecureToken\":true}}", Encoding.UTF8, "application/json");
                    var responseMessage = await client.SendAsync(msg);
                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                        throw new Exception("Job service couldn't authorize");

                    _idToken = (await responseMessage.Content.ReadFromJsonAsync<LoginResult>()).IdToken;
                    _tokenReceivingTime = DateTime.Now;
                    AppLog.Log("Updated JWT token");
                }
            }

           return _idToken;
        }

    }
}
