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
using System.Windows.Shapes;
using System.Diagnostics;
using MahApps.Metro.Controls;
using HMS.DataRecords;
using HMS.Managers;

namespace HMS.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow3.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SwitchToMakeMeasureMode_Checked(object sender, RoutedEventArgs e)
        {
            if(SwitchToMakeMeasureMode.IsLoaded==true)
            {
                WatchMode.Visibility = Visibility.Collapsed;
                MakeMeasureMode.Visibility = Visibility.Visible;
            }
        }

        private void SwitchToWatchMode_Checked(object sender, RoutedEventArgs e)
        {
            MakeMeasureMode.Visibility = Visibility.Collapsed;
            WatchMode.Visibility = Visibility.Visible;
        }

        private void LoadSettings()
        {
  

        }

        private void SaveSettings()
        {
            Properties.Settings.Default.ArticlesPanelWidth = MakeMeasureMode.column1.Width;
            Properties.Settings.Default.Save();
            Debug.WriteLine("Setting Saved");
            Debug.WriteLine("Width={0}",MakeMeasureMode.column1.Width);

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveSettings();
            base.OnClosed(e);
        }
    }
}
