using System;
using System.Net.Http;
using Client.Utilities;
using Microsoft.Extensions.Configuration;

namespace Client.Services;

public class BaseService
{
    protected readonly string _baseDataApiUri;
    protected HttpClient _httpClient;

    public BaseService()
    {
        IConfiguration configuration = new ConfigurationManager().AddJsonFile("appsettings.json").Build();
        _baseDataApiUri = configuration.GetConnectionString("BaseDataApi");
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_baseDataApiUri);
    }
}
