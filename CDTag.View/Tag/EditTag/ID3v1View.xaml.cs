using CDTag.Common;
using CDTag.ViewModel.Tag.EditTag;

namespace CDTag.Views.Tag.EditTag
{
    /// <summary>
    /// Interaction logic for ID3v1View.xaml
    /// </summary>
    public partial class ID3v1View : ViewBase
    {
        public ID3v1View()
            : this(IoC.Resolve<IID3v1ViewModel>())
        {
        }

        public ID3v1View(IID3v1ViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
