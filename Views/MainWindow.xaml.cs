using Microsoft.Win32;
using SimpleProject.Controller;
using SimpleProject.Domain;
using SimpleProject.Domain.Labels;
using SimpleProject.Services;
using System.Data;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PrintController _printController;
        private readonly LabelController _labelController;
        public DataTable? ExcelData { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _printController = new PrintController();
            _labelController = new LabelController();
            UpdateLabelPreview();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            int amount = Int32.Parse(AmountTextBox.Text);
            var label = _labelController.GetLabel();
            _labelController.Printlabel(label, amount);
        }

        private void PrintAllButton_Click(object sender, RoutedEventArgs e)
        {
            int amount = Int32.Parse(AmountTextBox.Text);

            if (ExcelGrid.ItemsSource is DataView dataView)
            {
                foreach (DataRow row in dataView.Table.Rows)
                {
                    var label = _labelController.CreateLabelFromRow(row);
                    _labelController.Printlabel(label, amount);
                }
            }
        }

        private void ViewSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            var SettingsWindow = new SettingsWindow(_printController);
            SettingsWindow.Show();
        }

        private void Input_TextChanged(object sender, RoutedEventArgs e)
        {
            UpdateLabelPreview();
        }
        
        private void UpdateLabelPreview()
        {
            Dictionary<string, string> labelData = new Dictionary<string, string>
            {
                { "Title", TitleTextBox.Text },
                { "Name", NameTextBox.Text },
                { "PhoneNumber", PhoneNumberTextBox.Text },
                { "Email", EmailTextBox.Text },
                { "Company", CompanyTextBox.Text },
                { "QR", QRTextBox.Text }
            };
            _labelController.UpdateLabelWithData(labelData);
            LabelPreviewImage.Source = _labelController.GetPreview();
        }

        public void ImportExcelFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var service = new ExcelImportService();
                var table = service.ImportExcel(openFileDialog.FileName);

                ExcelGrid.ItemsSource = table.DefaultView;
            }
        }

        private void ExcelGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExcelGrid.SelectedItem is DataRowView selectedRow)
            {
                var row = selectedRow.Row;
                _labelController.CreateLabelFromRow(row);
            }
        }
    }
}