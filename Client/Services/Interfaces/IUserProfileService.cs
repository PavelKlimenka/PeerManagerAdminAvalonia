using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models.DataModels;

namespace Client.Services.Interfaces;

public interface IUserProfileService
{
    Task<List<UserProfileModel>> GetAll();
}