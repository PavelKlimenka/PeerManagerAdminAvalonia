using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models.DataModels;
using Client.Services.Interfaces;

namespace Client.Services;

public sealed class RoleService : BaseService, IRoleService
{
    private const string Endpoint = "role/setRoles";

    public async Task SetRoles(string userId, string[] roles)
    {
        RoleSetModel model = new() { UserId = userId, Roles = roles };
        await _httpClient.PostAsJsonAsync<RoleSetModel>(Endpoint, model);
    }
}