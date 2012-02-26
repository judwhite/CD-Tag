using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Events;
using CDTag.Common.Mvvm;
using CDTag.Model.Profile;
using CDTag.Model.Profile.NewProfile;
using CDTag.Model.Tag;

namespace CDTag.ViewModels.Profile.NewProfile
{
    public class NewProfileViewModel : ViewModelBase<NewProfileViewModel>, INewProfileViewModel
    {
        public const string PageOneStateName = "PageOne";
        public const string PageTwoStateName = "PageTwo";

        private readonly string NextText = "_Next"; // TODO: Localize
        private readonly string FinishText = "_Finish"; // TODO: Localize

        private readonly DelegateCommand _nextCommand;
        private readonly DelegateCommand _previousCommand;

        private readonly ObservableCollection<FormatItem> _directoryFormats;
        private readonly ObservableCollection<FormatItem> _audioFileFormats;
        private readonly UserProfile _profile = new UserProfile();
        private readonly Album _album;

        private bool _storedCreateSampleNFO = true;
        private bool _storedHasExistingNFO;

        private bool _confirmedOverwrite;

        public NewProfileViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _nextCommand = new DelegateCommand(Next, () => !string.IsNullOrWhiteSpace(Profile.ProfileName));
            _previousCommand = new DelegateCommand(Previous, () => PageIndex > 0);

            NextButtonText = NextText;

            EnhancedPropertyChanged += NewProfileViewModel_EnhancedPropertyChanged;
            Profile.EnhancedPropertyChanged += Profile_EnhancedPropertyChanged;
            Profile.FileNaming.EnhancedPropertyChanged += FileNaming_EnhancedPropertyChanged;

            var tracks = new List<AlbumTrack>();
            tracks.Add(new AlbumTrack { Artist = "Björk", Album = "Medúlla", TrackNumber = "3", Title = "Where Is The Line?", ReleaseDate = "2004" });
            _album = new Album(@"C:\Bjork - Medulla - 2004", tracks);

            _directoryFormats = new ObservableCollection<FormatItem>
            {
                new FormatItem { FormatString = "<Artist> - <Album> (<Year>)" },
                new FormatItem { FormatString = "<Artist> - <Album> - <Year>" },
                new FormatItem { FormatString = "<Artist> - (<Year>) - <Album>" },
                new FormatItem { FormatString = "<Artist> - <Year> - <Album>" },
                new FormatItem { FormatString = "<Artist> - <Album>" },
            };

            _audioFileFormats = new ObservableCollection<FormatItem>
            {
                new FormatItem { FormatString = "<Track> - <Artist> - <Song>" },
                new FormatItem { FormatString = "<Artist> - <Track> - <Song>" },
                new FormatItem { FormatString = "<Track> - <Song>" },
            };

            DirectoryFormat = _directoryFormats[0];
            AudioFileFormat = _audioFileFormats[0];

            UpdateResults();

            CurrentVisualState = PageOneStateName;
        }

        private void Profile_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<UserProfile> e)
        {
            if (e.IsProperty(p => p.ProfileName))
            {
                _confirmedOverwrite = false;
            }
        }

        private void FileNaming_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<FileNaming> e)
        {
            if (e.IsProperty(p => p.UseLatinCharactersOnly) ||
                e.IsProperty(p => p.UseStandardCharactersOnly))
            {
                UpdateResults();
            }
            else if (e.IsProperty(p => p.UseUnderscores))
            {
                List<FormatItem> list = new List<FormatItem>();
                list.AddRange(_directoryFormats);
                list.AddRange(_audioFileFormats);

                bool useUnderscores = (bool)e.NewValue;
                foreach (var item in list)
                {
                    if (useUnderscores)
                    {
                        item.FormatString = item.FormatString.Replace("> - <", ">-<");
                        item.FormatString = item.FormatString.Replace("> - (", ">-(");
                        item.FormatString = item.FormatString.Replace(") - <", ")-<");
                    }
                    else
                    {
                        item.FormatString = item.FormatString.Replace(">-<", "> - <");
                        item.FormatString = item.FormatString.Replace(">-(", "> - (");
                        item.FormatString = item.FormatString.Replace(")-<", ") - <");
                    }
                }

                UpdateResults();
            }
        }

        private void NewProfileViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<NewProfileViewModel> e)
        {
            if (e.IsProperty(p => p.CreateNFO))
            {
                if (!CreateNFO)
                {
                    _storedCreateSampleNFO = CreateSampleNFO;
                    _storedHasExistingNFO = HasExistingNFO;

                    CreateSampleNFO = false;
                    HasExistingNFO = false;
                }
                else
                {
                    CreateSampleNFO = _storedCreateSampleNFO;
                    HasExistingNFO = _storedHasExistingNFO;
                }
            }
            else if (e.IsProperty(p => p.CreateSampleNFO))
            {
                if (CreateSampleNFO)
                {
                    HasExistingNFO = false;
                }
            }
            else if (e.IsProperty(p => p.HasExistingNFO))
            {
                if (HasExistingNFO)
                {
                    CreateSampleNFO = false;
                }
            }
            else if (e.IsProperty(p => p.PageIndex))
            {
                if (PageIndex == 0)
                {
                    CurrentVisualState = PageOneStateName;
                }
                else if (PageIndex == 1)
                {
                    CurrentVisualState = PageTwoStateName;
                }
            }
            else if (e.IsProperty(p => p.CurrentVisualState))
            {
                if (CurrentVisualState == PageOneStateName)
                {
                    IsProfileNameFocused = true;
                }
            }
            else if (e.IsProperty(p => p.DirectoryFormat) || e.IsProperty(p => p.AudioFileFormat))
            {
                var oldFormatItem = e.OldValue as FormatItem;
                if (oldFormatItem != null)
                    oldFormatItem.IsSelected = false;
                var newFormatItem = e.NewValue as FormatItem;
                if (newFormatItem != null)
                    newFormatItem.IsSelected = true;

                if (newFormatItem == null && oldFormatItem != null)
                {
                    if (e.IsProperty(p => p.DirectoryFormat))
                    {
                        DirectoryFormat = oldFormatItem;
                    }
                    else if (e.IsProperty(p => p.AudioFileFormat))
                    {
                        AudioFileFormat = oldFormatItem;
                    }
                }
            }
        }

        private void UpdateResults()
        {
            foreach (var item in AudioFileFormats)
            {
                item.Result = _profile.GetFileName(item.FormatString, _album.Tracks[0], ".mp3");
            }

            foreach (var item in DirectoryFormats)
            {
                item.Result = _profile.GetDirectoryName(item.FormatString, _album);
            }
        }

        private void Next()
        {
            if (PageIndex == 0)
            {
                if (!ValidateProfileName())
                {
                    IsProfileNameFocused = true;
                    return;
                }

                PageIndex += 1;
                NextButtonText = FinishText;
            }
            else if (PageIndex == 1)
            {
                // File Naming

                List<NamingFormatGroup> formatGroups = new List<NamingFormatGroup> {
                    Profile.FileNaming.SingleCD,
                    Profile.FileNaming.MultiCD,
                    Profile.FileNaming.Vinyl
                };

                string space = Profile.FileNaming.UseUnderscores ? "" : " ";
                string directoryFormat = DirectoryFormat.FormatString;
                string singleArtistAudioFileFormat = AudioFileFormat.FormatString;
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

                foreach (var formatGroup in formatGroups)
                {
                    formatGroup.SingleArtist.AudioFile = singleArtistAudioFileFormat;
                    formatGroup.VariousArtists.AudioFile = variousArtistsAudioFileFormat;

                    List<NamingFormat> formats = new List<NamingFormat> {
                        formatGroup.SingleArtist,
                        formatGroup.VariousArtists
                    };

                    foreach (var format in formats)
                    {
                        format.Directory = directoryFormat;

                        format.CUE = fileFormat;
                        format.Playlist = fileFormat;
                        format.Checksum = fileFormat;
                        format.NFO = fileFormat;
                        format.Images = imageFileformat;
                        format.EACLog = fileFormat;
                    }
                }

                // NFO
                if (CreateNFO)
                {
                    Profile.Finish.NFO = FinishNFO.CreateNew;
                    Profile.NFOOptions.ShowReleaseScreen = true;

                    if (CreateSampleNFO)
                    {
                        string sampleNFO = UserProfile.GetSampleNFO();
                        string nfoShortFileName = string.Format("{0}.nfo", Profile.ProfileName);
                        string nfoFullPath = Path.Combine(_pathService.ProfileDirectory, nfoShortFileName);

                        if (File.Exists(nfoFullPath))
                        {
                            string messageBoxText = string.Format("'{0}' exists. Overwrite?", nfoFullPath);
                            var result = MessageBox(messageBoxText, "New Profile", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result != MessageBoxResult.Yes)
                                return;
                        }

                        Profile.NFOOptions.TemplatePath = nfoShortFileName;

                        File.WriteAllText(nfoFullPath, sampleNFO);
                    }
                    else
                    {
                        IDialogService dialogService = IoC.Resolve<IDialogService>();
                        string fileName;
                        bool? result = dialogService.ShowOpenFileDialog(title: "Open file", filter: "*.nfo|*.nfo", fileName: out fileName);
                        if (result != true)
                            return;

                        Profile.NFOOptions.TemplatePath = fileName;
                    }
                }
                else
                {
                    Profile.Finish.NFO = FinishNFO.RenameExisting;
                    Profile.NFOOptions.ShowReleaseScreen = false;
                    Profile.NFOOptions.TemplatePath = null;
                }

                Profile.Save();

                CloseWindow(); // TODO: Open Edit Profile
            }
        }

        private bool ValidateProfileName()
        {
            string message;
            if (!Profile.ValidateProfileName(out message))
            {
                MessageBoxEvent messageBoxEvent = new MessageBoxEvent
                {
                    MessageBoxText = message,
                    Caption = "New profile",
                    MessageBoxButton = MessageBoxButton.OK,
                    MessageBoxImage = MessageBoxImage.Information
                };

                MessageBox(messageBoxEvent);
                return false;
            }

            // Check if file exists
            string fileName = Profile.GetProfileFileName();
            if (File.Exists(fileName) && !_confirmedOverwrite)
            {
                // TODO: Localize
                MessageBoxEvent messageBoxEvent = new MessageBoxEvent
                {
                    MessageBoxText = string.Format("'{0}' exists. Overwrite?", fileName),
                    Caption = "New profile",
                    MessageBoxButton = MessageBoxButton.YesNo,
                    MessageBoxImage = MessageBoxImage.Question
                };

                var result = MessageBox(messageBoxEvent);

                if (result != MessageBoxResult.Yes)
                    return false;

                _confirmedOverwrite = true;
            }

            // Check if able to create file
            if (!File.Exists(fileName))
            {
                try
                {
                    File.WriteAllBytes(fileName, new byte[0]);
                }
                catch
                {
                    // TODO: Localize
                    MessageBoxEvent messageBoxEvent = new MessageBoxEvent
                    {
                        MessageBoxText = string.Format("'{0}' cannot be created.", fileName),
                        Caption = "New profile",
                        MessageBoxButton = MessageBoxButton.OK,
                        MessageBoxImage = MessageBoxImage.Information
                    };

                    MessageBox(messageBoxEvent);

                    return false;
                }

                File.Delete(fileName);
            }

            return true;
        }

        private void Previous()
        {
            if (PageIndex > 0)
            {
                PageIndex -= 1;
                NextButtonText = NextText;
            }
        }

        public FormatItem DirectoryFormat
        {
            get { return Get<FormatItem>("DirectoryFormat"); }
            set { Set("DirectoryFormat", value); }
        }

        public FormatItem AudioFileFormat
        {
            get { return Get<FormatItem>("AudioFileFormat"); }
            set { Set("AudioFileFormat", value); }
        }

        public bool CreateNFO
        {
            get { return Get<bool>("CreateNFO"); }
            set { Set("CreateNFO", value); }
        }

        public bool CreateSampleNFO
        {
            get { return Get<bool>("CreateSampleNFO"); }
            set { Set("CreateSampleNFO", value); }
        }

        public bool HasExistingNFO
        {
            get { return Get<bool>("HasExistingNFO"); }
            set { Set("HasExistingNFO", value); }
        }

        public ICommand PreviousCommand
        {
            get { return _previousCommand; }
        }

        public ICommand NextCommand
        {
            get { return _nextCommand; }
        }

        public int PageIndex
        {
            get { return Get<int>("PageIndex"); }
            private set { Set("PageIndex", value); }
        }

        public bool IsProfileNameFocused
        {
            get { return Get<bool>("IsProfileNameFocused"); }
            set
            {
                if (value) // force change for Binding (forcing PropertyChanged isn't sufficient)
                    Set("IsProfileNameFocused", false);
                Set("IsProfileNameFocused", value);
            }
        }

        public ObservableCollection<FormatItem> DirectoryFormats
        {
            get { return _directoryFormats; }
        }

        public ObservableCollection<FormatItem> AudioFileFormats
        {
            get { return _audioFileFormats; }
        }

        public UserProfile Profile
        {
            get { return _profile; }
        }

        public string NextButtonText
        {
            get { return Get<string>("NextButtonText"); }
            private set { Set("NextButtonText", value); }
        }
    }
}
