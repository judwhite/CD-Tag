using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace CDTag.FileBrowser.Model
{
    /// <summary>
    /// FileCollection
    /// </summary>
    public class FileCollection : ObservableCollection<FileView>
    {
        internal FileView Find(string name)
        {
            FileView item = null;

            foreach (FileView fileView in Items)
            {
                if (fileView.Name == name)
                {
                    item = fileView;
                    break;
                }
            }

            return item;
        }

        /// <summary>Raises the <see cref="ObservableCollection&lt;FileView&gt;.CollectionChanged"/> event.</summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Make sure we fire ListChanged on the UI thread

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(new Action(() => OnCollectionChanged(e)));
                return;
            }
            
            base.OnCollectionChanged(e);
        }
    }
}
