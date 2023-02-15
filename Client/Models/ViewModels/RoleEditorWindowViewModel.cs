
using Client.Utilities;

namespace Client.Models.ViewModels
{
    public class RoleEditorWindowViewModel
    {
        public ObservableCollectionMod<RoleState> UserRoles { get; set; } = new();

        public class RoleState
        {
            public string Name { get; set; } = string.Empty;
            public bool Active { get; set; }
        }
    }
}
