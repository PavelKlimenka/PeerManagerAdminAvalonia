
using Client.Utilities;

namespace Client.Models.ViewModels
{
    public class DepartmentUserProfileViewModel
    {
        public ObservableCollectionMod<DepartmentUserProfileModel> DepartmentUserProfileItems { get; set; } = new();
    }
}
