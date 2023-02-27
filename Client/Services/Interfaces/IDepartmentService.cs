using Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentModel>> GetDepartmentsFullInfo();
        Task<List<DepartmentShort>> GetDepartmentsShortInfo();
        Task SetDepartment(string userId, Guid departmentId);
    }
}
