using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Dispatcher;
using CDTag.ViewModels.Profile.EditProfile;
using NUnit.Framework;

namespace CDTag.ViewModel.Tests.Profile.EditProfile
{
    [TestFixture]
    public class EditProfileViewModelTests
    {
        private const string _profileName = "unittests";
        private string _unitTestsProfile;
        private string _unitTestsNFO;

        [TestFixtureSetUp]
        public void Setup()
        {
            IoC.ClearAllRegistrations();

            IoC.RegisterInstance<IDispatcher>(new UnitTestDispatcher());
            IoC.RegisterInstance<IPathService>(new UnitTestPathService());
            IoC.RegisterInstance<IDialogService>(new UnitTestDialogService());

            string profileDirectory = IoC.Resolve<IPathService>().ProfileDirectory;
            _unitTestsProfile = Path.Combine(profileDirectory, _profileName + ".cfg");
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
            if (File.Exists(_unitTestsProfile))
                File.Delete(_unitTestsProfile);
            if (File.Exists(_unitTestsNFO))
                File.Delete(_unitTestsNFO);
        }

        [Test]
        public void ConstructorTest()
        {
            // Arrange/Act
            EditProfileViewModel editProfileViewModel = IoC.Resolve<EditProfileViewModel>();

            // Assert
            Assert.That(editProfileViewModel, Is.Not.Null, "editProfileViewModel");
            Assert.That(editProfileViewModel.CopyProfileCommand, Is.Not.Null, "editProfileViewModel.CopyProfileCommand");
            Assert.That(editProfileViewModel.DeleteProfileCommand, Is.Not.Null, "editProfileViewModel.DeleteProfileCommand");
            Assert.That(editProfileViewModel.NewProfileCommand, Is.Not.Null, "editProfileViewModel.NewProfileCommand");
            Assert.That(editProfileViewModel.RenameProfileCommand, Is.Not.Null, "editProfileViewModel.RenameProfileCommand");
            Assert.That(editProfileViewModel.Profiles, Is.Not.Null, "editProfileViewModel.Profiles");
            Assert.That(editProfileViewModel.Profile, Is.Null, "editProfileViewModel.Profile");
            Assert.That(editProfileViewModel.Header, Is.EqualTo("Select Profile"), "editProfileViewModel.Header");
            Assert.That(editProfileViewModel.WindowTitle, Is.EqualTo("Edit Profile"), "editProfileViewModel.Header");
        }
    }
}
