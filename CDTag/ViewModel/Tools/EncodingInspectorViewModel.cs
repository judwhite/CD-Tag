using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tools
{
    public class EncodingInspectorViewModel : ViewModelBase, IEncodingInspectorViewModel
    {
        public EncodingInspectorViewModel(EventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
