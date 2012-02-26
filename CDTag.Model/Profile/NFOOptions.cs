using CDTag.Common;
using CDTag.Common.Model;

namespace CDTag.Model.Profile
{
    public class NFOOptions : ModelBase<NFOOptions>
    {
        public NFOOptions()
        {
            UseAverageBitrate = true;
            DecimalCharacter = ".";
        }

        public bool ShowReleaseScreen
        {
            get { return Get<bool>("ShowReleaseScreen"); }
            set { Set("ShowReleaseScreen", value); }
        }

        public string TemplatePath
        {
            get { return Get<string>("TemplatePath"); }
            set { Set("TemplatePath", value); }
        }

        public bool UseAverageBitrate
        {
            get { return Get<bool>("UseAverageBitrate"); }
            set { Set("UseAverageBitrate", value); }
        }

        public string SingleCDHeader
        {
            get { return Get<string>("SingleCDHeader"); }
            set { Set("SingleCDHeader", value); }
        }

        public string MultiCDHeader
        {
            get { return Get<string>("MultiCDHeader"); }
            set { Set("MultiCDHeader", value); }
        }

        public string TracklistFooter
        {
            get { return Get<string>("TracklistFooter"); }
            set { Set("TracklistFooter", value); }
        }

        public string ReleaseNotesHeader
        {
            get { return Get<string>("ReleaseNotesHeader"); }
            set { Set("ReleaseNotesHeader", value); }
        }

        public string ReleaseNotesFooter
        {
            get { return Get<string>("ReleaseNotesFooter"); }
            set { Set("ReleaseNotesFooter", value); }
        }

        public string DecimalCharacter
        {
            get { return Get<string>("DecimalCharacter"); }
            set { Set("DecimalCharacter", value); }
        }

        public bool AutomaticallyPreviewNFO
        {
            get { return Get<bool>("AutomaticallyPreviewNFO"); }
            set { Set("AutomaticallyPreviewNFO", value); }
        }
    }
}
