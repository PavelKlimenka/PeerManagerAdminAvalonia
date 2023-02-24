using Client.Utilities;
using System.Collections.Generic;

namespace Client.Models.ViewModels
{
    public class RoleUserProfileViewModel
    {
        public ObservableCollectionMod<RoleUserProfileModel> RoleUserProfileItems { get; set; } = new();
    }
}
