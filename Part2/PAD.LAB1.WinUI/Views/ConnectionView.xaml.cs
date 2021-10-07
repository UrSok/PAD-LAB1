using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.ViewModels;

namespace PAD.LAB1.WinUI.Views
{
    /// <summary>
    /// Interaction logic for ConnectionView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(ConnectionViewModel))]
    public partial class ConnectionView : MvxWpfView
    {
        public ConnectionView()
        {
            InitializeComponent();
        }
    }
}
