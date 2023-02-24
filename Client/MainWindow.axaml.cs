using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Models;
using Client.Models.ViewModels;
using Client.Services;
using Client.Services.Interfaces;
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
        private RolesTabModel _rolesTabModel;
        private IUserProfileService _userProfileService;
        private ICacheService _cacheService;

        public List<UserProfileModel> UserProfiles { get; set; } = new();


        public MainWindow()
        {
            InitializeComponent();

            _userProfileService = new UserProfileService();
            _cacheService = new CacheService(); 
            #if DEBUG
            LogTabItem.IsVisible = true;
            AppLog.Initialize(LogTextBox);
            #endif

            _rolesTabModel = new(this, new RoleService());
        }

        public async Task RefreshUserProfiles()
        {
            UserProfiles = await _userProfileService.GetAll();
        }

        public async Task ResetPeerManagerUserProfileCache()
        {
            await _cacheService.ResetPeerManagerUserProfileCache();
        }
    }
}