using System;
using System.Collections.Generic;

namespace Client.Models
{
    public class DepartmentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<UserProfileModel> UserProfiles { get; set; } = new();
        public List<UserProfileModel> Managers { get; set; } = new();
    }
}
