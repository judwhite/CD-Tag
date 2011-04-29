using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Model.About;

namespace CDTag.ViewModel.About
{
    public interface IAboutViewModel : IViewModelBase
    {
        ICommand NavigateCommand { get; }
        string ReleaseNotes { get; }
        ObservableCollection<ComponentInformation> ComponentsCollection { get; }
        ICommand CopyComponentCommand { get; }
        ICommand CloseCommand { get; }
        string CopyrightText { get; }
        string VersionText { get; }
        DateTime ReleaseDate { get; }
    }
}
