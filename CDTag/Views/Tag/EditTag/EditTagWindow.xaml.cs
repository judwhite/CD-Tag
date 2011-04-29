using CDTag.ViewModel.Tag.EditTag;

namespace CDTag.Views.Tag.EditTag
{
    /// <summary>
    /// Interaction logic for EditTagWindow.xaml
    /// </summary>
    public partial class EditTagWindow : WindowViewBase
    {
        public EditTagWindow(IEditTagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
