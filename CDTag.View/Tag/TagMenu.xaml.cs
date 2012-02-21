using CDTag.Common;
using CDTag.ViewModels.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagMenu.xaml
    /// </summary>
    public partial class TagMenu : ViewBase
    {
        public TagMenu()
            : this(IoC.Resolve<ITagViewModel>())
        {
        }
     
        public TagMenu(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
