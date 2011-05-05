using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag.MassTag
{
    public class MassTagViewModel : ViewModelBase, IMassTagViewModel
    {
        public MassTagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
