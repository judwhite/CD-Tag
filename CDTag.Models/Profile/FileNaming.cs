using System;
using System.ComponentModel;
using System.Diagnostics;
using CDTag.Common;
using CDTag.Common.Events;
using CDTag.Common.Model;

namespace CDTag.Model.Profile
{
    public class FileNaming : ModelBase<FileNaming>
    {
        private bool _savedUseLatinCharactersOnly;

        public FileNaming()
        {
            SingleCD = new NamingFormatGroup();
            MultiCD = new NamingFormatGroup();
            Vinyl = new NamingFormatGroup();

            EnhancedPropertyChanged += FileNaming_EnhancedPropertyChanged;
        }

        private void FileNaming_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<FileNaming> e)
        {
            if (e.IsProperty(p => p.UseStandardCharactersOnly))
            {
                if (UseStandardCharactersOnly)
                {
                    _savedUseLatinCharactersOnly = UseLatinCharactersOnly;
                    UseLatinCharactersOnly = true;
                }
                else
                {
                    UseLatinCharactersOnly = _savedUseLatinCharactersOnly;
                }
            }
        }

        public NamingFormatGroup SingleCD
        {
            get { return Get<NamingFormatGroup>("SingleCD"); }
            set { Set("SingleCD", value); }
        }

        public NamingFormatGroup MultiCD
        {
            get { return Get<NamingFormatGroup>("MultiCD"); }
            set { Set("MultiCD", value); }
        }

        public NamingFormatGroup Vinyl
        {
            get { return Get<NamingFormatGroup>("Vinyl"); }
            set { Set("Vinyl", value); }
        }

        public int? FileCharacterLimit
        {
            get { return Get<int?>("FileCharacterLimit"); }
            set { Set("FileCharacterLimit", value); }
        }

        public int? DirectoryCharacterLimit
        {
            get { return Get<int?>("DirectoryCharacterLimit"); }
            set { Set("DirectoryCharacterLimit", value); }
        }

        public bool UseUnderscores
        {
            get { return Get<bool>("UseUnderscores"); }
            set { Set("UseUnderscores", value); }
        }

        public bool UseStandardCharactersOnly
        {
            get { return Get<bool>("UseStandardCharactersOnly"); }
            set { Set("UseStandardCharactersOnly", value); }
        }

        public bool UseLatinCharactersOnly
        {
            get { return Get<bool>("UseLatinCharactersOnly"); }
            set { Set("UseLatinCharactersOnly", value); }
        }

        public bool DontChangeCaseWordsWithAllCaps
        {
            get { return Get<bool>("DontChangeCaseWordsWithAllCaps"); }
            set { Set("DontChangeCaseWordsWithAllCaps", value); }
        }

        public bool DontChangeCaseWordsWithNonLetter
        {
            get { return Get<bool>("DontChangeCaseWordsWithNonLetter"); }
            set { Set("DontChangeCaseWordsWithNonLetter", value); }
        }
    }
}
