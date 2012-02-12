using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using CDTag.Common;
using CDTag.Common.Dispatcher;

namespace CDTag.FileBrowser.Model
{
    /// <summary>
    /// FileCollection
    /// </summary>
    public class FileCollection : ObservableCollection<FileView>
    {
        private static readonly IDispatcher _dispatcher;

        static FileCollection()
        {
            _dispatcher = IoC.Resolve<IDispatcher>();
        }

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
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(() => OnCollectionChanged(e));
                return;
            }
            
            base.OnCollectionChanged(e);
        }
    }
}
