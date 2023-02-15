using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Models.DataModels;
using Client.Models.ViewModels;
using Client.Services;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _dataContext = new();

        public List<UserProfileModel> UserProfiles { get; private set; } = new();
        public List<RoleUserProfileModel> RoleUserProfileItems { get; private set; } = new();
        public RoleUserProfileModel? SelectedRoleUserProfileItem { get; private set; } = null;

        private UserProfileService _userProfileService = new();
        private RoleService _roleService = new();

        private RoleEditorWindow _roleEditorWindow = new();

        public MainWindow()
        {
            InitializeComponent();

            #if DEBUG
            LogTabItem.IsVisible = true;
            AppLog.Initialize(LogTextBox);
            #endif

            DataContext = _dataContext;
            _ = PullRoles();
            _ = RefreshRoleUserProfileItems();

            RefreshRolesBtn.Click += RefreshRolesBtn_Click;
            ChangeRolesBtn.Click += ChangeRolesBtn_Click;
            RoleUserProfileGrid.SelectionChanged += RoleUserProfileGrid_SelectionChanged;
            _roleEditorWindow.RolesChanged += RoleEditorWindow_RolesChanged;
            RoleSearchTextBox.AddHandler(KeyDownEvent, RoleSearchTextBox_KeyDown, RoutingStrategies.Tunnel);
        }

        public async Task RefreshRoleUserProfileItems()
        {
            UserProfiles = await _userProfileService.GetAll();
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

            _dataContext.RoleUserProfileItems.Reset(RoleUserProfileItems);

            AppLog.Log($"Refreshed RoleUserProfile items: {UserProfiles.Count} items");
        }

        private async Task PullRoles()
        {
            _roleEditorWindow.Roles = await _roleService.GetRoles();
            AppLog.Log($"Pulled roles: {_roleEditorWindow.Roles.Count} roles");
        }

        private async void RefreshRolesBtn_Click(object? sender, RoutedEventArgs e)
        {
            await RefreshRoleUserProfileItems();
            e.Handled = true;
        }

        private async void ChangeRolesBtn_Click(object? sender, RoutedEventArgs e)
        {
            if (SelectedRoleUserProfileItem == null) return;

            _roleEditorWindow.Position = this.Position + new Avalonia.PixelPoint((int)(this.Width - _roleEditorWindow.Width) / 2, (int)(this.Height - _roleEditorWindow.Height) / 2);
            await _roleEditorWindow.ShowDialog(this, UserProfiles.Single(x => x.Id == SelectedRoleUserProfileItem.Id));
        }

        private void RoleUserProfileGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e) 
        {
            e.Handled = true;
            SelectedRoleUserProfileItem = (RoleUserProfileModel)RoleUserProfileGrid.SelectedItem;
        }

        private void RoleSearchTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            string text = RoleSearchTextBox.Text;

            if(string.IsNullOrEmpty(text))
            {
                _dataContext.RoleUserProfileItems.Reset(RoleUserProfileItems);
            }
            else
            {
                _dataContext.RoleUserProfileItems.Reset(RoleUserProfileItems.Where(x =>
                    x.FullName.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Id.Contains(text, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Roles.Contains(text, StringComparison.InvariantCultureIgnoreCase)));
            }
        }

        private async void RoleEditorWindow_RolesChanged(object? sender, EventArgs e)
        {
            await Task.Delay(2000);
            await RefreshRoleUserProfileItems();
        }

    }
}