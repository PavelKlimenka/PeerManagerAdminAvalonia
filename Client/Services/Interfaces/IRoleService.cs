using System.Threading.Tasks;

namespace Client.Services.Interfaces;

public interface IRoleService
{
    Task SetRoles(string userId, string[] roles);
}