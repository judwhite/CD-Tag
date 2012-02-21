﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CDTag.Common;
using CDTag.Common.Dispatcher;
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
            IoC.RegisterInstance<IPathService>(new PathService());
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
    }
}
