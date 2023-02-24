using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;
using Client.Services.Interfaces;
using Client.Utilities;

namespace Client.Services;

public sealed class UserProfileService : BaseDataService, IUserProfileService 
{
    private const string Endpoint = "user-profiles";

    public async Task<List<UserProfileModel>> GetAll()
    {
        try
        {
            HttpClient httpClient = await PrepareHttpClient();
            return await httpClient.GetFromJsonAsync<List<UserProfileModel>>(Endpoint);
        }
        catch(Exception ex) 
        {
            AppLog.LogError(ex.Message);
            throw;
        }
    }
}