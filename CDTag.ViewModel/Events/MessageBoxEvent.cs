using System.Windows;
using CDTag.View.Interfaces;

namespace CDTag.ViewModel.Events
{
    public class MessageBoxEvent
    {
        public IWindow Owner { get; set; }
        public string MessageBoxText { get; set; }
        public string Caption { get; set; }
        public MessageBoxButton MessageBoxButton { get; set; }
        public MessageBoxImage MessageBoxImage { get; set; }
        public MessageBoxResult Result { get; set; }
    }
}
