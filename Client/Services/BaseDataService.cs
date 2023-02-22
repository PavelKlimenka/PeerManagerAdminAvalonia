using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Utilities;
using Microsoft.Extensions.Configuration;

namespace Client.Services;

public class BaseDataService
{
    protected readonly string _baseDataApiUri;
    private HttpClient _httpClient;

    public BaseDataService()
    {
        IConfiguration configuration = new ConfigurationManager().AddJsonFile("appsettings.json").Build();
        _baseDataApiUri = configuration.GetConnectionString("BaseDataApi");
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_baseDataApiUri);
    }

    protected async Task<HttpClient> PrepareHttpClient(bool addAuthorization = true)
    {
        if(addAuthorization)
            await Authorization.AddAuthorizationHeader(_httpClient);
        return _httpClient;
    }
}
