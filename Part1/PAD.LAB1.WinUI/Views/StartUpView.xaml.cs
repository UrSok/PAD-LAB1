using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using PAD.LAB1.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StartUpView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(StartUpViewModel))]
    public partial class StartUpView : MvxWpfView
    {
        private readonly Regex numberOnlyRegex = new Regex("[^0-9]+");

        public StartUpView()
        {
            InitializeComponent();
        }
        
        private bool IsTextAllowed(string text)
        {
            return !numberOnlyRegex.IsMatch(text);
        }

        private void NumberValidationTextBoxOnInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void NumberValidationTextBoxOnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
