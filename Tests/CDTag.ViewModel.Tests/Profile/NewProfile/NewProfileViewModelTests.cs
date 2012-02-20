using System.Collections.Generic;
using System.IO;
using System.Windows;
using CDTag.Common;
using CDTag.Common.Dispatcher;
using CDTag.Model.Profile;
using CDTag.Model.Profile.NewProfile;
using CDTag.ViewModel.Profile.NewProfile;
using NUnit.Framework;

namespace CDTag.ViewModel.Tests.Profile.NewProfile
{
    [TestFixture]
    public class NewProfileViewModelTests
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
            Assert.That(newProfileViewModel.DirectoryFormat.IsSelected, Is.True, "newProfileViewModel.DirectoryFormat.IsSelected");
            Assert.That(newProfileViewModel.AudioFileFormat.IsSelected, Is.True, "newProfileViewModel.AudioFileFormat.IsSelected");
            Assert.That(newProfileViewModel.Profile, Is.Not.Null, "newProfileViewModel.Profile");
            Assert.That(newProfileViewModel.Profile.FileNaming, Is.Not.Null, "newProfileViewModel.Profile.FileNaming");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseUnderscores, Is.False, "newProfileViewModel.Profile.FileNaming.UseUnderscores");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");
            Assert.That(newProfileViewModel.NextButtonText, Is.EqualTo("_Next"), newProfileViewModel.NextButtonText);
        }

        [Test]
        public void UseUnderscoresPropertyTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool propertyChangedRaised = false;
            newProfileViewModel.Profile.FileNaming.PropertyChanged += (s, e) => { if (e.PropertyName == "UseUnderscores") propertyChangedRaised = true; };

            // Act
            newProfileViewModel.Profile.FileNaming.UseUnderscores = true;

            // Assert
            Assert.That(newProfileViewModel.Profile.FileNaming.UseUnderscores, Is.True, "newProfileViewModel.Profile.FileNaming.UseUnderscores");
            Assert.That(propertyChangedRaised, Is.True, "propertyChangedRaised");
        }

        [Test]
        public void UseStandardCharactersOnlyTest()
        {
            // Test UseStandardCharactersOnly and its affect on UseLatinCharactersOnly

            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool propertyChangedRaised = false;
            newProfileViewModel.Profile.FileNaming.PropertyChanged += (s, e) => { if (e.PropertyName == "UseStandardCharactersOnly") propertyChangedRaised = true; };

            // Act
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.True, "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.True, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");
            Assert.That(propertyChangedRaised, Is.True, "propertyChangedRaised");

            // Test UseLatinCharacters returning to default state (false)

            // Arrange
            propertyChangedRaised = false;

            // Act
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;

            // Assert
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");
            Assert.That(propertyChangedRaised, Is.True, "propertyChangedRaised");

            // Test UseLatinCharacters = true

            // Act
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.True, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");

            // Test UseLatinCharacters = true with UseStandardCharactersOnly = true

            // Arrange
            propertyChangedRaised = false;

            // Act
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.True, "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.True, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");
            Assert.That(propertyChangedRaised, Is.True, "propertyChangedRaised");

            // Test returning UseLatinCharacters = false, preserving original state of UseStandardCharactersOnly = true

            // Arrange
            propertyChangedRaised = false;

            // Act
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;

            // Assert
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.True, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");
            Assert.That(propertyChangedRaised, Is.True, "propertyChangedRaised");
        }

        [Test]
        public void UseLatinCharactersOnlyTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool propertyChangedRaised = false;
            newProfileViewModel.Profile.FileNaming.PropertyChanged += (s, e) => { if (e.PropertyName == "UseLatinCharactersOnly") propertyChangedRaised = true; };

            // Act
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.True, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");
            Assert.That(propertyChangedRaised, Is.True, "propertyChangedRaised");
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

            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool messageBoxShown = false;
            newProfileViewModel.ShowMessageBox += (s, e) => { messageBoxShown = true; };
            newProfileViewModel.Profile.ProfileName = _profileName;

            // Assert initial state
            Assert.That(newProfileViewModel.NextCommand.CanExecute(null), Is.True, "newProfileViewModel.NextCommand.CanExecute(null)");
            Assert.That(newProfileViewModel.PreviousCommand.CanExecute(null), Is.False, "newProfileViewModel.PreviousCommand.CanExecute(null)");

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(messageBoxShown, Is.False, "messageBoxShown");
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageTwoStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", _profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(1), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", _profileName));
            Assert.That(File.Exists(_unitTestsProfile), Is.False, "File.Exists(_unitTestsProfile), file should not exist yet.");
            Assert.That(newProfileViewModel.PreviousCommand.CanExecute(null), Is.True, "newProfileViewModel.PreviousCommand.CanExecute(null)");
            Assert.That(newProfileViewModel.NextButtonText, Is.EqualTo("_Finish"), newProfileViewModel.NextButtonText);

            // Arrange - Previous page
            newProfileViewModel.PreviousCommand.Execute(null);
            newProfileViewModel.Profile.ProfileName = null;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(messageBoxShown, Is.True, "messageBoxShown");
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
            newProfileViewModel.ShowMessageBox += (s, args) =>
            {
                shown = true;
                Assert.That(args, Is.Not.Null, "args");
                Assert.That(args.Data, Is.Not.Null, "args.Data");
                Assert.That(args.Data.MessageBoxButton, Is.EqualTo(MessageBoxButton.YesNo), "args.Data.MessageBoxButton");

                args.Data.Result = MessageBoxResult.No;
            };
            newProfileViewModel.Profile.ProfileName = profileName;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(shown, Is.True, string.Format("MessageBox shown, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", profileName));
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsProfile));
            Assert.That(newProfileViewModel.NextButtonText, Is.EqualTo("_Next"), newProfileViewModel.NextButtonText);
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
            newProfileViewModel.ShowMessageBox += (s, args) =>
            {
                shown = true;
                Assert.That(args, Is.Not.Null, "args");
                Assert.That(args.Data, Is.Not.Null, "args.Data");
                Assert.That(args.Data.MessageBoxButton, Is.EqualTo(MessageBoxButton.YesNo), "args.Data.MessageBoxButton");

                args.Data.Result = MessageBoxResult.Yes;
            };
            newProfileViewModel.Profile.ProfileName = profileName;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(shown, Is.True, string.Format("MessageBox shown, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageTwoStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(1), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", profileName));
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsProfile));
            string actualFileContents = File.ReadAllText(_unitTestsProfile);
            Assert.That(actualFileContents, Is.EqualTo(profileFileContents), "actualFileContents");
            Assert.That(newProfileViewModel.NextButtonText, Is.EqualTo("_Finish"), newProfileViewModel.NextButtonText);
        }

        private static void NextButtonValidationTest(string profileName)
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool shown = false;
            newProfileViewModel.ShowMessageBox += (s, e) => { shown = true; };
            newProfileViewModel.Profile.ProfileName = profileName;

            // Act
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(shown, Is.True, string.Format("MessageBox shown, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.IsProfileNameFocused, Is.True, string.Format("newProfileViewModel.IsProfileNameFocused, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), string.Format("newProfileViewModel.CurrentVisualState, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), string.Format("newProfileViewModel.PageIndex, profileName='{0}'", profileName));
            Assert.That(newProfileViewModel.NextButtonText, Is.EqualTo("_Next"), newProfileViewModel.NextButtonText);
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
            newProfileViewModel.Profile.ProfileName = profileName;

            // Act (go to page 2)
            newProfileViewModel.NextCommand.Execute(null);

            // Assert
            Assert.That(newProfileViewModel.Profile.ProfileName, Is.EqualTo(profileName), "newProfileViewModel.Profile.ProfileName");
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(1), "newProfileViewModel.PageIndex");
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageTwoStateName), "newProfileViewModel.CurrentVisualState");
            Assert.That(newProfileViewModel.NextButtonText, Is.EqualTo("_Finish"), newProfileViewModel.NextButtonText);

            // Act (go back to page 1)
            newProfileViewModel.PreviousCommand.Execute(null);

            // Assert
            Assert.That(newProfileViewModel.Profile.ProfileName, Is.EqualTo(profileName), "newProfileViewModel.Profile.ProfileName");
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), "newProfileViewModel.PageIndex");
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), "newProfileViewModel.CurrentVisualState");
            Assert.That(newProfileViewModel.CreateNFO, Is.False, "newProfileViewModel.CreateNFO");
            Assert.That(newProfileViewModel.PreviousCommand.CanExecute(null), Is.False, "newProfileViewModel.PreviousCommand.CanExecute(null)");
            Assert.That(newProfileViewModel.NextButtonText, Is.EqualTo("_Next"), newProfileViewModel.NextButtonText);
        }

        [Test]
        public void DirectoryFormatIsSelectedTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            var directoryFormat = newProfileViewModel.DirectoryFormats[1];

            // Act
            newProfileViewModel.DirectoryFormat = directoryFormat;

            // Assert

            Assert.That(newProfileViewModel.DirectoryFormat, Is.EqualTo(directoryFormat), "newProfileViewModel.DirectoryFormat");

            for (int i = 0; i < newProfileViewModel.DirectoryFormats.Count; i++)
            {
                var item = newProfileViewModel.DirectoryFormats[i];

                string message = string.Format("newProfileViewModel.DirectoryFormats[{0}]", i);

                if (item == directoryFormat)
                    Assert.That(item.IsSelected, Is.True, message);
                else
                    Assert.That(item.IsSelected, Is.False, message);
            }
        }

        [Test]
        public void AudioFileFormatIsSelectedTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            var audioFileFormat = newProfileViewModel.AudioFileFormats[1];

            // Act
            newProfileViewModel.AudioFileFormat = audioFileFormat;

            // Assert

            Assert.That(newProfileViewModel.AudioFileFormat, Is.EqualTo(audioFileFormat), "newProfileViewModel.AudioFileFormat");

            for (int i = 0; i < newProfileViewModel.AudioFileFormats.Count; i++)
            {
                var item = newProfileViewModel.AudioFileFormats[i];

                string message = string.Format("newProfileViewModel.AudioFileFormats[{0}]", i);

                if (item == audioFileFormat)
                    Assert.That(item.IsSelected, Is.True, message);
                else
                    Assert.That(item.IsSelected, Is.False, message);
            }
        }

        [Test]
        public void FileNameResultsTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormats.Count, Is.EqualTo(3), "newProfileViewModel.AudioFileFormats.Count");

            // Act - Spaces, Allow all characters
            newProfileViewModel.Profile.FileNaming.UseUnderscores = false;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = false;

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormats[0].Result, Is.EqualTo("03 - Björk - Where Is The Line?.mp3"), "newProfileViewModel.AudioFileFormats[0].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[1].Result, Is.EqualTo("Björk - 03 - Where Is The Line?.mp3"), "newProfileViewModel.AudioFileFormats[1].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[2].Result, Is.EqualTo("03 - Where Is The Line?.mp3"), "newProfileViewModel.AudioFileFormats[2].Result");

            // Act - Spaces, Standard characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = false;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormats[0].Result, Is.EqualTo("03 - Bjork - Where Is The Line.mp3"), "newProfileViewModel.AudioFileFormats[0].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[1].Result, Is.EqualTo("Bjork - 03 - Where Is The Line.mp3"), "newProfileViewModel.AudioFileFormats[1].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[2].Result, Is.EqualTo("03 - Where Is The Line.mp3"), "newProfileViewModel.AudioFileFormats[2].Result");

            // Act - Spaces, English characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = false;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormats[0].Result, Is.EqualTo("03 - Bjork - Where Is The Line?.mp3"), "newProfileViewModel.AudioFileFormats[0].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[1].Result, Is.EqualTo("Bjork - 03 - Where Is The Line?.mp3"), "newProfileViewModel.AudioFileFormats[1].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[2].Result, Is.EqualTo("03 - Where Is The Line?.mp3"), "newProfileViewModel.AudioFileFormats[2].Result");

            // Act - Underscores, Allow all characters
            newProfileViewModel.Profile.FileNaming.UseUnderscores = true;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = false;

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormats[0].Result, Is.EqualTo("03-Björk-Where_Is_The_Line?.mp3"), "newProfileViewModel.AudioFileFormats[0].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[1].Result, Is.EqualTo("Björk-03-Where_Is_The_Line?.mp3"), "newProfileViewModel.AudioFileFormats[1].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[2].Result, Is.EqualTo("03-Where_Is_The_Line?.mp3"), "newProfileViewModel.AudioFileFormats[2].Result");

            // Act - Underscores, Standard characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = true;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormats[0].Result, Is.EqualTo("03-Bjork-Where_Is_The_Line.mp3"), "newProfileViewModel.AudioFileFormats[0].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[1].Result, Is.EqualTo("Bjork-03-Where_Is_The_Line.mp3"), "newProfileViewModel.AudioFileFormats[1].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[2].Result, Is.EqualTo("03-Where_Is_The_Line.mp3"), "newProfileViewModel.AudioFileFormats[2].Result");

            // Act - Underscores, English characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = true;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormats[0].Result, Is.EqualTo("03-Bjork-Where_Is_The_Line?.mp3"), "newProfileViewModel.AudioFileFormats[0].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[1].Result, Is.EqualTo("Bjork-03-Where_Is_The_Line?.mp3"), "newProfileViewModel.AudioFileFormats[1].Result");
            Assert.That(newProfileViewModel.AudioFileFormats[2].Result, Is.EqualTo("03-Where_Is_The_Line?.mp3"), "newProfileViewModel.AudioFileFormats[2].Result");
        }

        [Test]
        public void DirectoryNameResultsTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Assert initial state
            Assert.That(newProfileViewModel.DirectoryFormats.Count, Is.EqualTo(5), "newProfileViewModel.DirectoryFormats.Count");

            // Act/Assert
            AssertDirectoryNameResultsSpaces(newProfileViewModel);
            AssertDirectoryNameResultsUnderscores(newProfileViewModel);

            // Act/Assert - test flipping back to Spaces from Underscores
            AssertDirectoryNameResultsSpaces(newProfileViewModel);
        }

        private static void AssertDirectoryNameResultsSpaces(NewProfileViewModel newProfileViewModel)
        {
            // Act - Spaces, Allow all characters
            newProfileViewModel.Profile.FileNaming.UseUnderscores = false;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = false;

            // Assert
            Assert.That(newProfileViewModel.DirectoryFormats[0].Result, Is.EqualTo("Björk - Medúlla (2004)"), "newProfileViewModel.DirectoryFormats[0].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[1].Result, Is.EqualTo("Björk - Medúlla - 2004"), "newProfileViewModel.DirectoryFormats[1].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[2].Result, Is.EqualTo("Björk - (2004) - Medúlla"), "newProfileViewModel.DirectoryFormats[2].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[3].Result, Is.EqualTo("Björk - 2004 - Medúlla"), "newProfileViewModel.DirectoryFormats[3].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[4].Result, Is.EqualTo("Björk - Medúlla"), "newProfileViewModel.DirectoryFormats[4].Result");

            // Act - Spaces, Standard characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = false;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.DirectoryFormats[0].Result, Is.EqualTo("Bjork - Medulla (2004)"), "newProfileViewModel.DirectoryFormats[0].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[1].Result, Is.EqualTo("Bjork - Medulla - 2004"), "newProfileViewModel.DirectoryFormats[1].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[2].Result, Is.EqualTo("Bjork - (2004) - Medulla"), "newProfileViewModel.DirectoryFormats[2].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[3].Result, Is.EqualTo("Bjork - 2004 - Medulla"), "newProfileViewModel.DirectoryFormats[3].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[4].Result, Is.EqualTo("Bjork - Medulla"), "newProfileViewModel.DirectoryFormats[4].Result");

            // Act - Spaces, English characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = false;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.DirectoryFormats[0].Result, Is.EqualTo("Bjork - Medulla (2004)"), "newProfileViewModel.DirectoryFormats[0].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[1].Result, Is.EqualTo("Bjork - Medulla - 2004"), "newProfileViewModel.DirectoryFormats[1].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[2].Result, Is.EqualTo("Bjork - (2004) - Medulla"), "newProfileViewModel.DirectoryFormats[2].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[3].Result, Is.EqualTo("Bjork - 2004 - Medulla"), "newProfileViewModel.DirectoryFormats[3].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[4].Result, Is.EqualTo("Bjork - Medulla"), "newProfileViewModel.DirectoryFormats[4].Result");
        }

        private static void AssertDirectoryNameResultsUnderscores(NewProfileViewModel newProfileViewModel)
        {
            // Act - Underscores, Allow all characters
            newProfileViewModel.Profile.FileNaming.UseUnderscores = true;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = false;

            // Assert
            Assert.That(newProfileViewModel.DirectoryFormats[0].Result, Is.EqualTo("Björk-Medúlla_(2004)"), "newProfileViewModel.DirectoryFormats[0].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[1].Result, Is.EqualTo("Björk-Medúlla-2004"), "newProfileViewModel.DirectoryFormats[1].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[2].Result, Is.EqualTo("Björk-(2004)-Medúlla"), "newProfileViewModel.DirectoryFormats[2].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[3].Result, Is.EqualTo("Björk-2004-Medúlla"), "newProfileViewModel.DirectoryFormats[3].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[4].Result, Is.EqualTo("Björk-Medúlla"), "newProfileViewModel.DirectoryFormats[4].Result");

            // Act - Underscores, Standard characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = true;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.DirectoryFormats[0].Result, Is.EqualTo("Bjork-Medulla_(2004)"), "newProfileViewModel.DirectoryFormats[0].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[1].Result, Is.EqualTo("Bjork-Medulla-2004"), "newProfileViewModel.DirectoryFormats[1].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[2].Result, Is.EqualTo("Bjork-(2004)-Medulla"), "newProfileViewModel.DirectoryFormats[2].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[3].Result, Is.EqualTo("Bjork-2004-Medulla"), "newProfileViewModel.DirectoryFormats[3].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[4].Result, Is.EqualTo("Bjork-Medulla"), "newProfileViewModel.DirectoryFormats[4].Result");

            // Act - Underscores, English characters only
            newProfileViewModel.Profile.FileNaming.UseUnderscores = true;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = false;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = true;

            // Assert
            Assert.That(newProfileViewModel.DirectoryFormats[0].Result, Is.EqualTo("Bjork-Medulla_(2004)"), "newProfileViewModel.DirectoryFormats[0].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[1].Result, Is.EqualTo("Bjork-Medulla-2004"), "newProfileViewModel.DirectoryFormats[1].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[2].Result, Is.EqualTo("Bjork-(2004)-Medulla"), "newProfileViewModel.DirectoryFormats[2].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[3].Result, Is.EqualTo("Bjork-2004-Medulla"), "newProfileViewModel.DirectoryFormats[3].Result");
            Assert.That(newProfileViewModel.DirectoryFormats[4].Result, Is.EqualTo("Bjork-Medulla"), "newProfileViewModel.DirectoryFormats[4].Result");
        }

        [Test]
        public void NextButtonEnabledTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Assert initial state
            Assert.That(newProfileViewModel.NextCommand.CanExecute(null), Is.False, "newProfileViewModel.NextCommand.CanExecute(null)");

            // Act
            newProfileViewModel.Profile.ProfileName = "test";

            // Assert
            Assert.That(newProfileViewModel.NextCommand.CanExecute(null), Is.True, "newProfileViewModel.NextCommand.CanExecute(null)");

            // Act
            newProfileViewModel.Profile.ProfileName = null;

            // Assert
            Assert.That(newProfileViewModel.NextCommand.CanExecute(null), Is.False, "newProfileViewModel.NextCommand.CanExecute(null)");

            // Act
            newProfileViewModel.Profile.ProfileName = string.Empty;

            // Assert
            Assert.That(newProfileViewModel.NextCommand.CanExecute(null), Is.False, "newProfileViewModel.NextCommand.CanExecute(null)");

            // Act
            newProfileViewModel.Profile.ProfileName = "  ";

            // Assert
            Assert.That(newProfileViewModel.NextCommand.CanExecute(null), Is.False, "newProfileViewModel.NextCommand.CanExecute(null)");

            // Act
            newProfileViewModel.Profile.ProfileName = "test2";

            // Assert
            Assert.That(newProfileViewModel.NextCommand.CanExecute(null), Is.True, "newProfileViewModel.NextCommand.CanExecute(null)");
        }

        [Test]
        public void DirectoryFormatDontAllowNullTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Act
            newProfileViewModel.DirectoryFormat = null;

            // Assert
            Assert.That(newProfileViewModel.DirectoryFormat, Is.Not.Null, "newProfileViewModel.DirectoryFormat");
        }

        [Test]
        public void AudioFormatDontAllowNullTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            // Act
            newProfileViewModel.AudioFileFormat = null;

            // Assert
            Assert.That(newProfileViewModel.AudioFileFormat, Is.Not.Null, "newProfileViewModel.AudioFileFormat");
        }

        [Test]
        public void FinishFileExistsTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            newProfileViewModel.Profile.ProfileName = _profileName;

            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };

            // Assert initial state
            Assert.That(File.Exists(_unitTestsProfile), Is.False, string.Format("File.Exists({0})", _unitTestsProfile));

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists({0})", _unitTestsProfile));
            Assert.That(closeWindowCalled, Is.True, "closeWindowCalled");
        }

        [Test]
        public void FinishTest()
        {
            // Arrange
            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            var directoryFormats = newProfileViewModel.DirectoryFormats;
            var audioFileFormats = newProfileViewModel.AudioFileFormats;

            // Assert
            foreach (var createNFO in new List<bool> { false, true })
            {
                foreach (var directoryFormat in directoryFormats)
                {
                    foreach (var audioFileFormat in audioFileFormats)
                    {
                        FinishTestParameters(directoryFormat, audioFileFormat, createNFO: createNFO, useUnderscores: false, useStandardCharactersOnly: false, useLatinCharactersOnly: false);
                        FinishTestParameters(directoryFormat, audioFileFormat, createNFO: createNFO, useUnderscores: false, useStandardCharactersOnly: true, useLatinCharactersOnly: true);
                        FinishTestParameters(directoryFormat, audioFileFormat, createNFO: createNFO, useUnderscores: false, useStandardCharactersOnly: false, useLatinCharactersOnly: true);

                        FinishTestParameters(directoryFormat, audioFileFormat, createNFO: createNFO, useUnderscores: true, useStandardCharactersOnly: false, useLatinCharactersOnly: false);
                        FinishTestParameters(directoryFormat, audioFileFormat, createNFO: createNFO, useUnderscores: true, useStandardCharactersOnly: true, useLatinCharactersOnly: true);
                        FinishTestParameters(directoryFormat, audioFileFormat, createNFO: createNFO, useUnderscores: true, useStandardCharactersOnly: false, useLatinCharactersOnly: true);
                    }
                }
            }
        }

        private void FinishTestParameters(FormatItem directoryFormat, FormatItem audioFormat, bool createNFO, bool useUnderscores, bool useStandardCharactersOnly, bool useLatinCharactersOnly)
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            newProfileViewModel.Profile.ProfileName = _profileName;
            newProfileViewModel.Profile.FileNaming.UseUnderscores = useUnderscores;
            newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly = useStandardCharactersOnly;
            newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly = useLatinCharactersOnly;

            newProfileViewModel.DirectoryFormat = directoryFormat;
            newProfileViewModel.AudioFileFormat = audioFormat;
            newProfileViewModel.CreateNFO = createNFO;

            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };

            // Assert initial state
            Assert.That(File.Exists(_unitTestsProfile), Is.False, string.Format("File.Exists({0})", _unitTestsProfile));

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(File.Exists(_unitTestsProfile), Is.True, string.Format("File.Exists({0})", _unitTestsProfile));
            Assert.That(closeWindowCalled, Is.True, "closeWindowCalled");

            if (createNFO)
            {
                Assert.That(newProfileViewModel.Profile.Finish.NFO, Is.EqualTo(FinishNFO.CreateNew), "newProfileViewModel.Profile.Finish.NFO");
                Assert.That(newProfileViewModel.Profile.NFOOptions.ShowReleaseScreen, Is.True, "newProfileViewModel.Profile.NFOOptions.ShowReleaseScreen");
                Assert.That(newProfileViewModel.Profile.NFOOptions.TemplatePath, Is.StringEnding(".nfo"), "newProfileViewModel.Profile.NFOOptions.TemplatePath");
                // TODO: Verify template path exists
            }
            else
            {
                Assert.That(newProfileViewModel.Profile.Finish.NFO, Is.EqualTo(FinishNFO.RenameExisting), "newProfileViewModel.Profile.Finish.NFO");
                Assert.That(newProfileViewModel.Profile.NFOOptions.ShowReleaseScreen, Is.False, "newProfileViewModel.Profile.NFOOptions.ShowReleaseScreen");
                Assert.That(newProfileViewModel.Profile.NFOOptions.TemplatePath, Is.Null, "newProfileViewModel.Profile.NFOOptions.TemplatePath");
            }

            Assert.That(newProfileViewModel.Profile.FileNaming.UseUnderscores, Is.EqualTo(useUnderscores), "newProfileViewModel.Profile.FileNaming.UseUnderscores");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.EqualTo(useStandardCharactersOnly), "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.EqualTo(useLatinCharactersOnly), "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");

            AssertProfileValues(newProfileViewModel);
        }

        private void AssertProfileValues(NewProfileViewModel newProfileViewModel)
        {
            var profile = UserProfile.Load(_unitTestsProfile);

            // Arrange
            var formatGroups = new List<NamingFormatGroup> {
                profile.FileNaming.SingleCD,
                profile.FileNaming.MultiCD,
                profile.FileNaming.Vinyl
            };

            string space = profile.FileNaming.UseUnderscores ? "" : " ";
            string directoryFormat = newProfileViewModel.DirectoryFormat.FormatString;
            string singleArtistAudioFileFormat = newProfileViewModel.AudioFileFormat.FormatString;
            string variousArtistsAudioFileFormat = singleArtistAudioFileFormat;
            string fileFormat = string.Format("<00>{0}-{0}{1}", space, directoryFormat);
            string imageFileformat = string.Format("<00>{0}-{0}{1}{0}-{0}<ImageText>", space, directoryFormat);

            if (variousArtistsAudioFileFormat.StartsWith("<Artist>"))
            {
                // For VA <Artist> - <Track> swap Track and Artist position for proper sorting
                variousArtistsAudioFileFormat = variousArtistsAudioFileFormat.Replace("<Artist>", "<T>");
                variousArtistsAudioFileFormat = variousArtistsAudioFileFormat.Replace("<Track>", "<Artist>");
                variousArtistsAudioFileFormat = variousArtistsAudioFileFormat.Replace("<T>", "<Track>");
            }

            // Assert
            Assert.That(profile.FileNaming.UseUnderscores, Is.EqualTo(newProfileViewModel.Profile.FileNaming.UseUnderscores), "profile.FileNaming.UseUnderscores");
            Assert.That(profile.FileNaming.UseStandardCharactersOnly, Is.EqualTo(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly), "profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(profile.FileNaming.UseLatinCharactersOnly, Is.EqualTo(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly), "profile.FileNaming.UseLatinCharactersOnly");

            foreach (var formatGroup in formatGroups)
            {
                Assert.That(formatGroup.SingleArtist.AudioFile, Is.EqualTo(singleArtistAudioFileFormat), "formatGroup.SingleArtist.AudioFile");
                Assert.That(formatGroup.VariousArtists.AudioFile, Is.EqualTo(variousArtistsAudioFileFormat), "formatGroup.VariousArtists.AudioFile");

                var formats = new List<NamingFormat> {
                    formatGroup.SingleArtist,
                    formatGroup.VariousArtists
                };

                foreach (var format in formats)
                {
                    Assert.That(format.Directory, Is.EqualTo(directoryFormat), "format.Directory");
                    Assert.That(format.CUE, Is.EqualTo(fileFormat), "format.CUE");
                    Assert.That(format.Playlist, Is.EqualTo(fileFormat), "format.Playlist");
                    Assert.That(format.Checksum, Is.EqualTo(fileFormat), "format.Checksum");
                    Assert.That(format.NFO, Is.EqualTo(fileFormat), "format.NFO");
                    Assert.That(format.Images, Is.EqualTo(imageFileformat), "format.Images");
                    Assert.That(format.EACLog, Is.EqualTo(fileFormat), "format.EACLog");
                }
            }
        }

        [Test]
        public void SampleNFOExistsNoOverwriteTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            newProfileViewModel.Profile.ProfileName = _profileName;
            newProfileViewModel.CreateNFO = true;
            newProfileViewModel.CreateSampleNFO = true;
            bool messageBoxShown = false;
            newProfileViewModel.ShowMessageBox += (s, e) => { e.Data.Result = MessageBoxResult.No; messageBoxShown = true; }; // do not overwrite
            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };
            const string contents = "unittestfilecontents";
            File.WriteAllText(_unitTestsNFO, contents); // create file

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(messageBoxShown, Is.True, "messageBoxShown");
            Assert.That(closeWindowCalled, Is.False, "closeWindowCalled");
            Assert.That(File.Exists(_unitTestsNFO), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsNFO));
            string actualContents = File.ReadAllText(_unitTestsNFO);
            Assert.That(actualContents, Is.EqualTo(contents), "actualContents");
        }

        [Test]
        public void SampleNFOExistsOverwriteTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            newProfileViewModel.Profile.ProfileName = _profileName;
            newProfileViewModel.CreateNFO = true;
            newProfileViewModel.CreateSampleNFO = true;
            bool messageBoxShown = false;
            newProfileViewModel.ShowMessageBox += (s, e) => { e.Data.Result = MessageBoxResult.Yes; messageBoxShown = true; }; // overwrite
            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };
            const string contents = "unittestfilecontents";
            File.WriteAllText(_unitTestsNFO, contents); // create file

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(messageBoxShown, Is.True, "messageBoxShown");
            Assert.That(closeWindowCalled, Is.True, "closeWindowCalled");
            Assert.That(File.Exists(_unitTestsNFO), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsNFO));
            string actualContents = File.ReadAllText(_unitTestsNFO);
            Assert.That(actualContents, Is.EqualTo(UserProfile.GetSampleNFO()), "actualContents");
            Assert.That(newProfileViewModel.Profile.NFOOptions.TemplatePath, Is.EqualTo("unittests.nfo"), "newProfileViewModel.Profile.NFOOptions.TemplatePath");
        }

        [Test]
        public void SampleNFODoesNotExistTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            newProfileViewModel.Profile.ProfileName = _profileName;
            newProfileViewModel.CreateNFO = true;
            newProfileViewModel.CreateSampleNFO = true;
            bool messageBoxShown = false;
            newProfileViewModel.ShowMessageBox += (s, e) => { e.Data.Result = MessageBoxResult.No; messageBoxShown = true; }; // do not overwrite
            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(messageBoxShown, Is.False, "messageBoxShown");
            Assert.That(closeWindowCalled, Is.True, "closeWindowCalled");
            Assert.That(File.Exists(_unitTestsNFO), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsNFO));
            string actualContents = File.ReadAllText(_unitTestsNFO);
            Assert.That(actualContents, Is.EqualTo(UserProfile.GetSampleNFO()), "actualContents");
            Assert.That(newProfileViewModel.Profile.NFOOptions.TemplatePath, Is.EqualTo("unittests.nfo"), "newProfileViewModel.Profile.NFOOptions.TemplatePath");
        }

        [Test]
        public void UseExistingNFOOpenDialogTrueTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            newProfileViewModel.Profile.ProfileName = _profileName;
            newProfileViewModel.CreateNFO = true;
            newProfileViewModel.HasExistingNFO = true;
            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };
            UnitTestDialogService dialogService = (UnitTestDialogService)IoC.Resolve<IDialogService>();
            dialogService.ShowOpenFileDialogFileName = _unitTestsNFO;
            dialogService.ShowOpenFileDialogResult = true; // "Open"
            const string contents = "unittestfilecontents";
            File.WriteAllText(_unitTestsNFO, contents); // create file

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(closeWindowCalled, Is.True, "closeWindowCalled");
            Assert.That(File.Exists(_unitTestsNFO), Is.True, string.Format("File.Exists(\"{0}\")", _unitTestsNFO));
            string actualContents = File.ReadAllText(_unitTestsNFO);
            Assert.That(actualContents, Is.EqualTo(contents), "actualContents");
            Assert.That(newProfileViewModel.Profile.NFOOptions.TemplatePath, Is.EqualTo(_unitTestsNFO), "newProfileViewModel.Profile.NFOOptions.TemplatePath");
        }

        [Test]
        public void UseExistingNFOOpenDialogFalseTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            newProfileViewModel.Profile.ProfileName = _profileName;
            newProfileViewModel.CreateNFO = true;
            newProfileViewModel.HasExistingNFO = true;
            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };
            UnitTestDialogService dialogService = (UnitTestDialogService)IoC.Resolve<IDialogService>();
            dialogService.ShowOpenFileDialogFileName = _unitTestsNFO;
            dialogService.ShowOpenFileDialogResult = false; // "Cancel"

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(closeWindowCalled, Is.False, "closeWindowCalled");
            Assert.That(File.Exists(_unitTestsNFO), Is.False, string.Format("File.Exists(\"{0}\")", _unitTestsNFO));
        }

        [Test]
        public void UseExistingNFOOpenDialogNullTest()
        {
            // Arrange
            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();

            newProfileViewModel.Profile.ProfileName = _profileName;
            newProfileViewModel.CreateNFO = true;
            newProfileViewModel.HasExistingNFO = true;
            bool closeWindowCalled = false;
            newProfileViewModel.CloseWindow = () => { closeWindowCalled = true; };
            UnitTestDialogService dialogService = (UnitTestDialogService)IoC.Resolve<IDialogService>();
            dialogService.ShowOpenFileDialogFileName = _unitTestsNFO;
            dialogService.ShowOpenFileDialogResult = null; // "Close"

            // Act
            newProfileViewModel.NextCommand.Execute(null); // Page 1 -> Page 2
            newProfileViewModel.NextCommand.Execute(null); // Page 2 -> Finish

            // Assert
            Assert.That(closeWindowCalled, Is.False, "closeWindowCalled");
            Assert.That(File.Exists(_unitTestsNFO), Is.False, string.Format("File.Exists(\"{0}\")", _unitTestsNFO));
        }
    }
}
