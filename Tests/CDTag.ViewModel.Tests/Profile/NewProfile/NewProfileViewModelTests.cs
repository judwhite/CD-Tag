using System.IO;
using System.Windows;
using CDTag.Common;
using CDTag.Common.Dispatcher;
using CDTag.ViewModel.Profile.NewProfile;
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
            Assert.That(newProfileViewModel.DirectoryFormat.IsSelected, Is.True, "newProfileViewModel.DirectoryFormat.IsSelected");
            Assert.That(newProfileViewModel.AudioFileFormat.IsSelected, Is.True, "newProfileViewModel.AudioFileFormat.IsSelected");
            Assert.That(newProfileViewModel.Profile, Is.Not.Null, "newProfileViewModel.Profile");
            Assert.That(newProfileViewModel.Profile.FileNaming, Is.Not.Null, "newProfileViewModel.Profile.FileNaming");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseUnderscores, Is.False, "newProfileViewModel.Profile.FileNaming.UseUnderscores");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseStandardCharactersOnly");
            Assert.That(newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly, Is.False, "newProfileViewModel.Profile.FileNaming.UseLatinCharactersOnly");
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
            const string profileName = "unittests";

            DeleteUnitTestsProfile();

            NewProfileViewModel newProfileViewModel = IoC.Resolve<NewProfileViewModel>();
            bool shown = false;
            newProfileViewModel.ShowMessageBox += (s, e) => { shown = true; };
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
            newProfileViewModel.ShowMessageBox += (s, args) =>
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
            newProfileViewModel.ShowMessageBox += (s, args) =>
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
            newProfileViewModel.ShowMessageBox += (s, e) => { shown = true; };
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

            // Assert
            Assert.That(newProfileViewModel.ProfileName, Is.EqualTo(profileName), "newProfileViewModel.ProfileName");
            Assert.That(newProfileViewModel.PageIndex, Is.EqualTo(0), "newProfileViewModel.PageIndex");
            Assert.That(newProfileViewModel.CurrentVisualState, Is.EqualTo(NewProfileViewModel.PageOneStateName), "newProfileViewModel.CurrentVisualState");
            Assert.That(newProfileViewModel.CreateNFO, Is.False, "newProfileViewModel.CreateNFO");
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

            for (int i=0; i<newProfileViewModel.DirectoryFormats.Count; i++)
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
    }
}
