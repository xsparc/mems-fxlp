﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using OxyPlot;
using OxyPlot.Series;

namespace mems_fx3lp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    

    public partial class MainWindow : Window
    {
        int x;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        internal void SwitchScreen(object sender)
        {

            var screen = ((UserControl)sender);
            if(screen!=null)
            {
                StackPanelPlot.Children.Clear();
                StackPanelPlot.Children.Add(screen);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ListViewItemPlot_Selected(object sender, RoutedEventArgs e)
        {
            if (x <= 1)
            {

                var item = new UserControlPlot();
                SwitchScreen(item);
            }
            else
            {
                var item = new UserControl1();
                SwitchScreen(item);
            }
        }
        private void ListViewItemFFT_Selected(object sender, RoutedEventArgs e)
        
            {
                var item = new UserControl2();
                SwitchScreen(item);
            }
        
        private void ListViewItemOverview_Selected(object sender, RoutedEventArgs e)
        {
            var item = new UserControlOverview();
            SwitchScreen(item);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void McCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            x = 1;
            // var item = new UserControl1();
            // SwitchScreen(item);
        }
        private void McCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            x = 2;
            // var item = new UserControlPlot();
            //  SwitchScreen(item);
        }

        private void ListViewItemPlotDUT_Selected(object sender, RoutedEventArgs e)
        {
            var item = new UserControlPlotDUT();
            SwitchScreen(item);
        }

        private void ListViewItemUCBoardVM_Selected(object sender, RoutedEventArgs e)
        {
            var item = new UserControlBasic();
            SwitchScreen(item);
        }
    }
}
