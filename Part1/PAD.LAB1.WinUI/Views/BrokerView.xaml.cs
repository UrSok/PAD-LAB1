﻿using MvvmCross.Platforms.Wpf.Presenters.Attributes;
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
    /// Interaction logic for BrokerView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(BrokerViewModel))]
    public partial class BrokerView : MvxWpfView
    {
        public BrokerView()
        {
            InitializeComponent();
        }
    }
}
