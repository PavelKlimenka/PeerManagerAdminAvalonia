
using System;

namespace Client.Models
{
    public class UserProfileUpdateModel
    {
        private string _email;

        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateOnly Birthday { get; set; }
        public DateOnly EmploymentDate { get; set; }
        public string? ProjectName { get; set; } = string.Empty;
        public int EnglishLevel { get; set; }
        public string? RoomNumber { get; set; }
        public string? Phone { get; set; }
        public string Email
        {
            get
            {
                return _email = _email.ToLower();
            }
            set
            {
                if (value != null) _email = value;
            }
        }
        public string? Position { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;

        public Guid DepartmentId { get; set; }
        public Guid OfficeLocationId { get; set; }
        
        public Guid? UnitId { get; set; }
    }
}
