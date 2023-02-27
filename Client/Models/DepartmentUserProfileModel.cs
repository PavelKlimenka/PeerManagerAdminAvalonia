
using System;

namespace Client.Models
{
    public class DepartmentUserProfileModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
