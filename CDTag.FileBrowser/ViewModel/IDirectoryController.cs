using System;
using System.Collections.Generic;
using System.Windows.Input;
using CDTag.FileBrowser.Model;

namespace CDTag.FileBrowser.ViewModel
{
    /// <summary>
    /// IDirectoryController interface. See <see cref="DirectoryController"/>.
    /// </summary>
    public interface IDirectoryController
    {
        /// <summary>Gets the go back command.</summary>
        ICommand GoBackCommand { get; }

        /// <summary>Gets the go forward command.</summary>
        ICommand GoForwardCommand { get; }

        /// <summary>Gets the go up command.</summary>
        ICommand GoUpCommand { get; }

        /// <summary>Gets the file collection.</summary>
        FileCollection FileCollection { get; }

        /// <summary>Occurs when select all requested.</summary>
        event EventHandler SelectAllRequested;

        /// <summary>Occurs when invert selection requested.</summary>
        event EventHandler InvertSelectionRequested;

        /// <summary>
        /// Occurs when navigating starts.
        /// </summary>
        event EventHandler Navigating;

        /// <summary>
        /// Occurs when navigation is complete.
        /// </summary>
        event EventHandler NavigationComplete;

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <value>The current directory.</value>
        FileView CurrentDirectory { get; }

        /// <summary>
        /// Gets the directory size in bytes.
        /// </summary>
        /// <value>The directory size in bytes.</value>
        long DirectorySizeBytes { get; }

        /// <summary>
        /// Navigates to the specified <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory">The directory.</param>
        void NavigateTo(string directory);

        /// <summary>
        /// Gets the back history.
        /// </summary>
        /// <returns>The back history.</returns>
        List<FileView> GetBackHistory();

        /// <summary>
        /// Gets the forward history.
        /// </summary>
        /// <returns>The forward history.</returns>
        List<FileView> GetForwardHistory();

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        void RefreshExplorer();

        /// <summary>Selects all.</summary>
        void SelectAll();

        /// <summary>Inverts the selection.</summary>
        void InvertSelection();
    }
}
