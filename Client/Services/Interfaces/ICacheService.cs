
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface ICacheService
    {
        Task ResetPeerManagerUserProfileCache();
    }
}
