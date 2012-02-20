using CDTag.Common;

namespace CDTag.Model.Profile
{
    public class Finish : ModelBase<Finish>
    {
        public Finish()
        {
            RenameDirectory = true;
            RenameAudioFiles = true;
            RenameCUEFiles = true;
            RenameImageFiles = true;
            RenameEACLogFiles = true;
            CreatePlaylist = true;
            NFO = FinishNFO.RenameExisting;
            WriteCUEFilesToPlaylist = true;
            DeleteOldFiles = true;
        }

        public bool RenameDirectory
        {
            get { return Get<bool>("RenameDirectory"); }
            set { Set("RenameDirectory", value); }
        }

        public bool RenameAudioFiles
        {
            get { return Get<bool>("RenameAudioFiles"); }
            set { Set("RenameAudioFiles", value); }
        }

        public bool RenameCUEFiles
        {
            get { return Get<bool>("RenameCUEFiles"); }
            set { Set("RenameCUEFiles", value); }
        }

        public bool RenameImageFiles
        {
            get { return Get<bool>("RenameImageFiles"); }
            set { Set("RenameImageFiles", value); }
        }

        public bool RenameEACLogFiles
        {
            get { return Get<bool>("RenameEACLogFiles"); }
            set { Set("RenameEACLogFiles", value); }
        }

        public bool CreatePlaylist
        {
            get { return Get<bool>("CreatePlaylist"); }
            set { Set("CreatePlaylist", value); }
        }

        public bool WriteCUEFilesToPlaylist
        {
            get { return Get<bool>("WriteCUEFilesToPlaylist"); }
            set { Set("WriteCUEFilesToPlaylist", value); }
        }

        public bool CreateSFV
        {
            get { return Get<bool>("CreateSFV"); }
            set { Set("CreateSFV", value); }
        }

        public bool CreateMD5
        {
            get { return Get<bool>("CreateMD5"); }
            set { Set("CreateMD5", value); }
        }

        public bool CreateSHA1
        {
            get { return Get<bool>("CreateSHA1"); }
            set { Set("CreateSHA1", value); }
        }

        public FinishNFO NFO
        {
            get { return Get<FinishNFO>("NFO"); }
            set { Set("NFO", value); }
        }

        public bool DeleteOldFiles
        {
            get { return Get<bool>("DeleteOldFiles"); }
            set { Set("DeleteOldFiles", value); }
        }

        public bool DeleteEmptySubdirectories
        {
            get { return Get<bool>("DeleteEmptySubdirectories"); }
            set { Set("DeleteEmptySubdirectories", value); }
        }
    }
}
