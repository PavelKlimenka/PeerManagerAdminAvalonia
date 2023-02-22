using Client.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Services
{
    public class BasePeerManagerService
    {
        protected readonly string _basePeerManagerApiUri;
        private HttpClient _httpClient;

        public BasePeerManagerService()
        {
            IConfiguration configuration = new ConfigurationManager().AddJsonFile("appsettings.json").Build();
            _basePeerManagerApiUri = configuration.GetConnectionString("PeerManagerApi");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_basePeerManagerApiUri);
        }

        protected async Task<HttpClient> PrepareHttpClient(bool addAuthorization = true)
        {
            if(addAuthorization)
                await Authorization.AddAuthorizationHeader(_httpClient);
            return _httpClient;
        }
    }
}
