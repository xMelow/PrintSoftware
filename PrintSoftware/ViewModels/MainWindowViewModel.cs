using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Input;
using System.Windows.Media;
using PrintSoftware.Controller;
using PrintSoftware.Domain.Label;
using PrintSoftware.Interfaces;
using PrintSoftware.Services;
using PrintSoftware.ViewModels;
using PrintSoftware.ViewModels.Commands;
using PrintController = PrintSoftware.Controller.PrintController;

namespace PrintSoftware.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly PrintController _printController;
    private readonly ILabelController _labelController;
    private readonly LabelPreviewController _previewController;
    private readonly ExcelImportController _excelImportController;
    private readonly IWindowService _windowService;

    public ObservableCollection<string> Printers { get; } = new();
    public ObservableCollection<string> Labels { get; } = new();

    public ICommand PrintCommand { get; }
    public ICommand PrintBatchCommand { get; }
    public ICommand SelectLabelCommand { get; }
    public ICommand ImportExcelCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    private Label _label;
    public Label Label
    {
        get => _label;
        set
        {
            if (_label == value) return;
            _label = value;
            OnPropertyChanged();
            RefreshLabelPreview();
        }
    }
    
    private string _selectedPrinter;
    public string SelectedPrinter
    {
        get => _selectedPrinter;
        set
        {
            _selectedPrinter = value; OnPropertyChanged();
        }
    }

    private ImageSource _labelPreviewImage;
    public ImageSource LabelPreviewImage
    {
        get => _labelPreviewImage;
        set { _labelPreviewImage = value; OnPropertyChanged(); }
    }

    public int Amount { get; set; } = 1;

    private DataTable _excelData;
    public DataTable ExcelData
    {
        get => _excelData;
        set { _excelData = value; OnPropertyChanged(); }
    }

    public MainWindowViewModel()
    {
        _printController = new PrintController();
        _previewController = new  LabelPreviewController();
        _excelImportController = new  ExcelImportController();
        _windowService = new WindowService();

        PrintCommand = new RelayCommand(PrintCurrentLabel);
        PrintBatchCommand = new RelayCommand(PrintExcelBatch);
        SelectLabelCommand = new RelayCommand(SelectLabel);
        ImportExcelCommand = new RelayCommand(ImportExcelFile);
        OpenSettingsCommand = new RelayCommand(OpenSettings);

        Initialize();
    }

    private void Initialize()
    {
        LoadInstalledPrinters();
        InitializeDefaultLabel();
    }

    private void InitializeDefaultLabel() =>
        Label = _labelController.GetLabel("TestLabel");

    private void LoadInstalledPrinters()
    {
        foreach (string printer in PrinterSettings.InstalledPrinters)
            Printers.Add(printer);

        SelectedPrinter = new PrinterSettings().PrinterName;
    }

    private void PrintCurrentLabel()
    {
        var label = _labelController.GetCurrentLabel();
        _printController.Printlabel(label, Amount);
    }

    private void PrintExcelBatch()
    {
        if (!HasExcelData()) return;

        foreach (DataRow row in GetExcelRows())
        {
            _labelController.UpdateLabelDataFromRow(row);
            _printController.Printlabel(_label, Amount);
        }
    }

    private bool HasExcelData() =>
        ExcelData?.DefaultView?.Table?.Rows?.Count > 0;

    private IEnumerable<DataRow> GetExcelRows() =>
        ExcelData.DefaultView.Table.Rows.Cast<DataRow>();

    private void SelectLabel()
    {
        var selectedLabel = _windowService.ShowLabelSelectScreen();
        if (selectedLabel != null)
            Label = selectedLabel;
    }

    private void ImportExcelFile()
    {
        var path = _windowService.OpenExcelDialog();
        if (path != null)
            ExcelData = _excelImportController.ImportExcel(path);
    }

    private void OpenSettings() =>
        _windowService.ShowSettingsScreen();

    private void RefreshLabelPreview()
    {
        _previewController.RenderStaticElements();
        LabelPreviewImage = _previewController.CreateLabelPreview();
    }
}
