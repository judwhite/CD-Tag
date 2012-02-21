using CDTag.View.Interfaces.Tag.MassTag;
using CDTag.ViewModels.Tag.MassTag;

namespace CDTag.Views.Tag.MassTag
{
    /// <summary>
    /// Interaction logic for MassTagWindow.xaml
    /// </summary>
    public partial class MassTagWindow : WindowViewBase, IMassTagWindow
    {
        public MassTagWindow(IMassTagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
