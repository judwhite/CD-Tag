using CDTag.Common;
using CDTag.Common.Model;

namespace CDTag.Model.Profile
{
    public class NamingFormat : ModelBase<NamingFormat>
    {
        public string Directory
        {
            get { return Get<string>("Directory"); }
            set { Set("Directory", value); }
        }

        public string AudioFile
        {
            get { return Get<string>("AudioFile"); }
            set { Set("AudioFile", value); }
        }

        public string CUE
        {
            get { return Get<string>("CUE"); }
            set { Set("CUE", value); }
        }

        public string Playlist
        {
            get { return Get<string>("Playlist"); }
            set { Set("Playlist", value); }
        }

        public string Checksum
        {
            get { return Get<string>("Checksum"); }
            set { Set("Checksum", value); }
        }

        public string NFO
        {
            get { return Get<string>("NFO"); }
            set { Set("NFO", value); }
        }

        public string Images
        {
            get { return Get<string>("Images"); }
            set { Set("Images", value); }
        }

        public string EACLog
        {
            get { return Get<string>("EACLog"); }
            set { Set("EACLog", value); }
        }
    }
}
