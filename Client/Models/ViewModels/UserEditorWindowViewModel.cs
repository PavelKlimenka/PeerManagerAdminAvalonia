
using Client.Utilities;
using System;
using System.ComponentModel;

namespace Client.Models.ViewModels
{
    public class UserEditorWindowViewModel
    {
        public ObservableCollectionMod<RoleState> UserRoles { get; set; } = new();
        public ObservableCollectionMod<DepartmentState> Departments { get; set; } = new();

        public class RoleState
        {
            public string Name { get; set; }
            public bool Active { get; set; }
        }

        public class DepartmentState
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public bool Selected { get; set; }
        }
    }
}
