using CDTag.ViewModel.Tag.MassTag;

namespace CDTag.Views.Tag.MassTag
{
    /// <summary>
    /// Interaction logic for MassTagWindow.xaml
    /// </summary>
    public partial class MassTagWindow : WindowViewBase
    {
        public MassTagWindow(IMassTagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
