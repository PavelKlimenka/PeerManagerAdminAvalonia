
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Models.ViewModels;
using Client.Services.Interfaces;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models.DataModels
{
    public class RolesTabModel : TabModel
    {
        public List<RoleUserProfileModel> RoleUserProfileItems { get; private set; } = new();
        public RoleUserProfileModel? SelectedRoleUserProfileItem { get; private set; } = null;
        private RoleUserProfileViewModel _viewModel = new();

        private RoleEditorWindow _roleEditorWindow = new();

        private IRoleService _roleService;

        public RolesTabModel(MainWindow _window, IRoleService roleService) : base(_window)
        {
            _roleService = roleService;

            _ = PullRoles();
            _ = RefreshRoleUserProfileItems();

            _window.RoleUserProfileGrid.DataContext = _viewModel;

            _roleEditorWindow.RolesChanged += RoleEditorWindow_RolesChanged;
            _window.RefreshRolesBtn.Click += RefreshRolesBtn_Click;
            _window.ChangeRolesBtn.Click += ChangeRolesBtn_Click;
            _window.RoleUserProfileGrid.SelectionChanged += RoleUserProfileGrid_SelectionChanged;
            _window.RoleSearchTextBox.AddHandler(InputElement.KeyDownEvent, RoleSearchTextBox_KeyDown, RoutingStrategies.Tunnel);
        }

        private async Task PullRoles()
        {
            _roleEditorWindow.Roles = await _roleService.GetRoles();
            AppLog.Log($"Pulled roles: {_roleEditorWindow.Roles.Count} roles");
        }

        public async Task RefreshRoleUserProfileItems()
        {
            await _window.RefreshUserProfiles();
            List<UserProfileModel> UserProfiles = _window.UserProfiles;
            RoleUserProfileItems = new(UserProfiles.Select(x =>
            {
                return new RoleUserProfileModel()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Roles = x.Roles.Aggregate("", 
                        (str, role) => str + $"{role.RoleName}, ",
                        (result) => string.IsNullOrEmpty(result)? string.Empty : result.Substring(0, result.Length - 2))
                };
            }));

            _viewModel.RoleUserProfileItems.Reset(RoleUserProfileItems);

            SetRoleSearchResults(_window.RoleSearchTextBox.Text);

            AppLog.Log($"Refreshed RoleUserProfile items: {UserProfiles.Count} items");
        }

        private void SetRoleSearchResults(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                _viewModel.RoleUserProfileItems.Reset(RoleUserProfileItems);
            }
            else
            {
                _viewModel.RoleUserProfileItems.Reset(RoleUserProfileItems.Where(x =>
                    x.FullName.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Id.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Roles.Contains(text, StringComparison.InvariantCultureIgnoreCase)));
            }
        }

        private async void RoleEditorWindow_RolesChanged(object? sender, EventArgs e)
        {
            ToggleLoaderIcon(true);
            await Task.Delay(2000);
            await RefreshRoleUserProfileItems();
            _ = _window.ResetPeerManagerUserProfileCache();
            ToggleLoaderIcon(false);
        }

        private async void RefreshRolesBtn_Click(object? sender, RoutedEventArgs e)
        {
            ToggleLoaderIcon(true);

            await RefreshRoleUserProfileItems();
            e.Handled = true;

            ToggleLoaderIcon(false);
        }

        private async void ChangeRolesBtn_Click(object? sender, RoutedEventArgs e)
        {
            if (SelectedRoleUserProfileItem == null) return;

            _roleEditorWindow.Position = _window.Position + new Avalonia.PixelPoint((int)(_window.Width - _roleEditorWindow.Width) / 2, (int)(_window.Height - _roleEditorWindow.Height) / 2);
            await _roleEditorWindow.ShowDialog(_window, _window.UserProfiles.Single(x => x.Id == SelectedRoleUserProfileItem.Id));
        }

        private void RoleUserProfileGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e) 
        {
            e.Handled = true;
            SelectedRoleUserProfileItem = (RoleUserProfileModel)_window.RoleUserProfileGrid.SelectedItem;
        }

        private void RoleSearchTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            SetRoleSearchResults(_window.RoleSearchTextBox.Text);
        }

        private void ToggleLoaderIcon(bool on)
        {
            _window.RoleSyncImage.IsVisible = on;
        }
    }
}
