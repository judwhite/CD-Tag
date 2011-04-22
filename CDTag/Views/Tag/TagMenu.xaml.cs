using System.Windows.Input;
using CDTag.ViewModel.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagMenu.xaml
    /// </summary>
    public partial class TagMenu : ViewBase
    {
        public TagMenu()
            : this(Unity.Resolve<ITagViewModel>())
        {
        }
     
        public TagMenu(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
