using CDTag.ViewModel.Tools;

namespace CDTag.Views.Tools
{
    /// <summary>
    /// Interaction logic for EncodingInspectorWindow.xaml
    /// </summary>
    public partial class EncodingInspectorWindow : WindowViewBase
    {
        public EncodingInspectorWindow(IEncodingInspectorViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
