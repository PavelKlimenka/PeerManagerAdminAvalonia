using Client.Utilities;

namespace Client.Models.ViewModels
{
    public class RoleUserProfileViewModel
    {
        public ObservableCollectionMod<UserProfileViewModel> UserProfileItems { get; set; } = new();
    }
}
