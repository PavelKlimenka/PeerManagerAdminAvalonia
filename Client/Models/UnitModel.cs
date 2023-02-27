using System;
using System.Collections.Generic;

namespace Client.Models
{
    public class UnitModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<UserProfileModel> Users { get; set; } = default!;
        public string ManagerId { get; set; } = default!;
        public UserProfileModel Manager { get; set; } = default!;

    }
}
