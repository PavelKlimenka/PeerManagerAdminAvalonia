using Avalonia.Controls;
using Client.Models;
using Client.Models.Tabs;
using Client.Services;
using Client.Services.Interfaces;
using Client.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public partial class MainWindow : Window
    {
        private UsersTabModel _usersTabModel;
        private IUserProfileService _userProfileService;
        private IDepartmentService _departmentService;
        private ICacheService _cacheService;

        public List<UserProfileModel> UserProfiles { get; set; } = new();
        public List<DepartmentShort> Departments { get; set; } = new();


        public MainWindow()
        {
            InitializeComponent();

            _userProfileService = new UserProfileService();
            _cacheService = new CacheService();
            _departmentService = new DepartmentService();
            #if DEBUG
            LogTabItem.IsVisible = true;
            AppLog.Initialize(LogTextBox);
            #endif

            _usersTabModel = new(this, new RoleService());
        }

        public async Task PullData()
        {
            await Task.WhenAll(PullUserProfiles(), PullDepartments());
        }

        public async Task PullUserProfiles()
        {
            UserProfiles = await _userProfileService.GetAll();
        }

        public async Task PullDepartments()
        {
            Departments = await _departmentService.GetDepartmentsShortInfo();
        }

        public async Task ResetPeerManagerUserProfileCache()
        {
            await _cacheService.ResetPeerManagerUserProfileCache();
        }
    }
}