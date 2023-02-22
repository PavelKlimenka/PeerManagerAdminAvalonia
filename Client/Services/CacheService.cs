using Client.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Services
{
    public class CacheService : BasePeerManagerService, Interfaces.ICacheService
    {
        private const string ResetUserProfileCacheEndpoint = "Maintenance/ClearCache/UserProfile";

        public async Task ResetPeerManagerUserProfileCache()
        {
            try
            {
                HttpClient httpClient = await PrepareHttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(ResetUserProfileCacheEndpoint);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to reset user profile cache. Response status code: '{response.StatusCode}'");
                AppLog.Log("UserProfile cache was reset");
            }
            catch(Exception ex) 
            {
                AppLog.LogError(ex.Message);
            }
        }
    }
}
