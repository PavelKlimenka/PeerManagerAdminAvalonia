using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Models;
using Client.Models.ViewModels;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Windows
{
    public partial class DepartmentEditorWindow : Window
    {
        public List<DepartmentShort> Departments { get; set; } = new();

        private DepartmentEditorWindowViewModel _viewModel = new();
        private UserProfileModel? _selectedUserProfile = null;

        private DepartmentService _departmentService = new();

        public event EventHandler DepartmentsChanged;

        public DepartmentEditorWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
            this.Opened += RoleEditorWindow_Opened;
            DepartmentListBox.DataContext = _viewModel;
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
            _viewModel.Departments.Reset(Departments);
            DepartmentListBox.SelectedItem = _viewModel.Departments.Single(x => x.Id == _selectedUserProfile?.DepartmentId);
        }

        private void ApplyBtn_Click(object? sender, RoutedEventArgs e)
        {
            e.Handled = true;
            DepartmentShort? selectedDepartment = (DepartmentShort?)DepartmentListBox.SelectedItem;
            if (selectedDepartment == null) return;
            _ = _departmentService.SetDepartment(_selectedUserProfile.Id, ((DepartmentShort)DepartmentListBox.SelectedItem).Id);
            DepartmentsChanged?.Invoke(this, EventArgs.Empty);
            this.Hide();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.Hide();
        }
    }
}
