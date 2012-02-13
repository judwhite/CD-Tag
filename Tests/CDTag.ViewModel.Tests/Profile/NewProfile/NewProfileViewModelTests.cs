using System.IO;
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
            Assert.That(newProfileViewModel.CreateNFO, Is.False, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.False, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.False, "newProfileViewModel.HasExistingNFO");
            Assert.That(newProfileViewModel.DirectoryFormats, Is.Not.Null, "newProfileViewModel.DirectoryFormats");
            Assert.That(newProfileViewModel.DirectoryFormats.Count, Is.EqualTo(5), "newProfileViewModel.DirectoryFormats.Count");
            Assert.That(newProfileViewModel.AudioFileFormats, Is.Not.Null, "newProfileViewModel.AudioFileFormats");
            Assert.That(newProfileViewModel.AudioFileFormats.Count, Is.EqualTo(3), "newProfileViewModel.AudioFileFormats.Count");
            Assert.That(newProfileViewModel.DirectoryFormat, Is.Not.Null, "newProfileViewModel.DirectoryFormat");
            Assert.That(newProfileViewModel.AudioFileFormat, Is.Not.Null, "newProfileViewModel.AudioFileFormat");
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
        public void NextPageTest()
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
            Assert.That(File.Exists(_unitTestsProfile), Is.False, "File.Exists(_unitTestsProfile), file should not exist yet.");
        }

        [Test]
        public void CreateExistingProfileNoOverwriteTest()
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

        [Test]
        public void CreateExistingProfileYesOverwriteTest()
        {
            // Arrange
            const string profileName = "unittests";
            const string profileFileContents = "don't delete me";

            DeleteUnitTestsProfile();
            File.WriteAllText(_unitTestsProfile, profileFileContents);
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsProfile));

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool shown = false;
            newProfileViewModel.ShowMessageBox += delegate(object sender, DataEventArgs<MessageBoxEvent> args)
            {
                shown = true;
                Assert.That(args, Is.Not.Null, "args");
                Assert.That(args.Data, Is.Not.Null, "args.Data");
                Assert.That(args.Data.MessageBoxButton, Is.EqualTo(MessageBoxButton.YesNo), "args.Data.MessageBoxButton");

                args.Data.Result = MessageBoxResult.Yes;
            };
            newProfileViewModel.ProfileName = profileName;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(shown, Is.True, string.Format("MessageBox shown, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageTwoStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(1), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", profileName));
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsProfile));
            string actualFileContents = File.ReadAllText(_unitTestsProfile);
            Assert.That(actualFileContents, Is.EqualTo(profileFileContents), "actualFileContents");
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

        [Test]
        public void CreateNFOTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Act/Assert
            
            // Test initial state
            Assert.That(newProfileViewModel.CreateNFO, Is.False, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.False, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.False, "newProfileViewModel.HasExistingNFO");

            // Test CreateSampleNFO default of true when CreateNFO is set
            newProfileViewModel.CreateNFO = true;
            Assert.That(newProfileViewModel.CreateNFO, Is.True, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.True, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.False, "newProfileViewModel.HasExistingNFO");

            // Test returning to initial state
            newProfileViewModel.CreateNFO = false;
            Assert.That(newProfileViewModel.CreateNFO, Is.False, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.False, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.False, "newProfileViewModel.HasExistingNFO");

            // Test returning to CreateNFO = true
            newProfileViewModel.CreateNFO = true;
            Assert.That(newProfileViewModel.CreateNFO, Is.True, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.True, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.False, "newProfileViewModel.HasExistingNFO");

            // Test the mutually exclusive state of CreateSampleNFO and HasExistingNFO
            newProfileViewModel.HasExistingNFO = true;
            Assert.That(newProfileViewModel.CreateNFO, Is.True, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.False, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.True, "newProfileViewModel.HasExistingNFO");

            // Test the mutually exclusive state of CreateSampleNFO and HasExistingNFO
            newProfileViewModel.CreateSampleNFO = true;
            Assert.That(newProfileViewModel.CreateNFO, Is.True, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.True, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.False, "newProfileViewModel.HasExistingNFO");

            // Test going back to CreateNFO = false
            newProfileViewModel.HasExistingNFO = true;
            newProfileViewModel.CreateNFO = false;
            Assert.That(newProfileViewModel.CreateNFO, Is.False, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.False, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.False, "newProfileViewModel.HasExistingNFO");

            // Test HasExistingNFO value is restored
            newProfileViewModel.CreateNFO = true;
            Assert.That(newProfileViewModel.CreateNFO, Is.True, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.CreateSampleNFO, Is.False, "newProfileViewModel.CreateSampleNFO");
            Assert.That(newProfileViewModel.HasExistingNFO, Is.True, "newProfileViewModel.HasExistingNFO");
        }

        [Test]
        public void PreviousPageNoNFOTest()
        {
            // Arrange
            const string profileName = "unittests";

            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            newProfileViewModel.ProfileName = profileName;

            // Act (go to page 2)
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(newProfileViewModel.ProfileName, Is.EqualTo(profileName), "newProfileViewModel.ProfileName");
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(1), "newProfileViewModel.PageIndex");
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageTwoStateName), "newProfileViewModel.CurrentVisualState");

            // Act (go back to page 1)
            newProfileViewModel.PreviousCommand.Execute(null);

            Assert.That(newProfileViewModel.ProfileName, Is.EqualTo(profileName), "newProfileViewModel.ProfileName");
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), "newProfileViewModel.PageIndex");
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), "newProfileViewModel.CurrentVisualState");
            Assert.That(newProfileViewModel.CreateNFO, Is.False, "newProfileViewModel.CreateNFO");
        }
    }
}
