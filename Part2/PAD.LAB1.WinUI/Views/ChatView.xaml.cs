using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.ViewModels;


namespace PAD.LAB1.WinUI.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(ChatViewModel))]
    public partial class ChatView : MvxWpfView
    {
        public ChatView()
        {
            InitializeComponent();
        }
    }
}
