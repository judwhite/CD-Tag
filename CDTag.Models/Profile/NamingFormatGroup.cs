using CDTag.Common;
using CDTag.Common.Model;

namespace CDTag.Model.Profile
{
    public class NamingFormatGroup : ModelBase<NamingFormatGroup>
    {
        public NamingFormatGroup()
        {
            SingleArtist = new NamingFormat();
            VariousArtists = new NamingFormat();
        }

        public NamingFormat SingleArtist
        {
            get { return Get<NamingFormat>("SingleArtist"); }
            set { Set("SingleArtist", value); }
        }

        public NamingFormat VariousArtists
        {
            get { return Get<NamingFormat>("VariousArtists"); }
            set { Set("VariousArtists", value); }
        }
    }
}
