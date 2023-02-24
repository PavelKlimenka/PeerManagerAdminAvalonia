using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;
using Client.Services.Interfaces;
using Client.Utilities;

namespace Client.Services;

public sealed class RoleService : BaseDataService, IRoleService
{
    private const string EndpointSet = "role/setRoles";
    private const string EndpointGet = "role";

    public async Task SetRoles(string userId, string[] roles)
    {
        RoleSetModel model = new() { UserId = userId, Roles = roles };
        try
        {
            HttpClient httpClient = await PrepareHttpClient();
            await httpClient.PostAsJsonAsync<RoleSetModel>(EndpointSet, model);
            AppLog.Log($"Roles applied. User: {userId}");
        }
        catch(Exception ex)
        {
            AppLog.LogError(ex.Message);
            throw;
        }
    }

    public async Task<List<RoleModel>> GetRoles()
    {
        try
        {
            HttpClient httpClient = await PrepareHttpClient();
            return await httpClient.GetFromJsonAsync<List<RoleModel>>(EndpointGet);
        }
        catch(Exception ex)
        {
            AppLog.LogError(ex.Message);
            throw;
        }
    }
}