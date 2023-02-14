using Client.Utilities;
using System.Collections.ObjectModel;

namespace Client.Models.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollectionMod<RoleUserProfileItemViewModel> RoleUserProfileItems { get; set; } = new();
    }
}
