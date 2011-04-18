using System;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag
{
    public class TagToolbarViewModel : ViewModelBase, ITagToolbarViewModel
    {
        private readonly DelegateCommand _backCommand;
        private readonly DelegateCommand _forwardCommand;
        private readonly DelegateCommand _upCommand;
        private readonly DelegateCommand _helpCommand;

        public TagToolbarViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _backCommand = new DelegateCommand(() => eventAggregator.GetEvent<GoBackEvent>().Publish(null));
            _forwardCommand = new DelegateCommand(() => { eventAggregator.GetEvent<GoForwardEvent>().Publish(null); });
            _upCommand = new DelegateCommand(() => { eventAggregator.GetEvent<GoUpEvent>().Publish(null); });
            _helpCommand = new DelegateCommand(() => { });
        }

        public ICommand BackCommand
        {
            get { return _backCommand; }
        }

        public ICommand ForwardCommand
        {
            get { return _forwardCommand; }
        }

        public ICommand UpCommand
        {
            get { return _upCommand; }
        }

        public ICommand HelpCommand
        {
            get { return _helpCommand; }
        }
    }
}
