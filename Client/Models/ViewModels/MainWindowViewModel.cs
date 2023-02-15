using Client.Models.DataModels;
using Client.Utilities;
using System.Collections.ObjectModel;

namespace Client.Models.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollectionMod<RoleUserProfileModel> RoleUserProfileItems { get; set; } = new();
    }
}
