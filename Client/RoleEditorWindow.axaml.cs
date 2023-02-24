using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Models;
using Client.Models.ViewModels;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Client.Models.ViewModels.RoleEditorWindowViewModel;

namespace Client
{
    public partial class RoleEditorWindow : Window
    {
        public List<RoleModel> Roles { get; set; } = new();

        private RoleEditorWindowViewModel _dataContext = new();
        private UserProfileModel? _selectedUserProfile = null;

        private RoleService _roleService = new();

        public event EventHandler RolesChanged;

        public RoleEditorWindow()
        {
            InitializeComponent();
            DataContext = _dataContext;
            this.Opened += RoleEditorWindow_Opened;
            RoleItemsControl.DataContext = _dataContext.UserRoles;
            ApplyBtn.Click += ApplyBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }
        public void Show(UserProfileModel selectedUserProfile)
        {
            _selectedUserProfile = selectedUserProfile;
            this.Show();
        }

        public void Show(Window owner, UserProfileModel selectedUserProfile)
        {
            _selectedUserProfile = selectedUserProfile;
            this.Show(owner);
        }

        public async Task ShowDialog(Window owner, UserProfileModel selectedUserProfile)
        {
            _selectedUserProfile = selectedUserProfile;
            await this.ShowDialog(owner);
        }

        private void RoleEditorWindow_Opened(object? sender, EventArgs e)
        {
            _dataContext.UserRoles.Clear();
            List<RoleState> userRoles = new();
            foreach(RoleModel role in Roles)
            {
                bool active = false;
                if(_selectedUserProfile.Roles.Contains(role))
                    active = true;
                userRoles.Add(new() { Name = role.RoleName, Active = active });
            }
            _dataContext.UserRoles.Reset(userRoles);
        }

        private void ApplyBtn_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _ = _roleService.SetRoles(_selectedUserProfile.Id, _dataContext.UserRoles.Where(x => x.Active).Select(x => x.Name).ToArray());
            RolesChanged?.Invoke(this, EventArgs.Empty);
            this.Hide();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.Hide();
        }
    }
}
