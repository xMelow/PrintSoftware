using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using PrintSoftware.Controller;
using PrintSoftware.Domain.Label;
using PrintSoftware.Interfaces;
using PrintSoftware.Services;
using PrintSoftware.ViewModels;
using PrintSoftware.ViewModels.Commands;
using Label = PrintSoftware.Domain.Label.Label;

namespace PrintSoftware.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly IPrintController _printController;
    private readonly ILabelController _labelController;
    private readonly ILabelPreviewController _previewController;
    private readonly IExcelImportService _excelImportController;
    private readonly IWindowService _windowService;

    public ObservableCollection<string> Printers { get; } = new();
    
    public ICommand PrintCommand { get; }
    public ICommand PrintAllCommand { get; }
    public ICommand SelectLabelCommand { get; }
    public ICommand ImportExcelCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    
    public ICommand SelectExcelRowCommand { get; }
    
    private ObservableCollection<LabelField> _currentLabelFields = new();
    public ObservableCollection<LabelField> CurrentLabelFields
    {
        get => _currentLabelFields;
        set
        {
            _currentLabelFields = value;
            OnPropertyChanged();
        }
    }

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
            _selectedPrinter = value; 
            OnPropertyChanged();
        }
    }

    private ImageSource _labelPreviewImage;
    public ImageSource LabelPreviewImage
    {
        get => _labelPreviewImage;
        set { 
            _labelPreviewImage = value; 
            OnPropertyChanged(nameof(LabelPreviewImage)); 
        }
    }

    public int Amount { get; set; } = 1;

    private DataTable _excelData;
    public DataTable ExcelData
    {
        get => _excelData;
        set { _excelData = value; OnPropertyChanged(); }
    }

    public MainWindowViewModel(
        IPrintController printController,
        ILabelController labelController,
        ILabelPreviewController previewController,
        IExcelImportService excelImportService,
        IWindowService windowService
    ) {
        _printController = printController;
        _labelController = labelController;
        _previewController = previewController;
        _excelImportController = excelImportService;
        _windowService = windowService;

        PrintCommand = new RelayCommand(PrintCurrentLabel);
        PrintAllCommand = new RelayCommand(PrintBatchCommand);
        SelectLabelCommand = new RelayCommand(SelectLabel);
        ImportExcelCommand = new RelayCommand(ImportExcelFile);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        SelectExcelRowCommand = new RelayCommand(SelectExcelRow);

        Initialize();
    }

    private void Initialize()
    {
        LoadInstalledPrinters();
        Label = _labelController.GetLabel("Testlabel");
        RenderSelectedLabelFields();
    }

    private void LoadInstalledPrinters()
    {
        foreach (string printer in PrinterSettings.InstalledPrinters)
            Printers.Add(printer);

        SelectedPrinter = new PrinterSettings().PrinterName;
    }

    private void RenderSelectedLabelFields()
    {
        CurrentLabelFields = new ObservableCollection<LabelField>(_labelController.GetLabelFields());
        OnPropertyChanged(nameof(CurrentLabelFields));

        foreach (var field in CurrentLabelFields)
        {
            field.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LabelField.Content))
                    RefreshDynamicLabelPreviewElement(field.Name, field.Content);
            };
        }
    }
    
    private void PrintCurrentLabel()
    {
        var label = _labelController.GetCurrentLabel();
        _printController.PrintLabel(label, Amount);
    }

    private void PrintBatchCommand()
    {
        if (!HasExcelData()) return;

        foreach (DataRow row in GetExcelRows())
        {
            _labelController.UpdateLabelDataFromRow(row);
            _printController.PrintLabel(_label, Amount);
        }
    }

    private bool HasExcelData() =>
        ExcelData?.DefaultView?.Table?.Rows?.Count > 0;

    private IEnumerable<DataRow> GetExcelRows() =>
        ExcelData.DefaultView.Table.Rows.Cast<DataRow>();

    private void SelectExcelRow()
    {
        Console.WriteLine("selectExcelRow");
        Console.WriteLine($"Datarow: {ExcelData.DefaultView.Table.Rows[0]}");
        // update label data
        _labelController.UpdateLabelDataFromRow(ExcelData.DefaultView.Table.Rows[0]);
        // update label preview
        _previewController.RenderDynamicElements();
    }

    private void SelectLabel()
    {
        var selectedLabel = _windowService.ShowLabelSelectScreen();
        if (selectedLabel != null)
            Label = selectedLabel;
        
        RenderSelectedLabelFields();
    }

    private void ImportExcelFile()
    {
        var path = _windowService.OpenExcelDialog();
        if (path != null)
            ExcelData = _excelImportController.ImportExcelFile(path);
    }

    private void OpenSettings() =>
        _windowService.ShowSettingsScreen();

    private void RefreshLabelPreview()
    {
        if (Label == null)
        {
            LabelPreviewImage = null;
            return;
        }

        _previewController.CreateLabelPreview(Label);
        LabelPreviewImage = _previewController.RenderStaticElements();
        LabelPreviewImage = _previewController.RenderDynamicElements();
    }

    private void RefreshDynamicLabelPreviewElement(string name, string content)
    {
        _labelController.UpdateLabelElementData(name, content);
        LabelPreviewImage = _previewController.RenderDynamicElement(name);
    }
}
