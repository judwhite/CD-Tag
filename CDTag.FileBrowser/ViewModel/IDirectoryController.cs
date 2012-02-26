using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Common.Mvvm;
using CDTag.FileBrowser.Model;

namespace CDTag.FileBrowser.ViewModel
{
    /// <summary>
    /// IDirectoryController interface. See <see cref="DirectoryController"/>.
    /// </summary>
    public interface IDirectoryController : IViewModelBase<IDirectoryController>
    {
        /// <summary>Gets the go back command.</summary>
        ICommand GoBackCommand { get; }

        /// <summary>Gets the go forward command.</summary>
        ICommand GoForwardCommand { get; }

        /// <summary>Gets the go up command.</summary>
        ICommand GoUpCommand { get; }

        /// <summary>Gets the select all command.</summary>
        ICommand SelectAllCommand { get; }
        
        /// <summary>Gets the invert selection command.</summary>
        ICommand InvertSelectionCommand { get; }

        /// <summary>Gets the file collection.</summary>
        FileCollection FileCollection { get; }

        /// <summary>Occurs when navigating starts.</summary>
        event EventHandler Navigating;

        /// <summary>Occurs when navigation is complete.</summary>
        event EventHandler NavigationComplete;

        /// <summary>Occurs when a request is made to hide the address text box.</summary>
        event EventHandler HideAddressTextBoxRequested;
        
        /// <summary>Occurs when a request is made to focus the address text box.</summary>
        event EventHandler FocusAddressTextBoxRequested;

        /// <summary>Occurs when a request is made to close the popup.</summary>
        event EventHandler ClosePopupRequested;

        /// <summary>Gets the current directory.</summary>
        /// <value>The current directory.</value>
        string CurrentDirectory { get; set; }

        /// <summary>Sets the initial directory.</summary>
        /// <value>The initial directory.</value>
        string InitialDirectory { set; }

        /// <summary>Gets or sets the typing directory.</summary>
        /// <value>The typing directory.</value>
        string TypingDirectory { get; set; }

        /// <summary>Gets the directory size in bytes.</summary>
        /// <value>The directory size in bytes.</value>
        long DirectorySizeBytes { get; }

        /// <summary>Gets the back history.</summary>
        /// <returns>The back history.</returns>
        List<FileView> GetBackHistory();

        /// <summary>Gets the forward history.</summary>
        /// <returns>The forward history.</returns>
        List<FileView> GetForwardHistory();

        /// <summary>Gets the selected items.</summary>
        List<FileView> SelectedItems { get; }

        /// <summary>Refreshes the view.</summary>
        void RefreshExplorer();

        /// <summary>Clears the history.</summary>
        void ClearHistory();

        /// <summary>Gets the sub directories of the <see cref="CurrentDirectory" />.</summary>
        ObservableCollection<string> SubDirectories { get; }

        /// <summary>Gets the history.</summary>
        ObservableCollection<HistoryItem> History { get; }

        /// <summary>Navigates to the specified history offset.</summary>
        /// <param name="offset">The history offset.</param>
        void Navigate(int offset);

        /// <summary>Focuses the address text box.</summary>
        void FocusAddressTextBox();
        
        /// <summary>Hides the address text box.</summary>
        void HideAddressTextBox();

        /// <summary>Closes the popup.</summary>
        void ClosePopup();
    }
}
