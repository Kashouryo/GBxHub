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

namespace GBxHub
{
    /// <summary>
    /// ReaderPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReaderPage : Page
    {
        private ImageBrush pendingBrush = new ImageBrush();
        private ImageBrush connectedBrush = new ImageBrush();
        public ReaderPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
