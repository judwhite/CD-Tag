using CDTag.Common;

namespace CDTag.Model.Profile.NewProfile
{
    public class FormatItem : ModelBase<FormatItem>
    {
        public string FormatString
        {
            get { return Get<string>("FormatString"); }
            set { Set("FormatString", value); }
        }

        public string Result
        {
            get { return Get<string>("Result"); }
            set { Set("Result", value); }
        }
    }
}
