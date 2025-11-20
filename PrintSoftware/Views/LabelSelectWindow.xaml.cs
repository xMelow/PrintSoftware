using System;
using System.IO;
using System.Windows;
using PrintSoftware.Controller;
using PrintSoftware.Services;

namespace PrintSoftware.Views;

public partial class LabelSelectWindow : Window
{
    private readonly LabelController _labelController;
    public LabelSelectWindow(LabelController labelController)
    {
        InitializeComponent();
        
        _labelController = labelController;
        
        LoadLabels();
    }

    private void LoadLabels()
    {
        string labelsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Labels");
        
        if (!Directory.Exists(labelsFolder))
            MessageBox.Show("Labels folder does not exist");

        var labelFiles = Directory.GetFiles(labelsFolder)
            .Select(Path.GetFileName)
            .ToList();

        LabelList.ItemsSource = labelFiles;
    }
    
    private void OpenLabel_Click(object sender, RoutedEventArgs e)
    {
        if (LabelList.SelectedItem == null)
        {
            MessageBox.Show("Please select a label.");
            return;
        }

        string selectedFile = LabelList.SelectedItem.ToString();
        string filePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "labels",
            selectedFile);

        MessageBox.Show("Selected label: " + filePath);

         // _labelService.CurrentLabel = ;
    }

}