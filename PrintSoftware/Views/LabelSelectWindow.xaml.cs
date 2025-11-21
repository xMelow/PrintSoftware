using System;
using System.IO;
using System.Windows;
using PrintSoftware.Controller;
using PrintSoftware.Services;
using PrintSoftware.ViewModels;

namespace PrintSoftware.Views;

public partial class LabelSelectWindow : Window
{
    public string? SelectedLabelName { get; private set; }

    public LabelSelectWindow(LabelSelectViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;

        // Subscribe to the event
        vm.LabelSelected += label =>
        {
            if (!string.IsNullOrEmpty(label))
            {
                SelectedLabelName = label;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a label."); // View handles UI
            }
        };
    }
}