using System;
using System.Collections.Generic;

namespace Client.Models;

public class UserProfileModel
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public DateOnly Birthday { get; set; }
    public DateOnly EmploymentDate { get; set; }
    public string? ProjectName { get; set; }
    public int EnglishLevel { get; set; }
    public Guid OfficeLocationId { get; set; }
    public string? RoomNumber { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; }
    public string Position { get; set; }
    public string ImageUrl { get; set; }
    public virtual OfficeLocationModel OfficeLocation { get; set; }

    public List<RoleModel> Roles { get; set; }

    public Guid DepartmentId { get; set; }
    public DepartmentShort Department { get; set; }

    public Guid? UnitId { get; set; }
    public UnitModel Unit { get; set; }

    public Guid? LesUserProfileId { get; set; }

    public ICollection<DepartmentShort> ManagerOfDepartments { get; set; }

}