using System.IO;
using System.Windows;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Dispatcher;
using CDTag.Model.Profile;
using CDTag.ViewModels.Profile.EditProfile;
using NUnit.Framework;

namespace CDTag.ViewModel.Tests.Profile.EditProfile
{
    [TestFixture]
    public class EditProfileViewModelTests
    {
        private const string _profileName = "unittests";
        private const string _profileName2 = "unittests2";
        private const string _profileName3 = "unittests3";
        private string _defaultProfile;
        private string _unitTestsProfile;
        private string _unitTestsProfile2;
        private string _unitTestsProfile3;
        private string _unitTestsNFO;

        [TestFixtureSetUp]
        public void Setup()
        {
            IoC.ClearAllRegistrations();

            IoC.RegisterInstance<IDispatcher>(new UnitTestDispatcher());
            IoC.RegisterInstance<IPathService>(new UnitTestPathService());
            IoC.RegisterInstance<IDialogService>(new UnitTestDialogService());

            string profileDirectory = IoC.Resolve<IPathService>().ProfileDirectory;
            _defaultProfile = Path.Combine(profileDirectory, "Default.cfg");
            _unitTestsProfile = Path.Combine(profileDirectory, _profileName + ".cfg");
            _unitTestsProfile2 = Path.Combine(profileDirectory, _profileName2 + ".cfg");
            _unitTestsProfile3 = Path.Combine(profileDirectory, _profileName3 + ".cfg");
            _unitTestsNFO = Path.Combine(profileDirectory, _profileName + ".nfo");

            DeleteUnitTestsProfile();
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            DeleteUnitTestsProfile();
        }

        private void DeleteUnitTestsProfile()
        {
            if (File.Exists(_defaultProfile))
                File.Delete(_defaultProfile);
            if (File.Exists(_unitTestsProfile))
                File.Delete(_unitTestsProfile);
            if (File.Exists(_unitTestsProfile2))
                File.Delete(_unitTestsProfile2);
            if (File.Exists(_unitTestsProfile3))
                File.Delete(_unitTestsProfile3);
            if (File.Exists(_unitTestsNFO))
                File.Delete(_unitTestsNFO);
        }

        [Test]
        public void ConstructorTest()
        {
            // Arrange/Act
            DeleteUnitTestsProfile();

            EditProfileViewModel editProfileViewModel = IoC.Resolve<EditProfileViewModel>();

            // Assert
            Assert.That(editProfileViewModel, Is.Not.Null, "editProfileViewModel");
            Assert.That(editProfileViewModel.CopyProfileCommand, Is.Not.Null, "editProfileViewModel.CopyProfileCommand");
            Assert.That(editProfileViewModel.DeleteProfileCommand, Is.Not.Null, "editProfileViewModel.DeleteProfileCommand");
            Assert.That(editProfileViewModel.NewProfileCommand, Is.Not.Null, "editProfileViewModel.NewProfileCommand");
            Assert.That(editProfileViewModel.RenameProfileCommand, Is.Not.Null, "editProfileViewModel.RenameProfileCommand");
            Assert.That(editProfileViewModel.Profiles, Is.Not.Null, "editProfileViewModel.Profiles");
            Assert.That(editProfileViewModel.Profiles.Count, Is.EqualTo(1), "editProfileViewModel.Profiles.Count");
            Assert.That(editProfileViewModel.Profile, Is.Not.Null, "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Profile.ProfileName, Is.EqualTo("Default"), "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Header, Is.EqualTo("Select Profile"), "editProfileViewModel.Header");
            Assert.That(editProfileViewModel.WindowTitle, Is.EqualTo("Edit Profile - Default"), "editProfileViewModel.WindowTitle");
            Assert.That(File.Exists(_defaultProfile), Is.True, "File.Exists(_defaultProfile)");
        }

        [Test]
        public void DeleteProfileTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            UserProfile profile = new UserProfile();
            profile.ProfileName = _profileName;
            profile.Save();

            profile = new UserProfile();
            profile.ProfileName = _profileName2;
            profile.Save();

            profile = new UserProfile();
            profile.ProfileName = _profileName3;
            profile.Save();

            EditProfileViewModel editProfileViewModel = IoC.Resolve<EditProfileViewModel>();
            bool confirmationShown = false;
            editProfileViewModel.ShowMessageBox += (sender, args) => { confirmationShown = true; args.Data.Result = MessageBoxResult.Yes; }; // delete confirmation
            var profiles = editProfileViewModel.Profiles;

            Assert.That(profiles.Count, Is.EqualTo(3), "profiles.Count");
            Assert.That(editProfileViewModel.WindowTitle, Is.EqualTo("Edit Profile - " + _profileName), "editProfileViewModel.WindowTitle");

            // Act - Delete 2nd of 3 profiles
            var deleteProfile = profiles[1];
            editProfileViewModel.Profile = deleteProfile;
            editProfileViewModel.DeleteProfileCommand.Execute(null);

            // Assert
            Assert.That(confirmationShown, Is.True, "confirmationShown");
            Assert.That(editProfileViewModel.Profiles, Is.EqualTo(profiles), "editProfileViewModel.Profiles");
            Assert.That(editProfileViewModel.Profiles.Count, Is.EqualTo(2), "editProfileViewModel.Profiles.Count");
            Assert.That(editProfileViewModel.Profile, Is.Not.Null, "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Profile, Is.EqualTo(profiles[1]), "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Profiles.Contains(deleteProfile), Is.False, "editProfileViewModel.Profiles.Contains(deleteProfile)");
            Assert.That(editProfileViewModel.WindowTitle, Is.EqualTo("Edit Profile - " + _profileName3), "editProfileViewModel.WindowTitle");
            Assert.That(File.Exists(_defaultProfile), Is.False, "File.Exists(_defaultProfile)");
            Assert.That(File.Exists(_unitTestsProfile), Is.True, "File.Exists(_unitTestsProfile)");
            Assert.That(File.Exists(_unitTestsProfile2), Is.False, "File.Exists(_unitTestsProfile2)");
            Assert.That(File.Exists(_unitTestsProfile3), Is.True, "File.Exists(_unitTestsProfile3)");

            // Arrange
            confirmationShown = false;

            // Act - Delete 2nd of 2 profiles
            deleteProfile = profiles[1];
            editProfileViewModel.Profile = deleteProfile;
            editProfileViewModel.DeleteProfileCommand.Execute(null);

            // Assert
            Assert.That(confirmationShown, Is.True, "confirmationShown");
            Assert.That(editProfileViewModel.Profiles, Is.EqualTo(profiles), "editProfileViewModel.Profiles");
            Assert.That(editProfileViewModel.Profiles.Count, Is.EqualTo(1), "editProfileViewModel.Profiles.Count");
            Assert.That(editProfileViewModel.Profile, Is.Not.Null, "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Profile, Is.EqualTo(profiles[0]), "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Profiles.Contains(deleteProfile), Is.False, "editProfileViewModel.Profiles.Contains(deleteProfile)");
            Assert.That(editProfileViewModel.WindowTitle, Is.EqualTo("Edit Profile - " + _profileName), "editProfileViewModel.WindowTitle");
            Assert.That(File.Exists(_defaultProfile), Is.False, "File.Exists(_defaultProfile)");
            Assert.That(File.Exists(_unitTestsProfile), Is.True, "File.Exists(_unitTestsProfile)");
            Assert.That(File.Exists(_unitTestsProfile2), Is.False, "File.Exists(_unitTestsProfile2)");
            Assert.That(File.Exists(_unitTestsProfile3), Is.False, "File.Exists(_unitTestsProfile3)");

            // Arrange
            confirmationShown = false;

            // Act - Delete last remaining profile in list
            deleteProfile = profiles[0];
            editProfileViewModel.Profile = deleteProfile;
            editProfileViewModel.DeleteProfileCommand.Execute(null);

            // Assert
            Assert.That(confirmationShown, Is.True, "confirmationShown");
            Assert.That(editProfileViewModel.Profiles, Is.EqualTo(profiles), "editProfileViewModel.Profiles");
            Assert.That(editProfileViewModel.Profiles.Count, Is.EqualTo(1), "editProfileViewModel.Profiles.Count");
            Assert.That(editProfileViewModel.Profile, Is.Not.Null, "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Profiles.Contains(deleteProfile), Is.False, "editProfileViewModel.Profiles.Contains(deleteProfile)");
            Assert.That(editProfileViewModel.Profile.ProfileName, Is.EqualTo("Default"), "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.WindowTitle, Is.EqualTo("Edit Profile - Default"), "editProfileViewModel.WindowTitle");
            Assert.That(File.Exists(_defaultProfile), Is.True, "File.Exists(_defaultProfile)");
            Assert.That(File.Exists(_unitTestsProfile), Is.False, "File.Exists(_unitTestsProfile)");
            Assert.That(File.Exists(_unitTestsProfile2), Is.False, "File.Exists(_unitTestsProfile2)");
            Assert.That(File.Exists(_unitTestsProfile3), Is.False, "File.Exists(_unitTestsProfile3)");
        }
    }
}
