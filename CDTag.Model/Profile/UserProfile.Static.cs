using System;
using System.IO;
using CDTag.Common;
using CDTag.Common.Json;

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
            return profile;
        }

        public static string GetSampleNFO()
        {
            return Properties.Resources.SampleNFO;
        }
    }
}
