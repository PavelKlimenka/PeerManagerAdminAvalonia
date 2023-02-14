using System.Collections.Generic;

namespace Client.Models.DataModels;

public class UserProfileModel
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public Department Department { get; set; }
    public List<RoleModel> Roles { get; set; }
}