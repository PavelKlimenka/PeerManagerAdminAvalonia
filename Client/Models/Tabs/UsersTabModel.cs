
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Models.ViewModels;
using Client.Services.Interfaces;
using Client.Utilities;
using Client.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models.Tabs
{
    public class UsersTabModel : TabModel
    {
        public List<UserProfileViewModel> UserProfileItems { get; private set; } = new();
        public UserProfileViewModel? SelectedRoleUserProfileItem { get; private set; } = null;

        private RoleUserProfileViewModel _viewModel = new();
        private UserEditorWindow _userEditorWindow = new();

        public UsersTabModel(MainWindow window, IRoleService roleService) : base(window)
        {
            _ = RefreshUserProfileItems();

            _window.RoleUserProfileGrid.DataContext = _viewModel;

            _userEditorWindow.UserRolesChanged += UserEditorWindow_RolesChanged;
            _userEditorWindow.UserDepartmentChanged += UserEditorWindow_DepartmentChanged;
            _window.RefreshBtn.Click += RefreshBtn_Click;
            _window.EditBtn.Click += EditBtn_Click;
            _window.RoleUserProfileGrid.SelectionChanged += UserProfileGrid_SelectionChanged;
            _window.RoleSearchTextBox.AddHandler(InputElement.KeyDownEvent, SearchTextBox_KeyDown, RoutingStrategies.Tunnel);
        }

        public async Task RefreshUserProfileItems()
        {
            await _window.PullData();
            UserProfileItems = new(_window.UserProfiles.Select(x =>
            {
                return new UserProfileViewModel()
                {
                    UserId = x.Id,
                    FullName = x.FullName,
                    Roles = x.Roles.Aggregate("",
                        (str, role) => str + $"{role.RoleName}, ",
                        (result) => string.IsNullOrEmpty(result) ? string.Empty : result.Substring(0, result.Length - 2)),
                    Department = _window.Departments.SingleOrDefault(d => d.Id == x.DepartmentId)?.Name ?? "",
                    ManagedDepartments = x.ManagerOfDepartments.Aggregate(string.Empty,
                        (str, department) => str + $"{department.Name}, ",
                        (result) => string.IsNullOrEmpty(result) ? string.Empty : result.Substring(0, result.Length - 2)),
                };
            }));

            _viewModel.UserProfileItems.Reset(UserProfileItems);

            SetSearchResult(_window.RoleSearchTextBox.Text);

            AppLog.Log($"Refreshed UserProfile items: {UserProfileItems.Count} items");
        }

        private void SetSearchResult(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _viewModel.UserProfileItems.Reset(UserProfileItems);
            }
            else
            {
                _viewModel.UserProfileItems.Reset(UserProfileItems.Where(x =>
                    x.FullName.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.UserId.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Roles.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Department.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.ManagedDepartments.Contains(text, StringComparison.InvariantCultureIgnoreCase)));
            }
        }

        private async void UserEditorWindow_RolesChanged(object? sender, EventArgs e)
        {
            ToggleLoaderIcon(true);
            await Task.Delay(2000);
            await RefreshUserProfileItems();
            _ = _window.ResetPeerManagerUserProfileCache();
            ToggleLoaderIcon(false);
        }

        private async void UserEditorWindow_DepartmentChanged(object? sender, EventArgs e)
        {
            ToggleLoaderIcon(true);
            await Task.Delay(2000);
            await RefreshUserProfileItems();
            _ = _window.ResetPeerManagerUserProfileCache();
            ToggleLoaderIcon(false);
        }

        private async void RefreshBtn_Click(object? sender, RoutedEventArgs e)
        {
            ToggleLoaderIcon(true);

            await RefreshUserProfileItems();
            e.Handled = true;

            ToggleLoaderIcon(false);
        }

        private async void EditBtn_Click(object? sender, RoutedEventArgs e)
        {
            if (SelectedRoleUserProfileItem == null) return;

            await _userEditorWindow.ShowDialog(_window, _window.UserProfiles.Single(x => x.Id == SelectedRoleUserProfileItem.UserId));
        }

        private void UserProfileGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            SelectedRoleUserProfileItem = (UserProfileViewModel)_window.RoleUserProfileGrid.SelectedItem;
        }

        private void SearchTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            SetSearchResult(_window.RoleSearchTextBox.Text);
        }

        private void ToggleLoaderIcon(bool on)
        {
            _window.RoleSyncImage.IsVisible = on;
        }
    }
}
