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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PrintSoftware.Controller;
using Wpf.Ui.Controls;

namespace PrintSoftware
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : FluentWindow
    {
        private readonly PrintController _printController;

        public SettingsWindow(PrintController printController)
        {
            _printController = printController;
            InitializeComponent();
            SetDefaultValues();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            string directionSetting = (DirectionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            bool isCutterEnalbed = CutterCheckBox.IsChecked == true;
            bool isTearEnalbed = TearCheckBox.IsChecked == true;

            Dictionary<string, string> printSettings = new Dictionary<string, string>
            {
                { "Density", DensityBox.Text },
                { "Speed", SpeedBox.Text },
                { "Direction", directionSetting == "Default" ? "0" : "1" },
                { "Gap", GapBox.Text },
                { "Cutter",  isCutterEnalbed ? "ON" : "OFF"},
                { "Offset", OffsetBox.Text },
                { "Tear", isTearEnalbed ? "ON" : "OFF" },

            };
            _printController.SetPrintSettings(printSettings);
            Close();
        }

        private void SetDefaultValues()
        {
            Dictionary<string, string> defaultSettings = _printController.GetPrinterSettings();

            SpeedBox.Text = defaultSettings["Speed"];
            DensityBox.Text = defaultSettings["Density"];
            DirectionComboBox.SelectedItem = defaultSettings["Direction"];
            GapBox.Text = defaultSettings["Gap"];
            CutterCheckBox.IsChecked = defaultSettings["Cutter"] == "ON";
            OffsetBox.Text = defaultSettings["Offset"];
            TearCheckBox.IsChecked = defaultSettings["Tear"] == "ON";
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
