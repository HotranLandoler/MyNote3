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
using System.Windows.Shapes;

namespace MyNote3
{
    /// <summary>
    /// ExitDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ExitDialog : Window
    {
        public ExitDialog(ViewModels.ViewModel vm, Window owner)
        {
            this.DataContext = vm;
            this.Owner = owner;
            InitializeComponent();
        }
    }
}
