using System;
using System.Collections.Generic;

namespace CDTag.FileBrowser
{
    /// <summary>
    /// IExplorer
    /// </summary>
    public interface IExplorer
    {
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
        /// Occurs when the selected file/directory list has changed.
        /// </summary>
        event EventHandler SelectionChanged;

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <value>The current directory.</value>
        string CurrentDirectory { get; }

        /// <summary>
        /// Gets the display name of the current directory.
        /// </summary>
        /// <value>The display name of the current directory.</value>
        string CurrentDirectoryDisplayName { get; }

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
        List<string> GetBackHistory();

        /// <summary>
        /// Gets the forward history.
        /// </summary>
        /// <returns>The forward history.</returns>
        List<string> GetForwardHistory();

        /// <summary>
        /// Focuses the address bar.
        /// </summary>
        void FocusAddressBar();

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <returns>The selected items.</returns>
        List<string> GetSelectedItems();

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        void RefreshExplorer();
    }
}
