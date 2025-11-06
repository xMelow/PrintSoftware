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
using Wpf.Ui.Controls;
using System.Drawing.Printing;



namespace SimpleProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        private readonly Controller.PrintController _printController;
        private readonly LabelController _labelController;
        private readonly LabelPreviewController _labelPreviewController;
        public DataTable? ExcelData { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _printController = new Controller.PrintController();
            _labelController = new LabelController();
            _labelPreviewController = new LabelPreviewController(_labelController.GetLabel());

            LabelPreviewImage.Source = _labelPreviewController.RenderFullLabelPreview();
            PopulateData();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            int amount = Int32.Parse(AmountTextBox.Text);
            var label = _labelController.GetLabel();
            _printController.Printlabel(label, amount);
        }

        private void PrintAllButton_Click(object sender, RoutedEventArgs e)
        {
            int amount = Int32.Parse(AmountTextBox.Text);

            if (ExcelGrid.ItemsSource is DataView dataView)
            {
                foreach (DataRow row in dataView.Table.Rows)
                {
                    var label = _labelController.CreateLabelFromRow(row);
                    _printController.Printlabel(label, amount);
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
            Debug.WriteLine($"Sender: {sender}");
            UpdateLabelPreview();
        }
        
        private void UpdateLabelPreview()
        {
            //change to update only field with data

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
            _labelPreviewController.RenderDynamicLabelPreview();
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
                _labelPreviewController.RenderDynamicLabelPreview();
                SetLabelDataFields(row);
            }
        }

        private void SetLabelDataFields(DataRow row)
        {
            TitleTextBox.Text = row["ID"].ToString();
            NameTextBox.Text = row["NAME"].ToString();
            PhoneNumberTextBox.Text = row["PHONENUMBER"].ToString();
            EmailTextBox.Text = row["EMAIL"].ToString();
            CompanyTextBox.Text = row["OCCUPATION"].ToString();
            QRTextBox.Text = row["POSTCODE"].ToString();
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

        private void PopulateData()
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
                PrinterComboBox.Items.Add(printer);

            PrinterComboBox.SelectedItem = new PrinterSettings().PrinterName;

            AmountTextBox.Text = "1";
        }
    }
}