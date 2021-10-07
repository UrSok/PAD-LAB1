using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PAD.LAB1.WinUI.Views
{
    /// <summary>
    /// Interaction logic for ConnectionLostView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(ConnectionLostViewModel))]
    public partial class ConnectionLostView : MvxWpfView
    {
        public ConnectionLostView()
        {
            InitializeComponent();
        }
    }
}
