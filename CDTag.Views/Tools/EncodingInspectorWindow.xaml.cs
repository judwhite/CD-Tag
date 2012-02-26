using CDTag.ViewModels.Tools;
using CDTag.Views.Interfaces.Tools;

namespace CDTag.Views.Tools
{
    /// <summary>
    /// Interaction logic for EncodingInspectorWindow.xaml
    /// </summary>
    public partial class EncodingInspectorWindow : WindowViewBase, IEncodingInspectorWindow
    {
        public EncodingInspectorWindow(IEncodingInspectorViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
