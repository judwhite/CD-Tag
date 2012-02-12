using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CDTag.Common;
using CDTag.Common.Dispatcher;
using CDTag.ViewModel.Events;
using CDTag.ViewModel.Profile.NewProfile;
using IdSharp.Common.Events;
using NUnit.Framework;

namespace CDTag.ViewModel.Tests.Profile.NewProfile
{
    [TestFixture]
    public class NewProfileViewModelTests
    {
        private string _unitTestsProfile;

        [TestFixtureSetUp]
        public void Setup()
        {
            IoC.ClearAllRegistrations();

            IoC.RegisterInstance<IDispatcher>(new UnitTestDispatcher());
            IoC.RegisterInstance<IPathService>(new PathService());

            _unitTestsProfile = Path.Combine(IoC.Resolve<IPathService>().ProfileDirectory, "unittests.cfg");

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
        }

        [Test]
        public void ConstructorTest()
        {
            // Arrange/Act
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Assert
            Assert.That(newProfileViewModel, Is.Not.Null, "newProfileViewModel");
            Assert.That(newProfileViewModel.EventAggregator, Is.Not.Null, "newProfileViewModel.EventAggregator");
            Assert.That(newProfileViewModel.IsProfileNameFocused, Is.True, "newProfileViewModel.IsProfileNameFocused");
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), "newProfileViewModel.CurrentVisualState");
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), "newProfileViewModel.PageIndex");
        }

        [Test]
        public void ProfileNameValidationTest()
        {
            NextButtonValidationTest(null);
            NextButtonValidationTest(string.Empty);
            NextButtonValidationTest(" ");
            NextButtonValidationTest("          ");
            NextButtonValidationTest("*");
            NextButtonValidationTest("?");
            NextButtonValidationTest("/");
            NextButtonValidationTest("\\");
            NextButtonValidationTest("a\\");
            NextButtonValidationTest("\\a");
            NextButtonValidationTest("a\\a");
            NextButtonValidationTest(":");
            NextButtonValidationTest(".");
            NextButtonValidationTest("..");
            NextButtonValidationTest(" .");
            NextButtonValidationTest(" ..");
            NextButtonValidationTest(". ");
            NextButtonValidationTest(".. ");
            NextButtonValidationTest(new string('z', 1024));

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                NextButtonValidationTest(c.ToString());
            }

            foreach (char c in Path.GetInvalidPathChars())
            {
                NextButtonValidationTest(c.ToString());
            }
        }

        [Test]
        public void CreateProfileTest()
        {
            // Arrange
            const string profileName = "unittests";

            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool shown = false;
            newProfileViewModel.ShowMessageBox += delegate { shown = true; };
            newProfileViewModel.ProfileName = profileName;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(shown, Is.False, string.Format("MessageBox shown, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageTwoStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(1), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", profileName));
        }

        [Test]
        public void CreateExistingProfileTest()
        {
            // Arrange
            const string profileName = "unittests";

            DeleteUnitTestsProfile();
            File.WriteAllText(_unitTestsProfile, string.Empty);
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsProfile));

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool shown = false;
            newProfileViewModel.ShowMessageBox += delegate(object sender, DataEventArgs<MessageBoxEvent> args)
            {
                shown = true;
                Assert.That(args, Is.Not.Null, "args");
                Assert.That(args.Data, Is.Not.Null, "args.Data");
                Assert.That(args.Data.MessageBoxButton, Is.EqualTo(MessageBoxButton.YesNo), "args.Data.MessageBoxButton");

                args.Data.Result = MessageBoxResult.No;
            };
            newProfileViewModel.ProfileName = profileName;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(shown, Is.True, string.Format("MessageBox shown, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", profileName));
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsProfile));
        }

        private static void NextButtonValidationTest(string profileName)
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool shown = false;
            newProfileViewModel.ShowMessageBox += delegate { shown = true; };
            newProfileViewModel.ProfileName = profileName;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(shown, Is.True, string.Format("MessageBox shown, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.IsProfileNameFocused, Is.True, string.Format("newProfileViewModel.IsProfileNameFocused, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", profileName));
        }
    }
}
