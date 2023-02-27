using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Models;
using Client.Models.ViewModels;
using Client.Services;
using Client.Services.Interfaces;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Client.Models.ViewModels.UserEditorWindowViewModel;

namespace Client.Windows
{
    public partial class UserEditorWindow : Window
    {
        private UserEditorWindowViewModel _viewModel = new();
        private UserProfileModel? _selectedUserProfile = null;

        private IRoleService _roleService;
        private IDepartmentService _departmentService;
        public event EventHandler? UserRolesChanged;
        public event EventHandler? UserDepartmentChanged;

        public UserEditorWindow()
        {
            InitializeComponent();
            _roleService = new RoleService();
            _departmentService = new DepartmentService();
            DataContext = _viewModel;
            this.Opened += RoleEditorWindow_Opened;
            RoleItemsControl.DataContext = _viewModel.UserRoles;
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

        private async void RoleEditorWindow_Opened(object? sender, EventArgs e)
        {
            if (_selectedUserProfile == null)
            {
                AppLog.LogError("Trying to user editor window when selected user is 'null'");
                return;
            }

            List<RoleModel> roles = await _roleService.GetRoles();
            _viewModel.UserRoles.Reset(roles.Select(x => new RoleState() { Name = x.RoleName, Active = _selectedUserProfile.Roles.Contains(x)}));

            List<DepartmentShort> departments = await _departmentService.GetDepartmentsShortInfo();
            _viewModel.Departments.Reset(departments.Select(x => new DepartmentState() { Id = x.Id, Name = x.Name, Selected = _selectedUserProfile.DepartmentId == x.Id}));
            DepartmentListBox.SelectedItem = _viewModel.Departments.SingleOrDefault(x => x.Id == _selectedUserProfile.DepartmentId);
        }

        private void ApplyBtn_Click(object? sender, RoutedEventArgs e)
        {
            if (_selectedUserProfile == null)
            {
                AppLog.LogError("Trying to apply user modification when selected user is 'null'");
                return;
            }

            int currentTab = TabControl.SelectedIndex;

            switch(currentTab)
            {
                case 0:
                    {
                        _ = _roleService.SetRoles(_selectedUserProfile.Id, _viewModel.UserRoles.Where(x => x.Active).Select(x => x.Name).ToArray());
                        UserRolesChanged?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                case 1:
                    {
                        if (DepartmentListBox.SelectedItem == null) break;
                        _ = _departmentService.SetDepartment(_selectedUserProfile.Id, ((DepartmentState)DepartmentListBox.SelectedItem).Id);
                        UserDepartmentChanged?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                default:
                    AppLog.LogError("Tab index is incorrect");
                    break;
            }

            e.Handled = true;
            this.Hide();
        }

        private void CancelBtn_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.Hide();
        }
    }
}
