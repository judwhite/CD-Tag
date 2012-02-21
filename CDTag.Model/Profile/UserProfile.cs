using System;
using System.Xml.Serialization;
using CDTag.Common;

namespace CDTag.Model.Profile
{
    public partial class UserProfile : ModelBase<UserProfile>
    {
        public UserProfile()
        {
            Revision = 1;
            FileNaming = new FileNaming();
            NFOOptions = new NFOOptions();
            Finish = new Finish();
            LastModified = DateTime.Now;
        }

        [XmlIgnore]
        public string ProfileName
        {
            get { return Get<string>("ProfileName"); }
            set { Set("ProfileName", value); }
        }

        [XmlIgnore]
        public DateTime LastModified
        {
            get { return Get<DateTime>("LastModified"); }
            set { Set("LastModified", value); }
        }

        public int Revision
        {
            get { return Get<int>("Revision"); }
            set { Set("Revision", value); }
        }

        public FileNaming FileNaming
        {
            get { return Get<FileNaming>("FileNaming"); }
            set { Set("FileNaming", value); }
        }

        public NFOOptions NFOOptions
        {
            get { return Get<NFOOptions>("NFOOptions"); }
            set { Set("NFOOptions", value); }
        }

        public Finish Finish
        {
            get { return Get<Finish>("Finish"); }
            set { Set("Finish", value); }
        }
    }
}
