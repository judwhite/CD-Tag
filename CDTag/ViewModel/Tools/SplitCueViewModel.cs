using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tools
{
    public class SplitCueViewModel : ViewModelBase, ISplitCueViewModel
    {
        public SplitCueViewModel(EventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
