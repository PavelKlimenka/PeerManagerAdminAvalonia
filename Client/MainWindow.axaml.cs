using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Models.DataModels;
using Client.Models.ViewModels;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client
{
    public partial class MainWindow : Window
    {
        private UserProfileService _userProfileService = new();
        private MainWindowViewModel _dataContext = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _dataContext;
            _ = RefreshRoleUserProfileItems();

            RefreshRolesBtn.Click += RefreshRolesBtn_Click;
            ChangeRolesBtn.Click += RefreshRolesBtn_Click;
        }

        private async Task RefreshRoleUserProfileItems()
        {
            List<UserProfileModel> userProfileModels = await _userProfileService.GetAll();
            _dataContext.RoleUserProfileItems.Clear();
            _dataContext.RoleUserProfileItems.Reset(userProfileModels.Select(x =>
            {
                return new RoleUserProfileItemViewModel()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Roles = x.Roles.Aggregate("", (str, role) => str + $"{role.RoleName}, ", (result) => result.Substring(0, result.Length - 2))
                };
            }));
        }

        private async void RefreshRolesBtn_Click(object? sender, RoutedEventArgs e)
        {
            await RefreshRoleUserProfileItems();
            e.Handled = true;
        }

        private async void ChangeRolesBtn_Click(object? sender, RoutedEventArgs e)
        {
            await RefreshRoleUserProfileItems();
            e.Handled = true;
        }
    }
}