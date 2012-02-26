using System.Windows.Input;
using CDTag.Common;
using CDTag.View;
using CDTag.ViewModels.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagToolbar.xaml
    /// </summary>
    public partial class TagToolbar : ViewBase
    {
        public TagToolbar()
            : this(IoC.Resolve<ITagViewModel>())
        {
        }

        public TagToolbar(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            //wrenchMenu.GotFocus += delegate { IoC.Resolve<IDialogService>().CloseAddressTextBox(); };
        }
    }
}
