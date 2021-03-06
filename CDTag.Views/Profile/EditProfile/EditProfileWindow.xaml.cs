﻿using CDTag.ViewModels.Profile.EditProfile;
using CDTag.Views.Interfaces.Profile.EditProfile;

namespace CDTag.Views.Profile.EditProfile
{
    /// <summary>
    /// Interaction logic for EditProfileWindow.xaml
    /// </summary>
    public partial class EditProfileWindow : WindowViewBase, IEditProfileWindow
    {
        public EditProfileWindow(IEditProfileViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
