using System.Windows.Input;
using CDTag.Common;
using CDTag.ViewModel.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagToolbar.xaml
    /// </summary>
    public partial class TagToolbar : ViewBase
    {
        public TagToolbar()
            : this(Unity.Resolve<ITagViewModel>())
        {
        }

        public TagToolbar(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            wrenchMenu.GotFocus += delegate { Unity.App.CloseAddressTextBox(); };
        }
    }
}
