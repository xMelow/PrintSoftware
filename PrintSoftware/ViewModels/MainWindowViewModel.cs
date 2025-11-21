using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Input;
using System.Windows.Media;
using PrintSoftware.Controller;
using PrintSoftware.Interfaces;
using PrintSoftware.ViewModels;
using PrintSoftware.ViewModels.Commands;
using PrintController = PrintSoftware.Controller.PrintController;

public class MainWindowViewModel : BaseViewModel
{
    private readonly PrintController _printController;
    private readonly LabelController _labelController;
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

    private string _selectedLabel;
    public string SelectedLabel
    {
        get => _selectedLabel;
        set
        {
            if (_selectedLabel == value) return;
            _selectedLabel = value;
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

    public MainWindowViewModel(
        PrintController printController,
        LabelController labelController,
        LabelPreviewController previewController,
        ExcelImportController excelController,
        IWindowService windowService)
    {
        _printController = printController;
        _labelController = labelController;
        _previewController = previewController;
        _excelImportController = excelController;
        _windowService = windowService;

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
        SelectedLabel = "TestLabel";

    private void LoadInstalledPrinters()
    {
        foreach (string printer in PrinterSettings.InstalledPrinters)
            Printers.Add(printer);

        SelectedPrinter = new PrinterSettings().PrinterName;
    }

    private void PrintCurrentLabel()
    {
        var label = _labelController.GetLabel();
        _printController.Printlabel(label, Amount);
    }

    private void PrintExcelBatch()
    {
        if (!HasExcelData()) return;

        foreach (DataRow row in GetExcelRows())
        {
            var label = _labelController.UpdateLabelDataFromRow(row);
            _printController.Printlabel(label, Amount);
        }
    }

    private bool HasExcelData() =>
        ExcelData?.DefaultView?.Table?.Rows?.Count > 0;

    private IEnumerable<DataRow> GetExcelRows() =>
        ExcelData.DefaultView.Table.Rows.Cast<DataRow>();

    private void SelectLabel()
    {
        var result = _windowService.ShowLabelSelectDialog();
        if (result != null)
            SelectedLabel = result;
    }

    private void ImportExcelFile()
    {
        var path = _windowService.ShowOpenExcelDialog();
        if (path != null)
            ExcelData = _excelImportController.ImportExcel(path);
    }

    private void OpenSettings() =>
        _windowService.ShowSettingsDialog();

    private void RefreshLabelPreview()
    {
        if (string.IsNullOrWhiteSpace(SelectedLabel)) return;

        _labelController.GetJsonLabel(SelectedLabel);
        _previewController.SetLabel(_labelController.GetLabel());
        _previewController.RenderStaticElements();

        LabelPreviewImage = _previewController.CreateLabelPreview();
    }
}
