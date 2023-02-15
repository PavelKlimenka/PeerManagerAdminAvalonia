using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models.DataModels;
using Client.Services.Interfaces;
using Client.Utilities;

namespace Client.Services;

public sealed class UserProfileService : BaseService, IUserProfileService 
{
    private const string Endpoint = "user-profiles";

    public async Task<List<UserProfileModel>> GetAll()
    {
        try
        {
            await Authorization.AddAuthorizationHeader(_httpClient);
            return await _httpClient.GetFromJsonAsync<List<UserProfileModel>>(Endpoint);
        }
        catch(Exception ex) 
        {
            AppLog.LogError(ex.Message);
            throw;
        }
    }
}