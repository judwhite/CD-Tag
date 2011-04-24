using System;
using System.Collections.Generic;
using CDTag.FileBrowser.Model;

namespace CDTag.FileBrowser.ViewModel
{
    /// <summary>
    /// IDirectoryController interface. See <see cref="DirectoryController"/>.
    /// </summary>
    public interface IDirectoryController
    {
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
        /// Occurs when <see cref="IsGoBackEnabled"/> has changed.
        /// </summary>
        event EventHandler GoBackEnabledChanged;

        /// <summary>
        /// Occurs when <see cref="IsGoForwardEnabled"/> has changed.
        /// </summary>
        event EventHandler GoForwardEnabledChanged;

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
        /// Gets a value indicating whether GoBack should be enabled.
        /// </summary>
        /// <value><c>true</c> if GoBack should be enabled; otherwise, <c>false</c>.</value>
        bool IsGoBackEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether GoForward should be enabled.
        /// </summary>
        /// <value><c>true</c> if GoForward should be enabled; otherwise, <c>false</c>.</value>
        bool IsGoForwardEnabled { get; }

        /// <summary>
        /// Navigates to the specified <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory">The directory.</param>
        void NavigateTo(string directory);

        /// <summary>
        /// Go up a level.
        /// </summary>
        void GoUp();

        /// <summary>
        /// Go forward in the browsing history.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Go forward <paramref name="count"/> steps in the browsing history.
        /// </summary>
        /// <param name="count">The number of steps to go forward.</param>
        void GoForward(int count);

        /// <summary>
        /// Go back in the browsing history.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Go back <paramref name="count"/> steps in the browsing history.
        /// </summary>
        /// <param name="count">The number of steps to go back.</param>
        void GoBack(int count);

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
