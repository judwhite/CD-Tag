using System;
using System.IO;
using System.Text;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Hash;
using CDTag.Common.Json;
using CDTag.Common.Model;

namespace CDTag.Model.Profile
{
    public partial class UserProfile : ModelBase<UserProfile>
    {
        private static readonly IPathService _pathService;

        static UserProfile()
        {
            _pathService = IoC.Resolve<IPathService>();
        }

        public static UserProfile Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");
            if (!File.Exists(path))
                throw new FileNotFoundException(string.Format("'{0}' not found.", path), path);

            string json = File.ReadAllText(path);
            UserProfile profile = JsonSerializer.ReadObject<UserProfile>(json);
            profile.ProfileName = Path.GetFileNameWithoutExtension(path);
            profile.LastModified = File.GetLastWriteTime(path);

            profile.UpdateModelHash();

            return profile;
        }

        public static string GetSampleNFO()
        {
            return Properties.Resources.SampleNFO;
        }
    }
}
