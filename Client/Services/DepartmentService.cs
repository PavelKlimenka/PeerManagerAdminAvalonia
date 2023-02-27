using Client.Models;
using Client.Models.Mappers;
using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class DepartmentService : BaseDataService, IDepartmentService
    {
        private const string EndpointGetFullInfo = "departments/fullInfo";
        private const string EndpointGetShortInfo = "departments";
        private const string EndpointUserProfiles = "user-profiles";

        public async Task<List<DepartmentModel>> GetDepartmentsFullInfo()
        {
            HttpClient client = await PrepareHttpClient();
            List<DepartmentModel>? result = await client.GetFromJsonAsync<List<DepartmentModel>>(EndpointGetFullInfo);
            if (result == null) throw new Exception("Failed to fetch full department info");
            return result;
        }

        public async Task<List<DepartmentShort>> GetDepartmentsShortInfo()
        {
            HttpClient client = await PrepareHttpClient();
            List<DepartmentShort>? result = await client.GetFromJsonAsync<List<DepartmentShort>>(EndpointGetShortInfo);
            if (result == null) throw new Exception("Failed to fetch short department info");
            return result;
        }

        public async Task SetDepartment(string userId, Guid departmentId)
        {
            HttpClient client = await PrepareHttpClient();
            UserProfileModel? user = await client.GetFromJsonAsync<UserProfileModel>($"{EndpointUserProfiles}/{userId}");
            if (user == null) throw new Exception($"Failed to fetch user profile. userId={userId}");
            if (user.DepartmentId == departmentId) return;
            user.DepartmentId = departmentId;
            _ = client.PutAsJsonAsync(EndpointUserProfiles, user.ToUpdateModel());
        }
    }
}
