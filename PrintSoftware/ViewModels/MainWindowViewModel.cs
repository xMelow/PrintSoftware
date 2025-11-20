using PrintSoftware.Controller;
using PrintSoftware.Domain.Label;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PrintController = PrintSoftware.Controller.PrintController;

namespace PrintSoftware.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly PrintController _printController;
        private readonly LabelController _labelController;
        private readonly LabelPreviewController _labelPreviewController;

        private string _selectedLabel;
        private string SelectedLabel
        {
            get => _selectedLabel;
            set
            {
                _selectedLabel = value;
                OnPropertyChanged();
                UpdateLabelPreview(_selectedLabel);
            }
        }

        private ImageSource _labelPreviewImage;
        public ImageSource LabelPreviewImage
        {
            get => _labelPreviewImage;
            set { _labelPreviewImage = value; OnPropertyChanged(); }
        }

        private int _amount = 1;
        public int Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }

        private string _selectedPrinter;
        public string SelectedPrinter
        {
            get => _selectedPrinter;
            set { _selectedPrinter = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Printers { get; set; } = new();
        public ObservableCollection<string> Labels { get; set; } = new();

        private DataTable _excelData;
        public DataTable ExcelData
        {
            get => _excelData;
            set { _excelData = value; OnPropertyChanged(); }
        }

        public ICommand PrintCommand { get; }
        public ICommand PrintAllCommand { get; }
        public ICommand SelectLabelCommand { get; }
        public ICommand ImportExcelCommand { get; }

        public MainWindowViewModel()
        {
            _printController = new PrintController();
            _labelController = new LabelController();
            _labelPreviewController = new LabelPreviewController();

            LoadPrinters();
            LoadDefaultLabel();

            PrintCommand = new RelayCommand(Print);
            PrintAllCommand = new RelayCommand(PrintAll);
            SelectLabelCommand = new RelayCommand(SelectLabel);
            ImportExcelCommand = new RelayCommand(ImportExcel);
        }

        private void LoadPrinters()
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
                Printers.Add(printer);

            SelectedPrinter = new PrinterSettings().PrinterName;
        }

        private void LoadDefaultLabel()
        {
            SelectedLabel = "TestLabel";
        }

        private void UpdateLabelPreview(string labelName)
        {
            if (string.IsNullOrEmpty(labelName)) return;

            _labelController.GetJsonLabel(labelName);
            _labelPreviewController.SetLabel(_labelController.GetLabel());

            var preview = _labelPreviewController.CreateLabelPreview();
            _labelPreviewController.RenderStaticElements();

            LabelPreviewImage = preview;
        }

        public void UpdateLabelData(string name, string data)
        {
            _labelController.UpdateLabelData(name, data);
            _labelPreviewController.RenderDynamicElement(name);

            UpdateLabelPreview(SelectedLabel);
        }

        private void Print()
        {
            var label = _labelController.GetLabel();
            _printController.Printlabel(label, Amount);
        }

        private void PrintAll()
        {
            if (ExcelData?.DefaultView == null) return;

            foreach (DataRow row in ExcelData.DefaultView.Table.Rows)
            {
                var label = _labelController.UpdateLabelDataFromRow(row);
                _printController.Printlabel(label, Amount);
            }
        }

        private void SelectLabel()
        {
            var labelWindow = new Views.LabelSelectWindow(_labelController);
            bool? result = labelWindow.ShowDialog();

            if (result == true)
            {
                SelectedLabel = labelWindow.SelectedLabelName;
            }
        }

        private void ImportExcel()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var service = new ExcelImportController();
                ExcelData = service.ImportExcel(openFileDialog.FileName);
            }
        }
    }
}
