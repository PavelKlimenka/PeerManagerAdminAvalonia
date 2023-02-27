
using Client.Utilities;

namespace Client.Models.ViewModels
{
    public class DepartmentEditorWindowViewModel
    {
        public ObservableCollectionMod<DepartmentShort> Departments { get; set; } = new();
    }
}
