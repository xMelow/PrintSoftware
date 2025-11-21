using PrintSoftware.Controller;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using PrintSoftware.Interfaces;
using PrintSoftware.ViewModels.Commands;

namespace PrintSoftware.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IPrintController _printController;
        private readonly PrinterController _printerController;
        public ObservableCollection<string> Printers { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand GetPrintersCommand { get; }
        
        private string _density;
        public string Density
        {
            get => _density;
            set { _density = value; OnPropertyChanged(); }
        }

        private string _speed;
        public string Speed
        {
            get => _speed;
            set { _speed = value; OnPropertyChanged(); }
        }

        private string _direction;
        public string Direction
        {
            get => _direction;
            set { _direction = value; OnPropertyChanged(); }
        }

        private string _gap;
        public string Gap
        {
            get => _gap;
            set { _gap = value; OnPropertyChanged(); }
        }

        private bool _cutterEnabled;
        public bool CutterEnabled
        {
            get => _cutterEnabled;
            set { _cutterEnabled = value; OnPropertyChanged(); }
        }

        private string _offset;
        public string Offset
        {
            get => _offset;
            set { _offset = value; OnPropertyChanged(); }
        }

        private bool _tearEnabled;
        public bool TearEnabled
        {
            get => _tearEnabled;
            set { _tearEnabled = value; OnPropertyChanged(); }
        }

        public SettingsViewModel(
            IPrintController printController
        ) {
            _printController = printController;
            _printerController = new PrinterController();

            Printers = new ObservableCollection<string>();
            
            LoadDefaultSettings();

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            GetPrintersCommand = new RelayCommand(async () => await _printerController.DiscoverPrintersOnNetwork());
        }
        
        private void LoadDefaultSettings()
        {
            var defaultSettings = _printController.GetPrinterSettings();

            Density = defaultSettings["Density"];
            Speed = defaultSettings["Speed"];
            Direction = defaultSettings["Direction"];
            Gap = defaultSettings["Gap"];
            CutterEnabled = defaultSettings["Cutter"] == "ON";
            Offset = defaultSettings["Offset"];
            TearEnabled = defaultSettings["Tear"] == "ON";
        }

        private void SaveSettings()
        {
            var settings = new Dictionary<string, string>
            {
                { "Density", Density },
                { "Speed", Speed },
                { "Direction", Direction == "Default" ? "0" : "1" },
                { "Gap", Gap },
                { "Cutter", CutterEnabled ? "ON" : "OFF" },
                { "Offset", Offset },
                { "Tear", TearEnabled ? "ON" : "OFF" }
            };

            _printController.SetPrinterSettings(settings);
        }
    }
}
