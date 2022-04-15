﻿using System;
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
using DoAn1_WPF.ViewModel;

namespace DoAn1_WPF.UserControlBar
{
    /// <summary>
    /// Interaction logic for ControlBarQL.xaml
    /// </summary>
    public partial class ControlBarQL : UserControl
    {
        public ControlBarViewModel Viewmodel { get; set; }
        public ControlBarQL()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new ControlBarViewModel();
        }
    }
}
