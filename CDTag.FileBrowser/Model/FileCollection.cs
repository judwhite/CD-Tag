using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CDTag.FileBrowser.Model
{
    /// <summary>
    /// FileCollection
    /// </summary>
    public class FileCollection : ObservableCollection<FileView>
    {
        private readonly AsyncOperation _oper;
        //internal bool _suspend;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCollection"/> class.
        /// </summary>
        public FileCollection()
        {
            // Setup Async
            _oper = AsyncOperationManager.CreateOperation(null);
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

        /// <summary>Raises the <see cref="E:CollectionChanged"/> event.</summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            //base.OnListChanged(e);
            //if (!_suspend)
            {
                // Debug Info
                //WriteDebugThreadInfo("OnListChanged");

                // We need to marshall changes back to the UI thread since FileSystemWatcher fires
                // updates on any ol' thread.
                _oper.Post(PostCallback, e);
            }
        }

        private void PostCallback(object state)
        {
            // Make sure we fire ListChanged on the UI thread
            var args = (state as NotifyCollectionChangedEventArgs);
            base.OnCollectionChanged(args);

            // Debug info
            //WriteDebugThreadInfo("PostCallback");
        }

        /*        private static void WriteDebugThreadInfo(string source)
                {
        #if DEBUG
                    Thread thread = Thread.CurrentThread;
                    string code = thread.GetHashCode().ToString();

                    Debug.WriteLine(string.Format("{0}: {1}, background: {2}, ThreadPoolThread: {3}", source, code, thread.IsBackground, thread.IsThreadPoolThread));
        #endif
                }
         */
    }
}
