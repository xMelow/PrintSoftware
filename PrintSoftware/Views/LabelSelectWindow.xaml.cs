using System;
using System.IO;
using System.Windows;
using PrintSoftware.Controller;
using PrintSoftware.Services;

namespace PrintSoftware.Views;

public partial class LabelSelectWindow : Window
{
    private readonly LabelController _labelController;
    public string SelectedLabelName { get; private set; }
    public LabelSelectWindow(LabelController labelController)
    {
        InitializeComponent();
        
        _labelController = labelController;
        
        LoadLabels();
    }

    private void LoadLabels()
    {
        var labelsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Labels");
        
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

        var selectedFile = LabelList.SelectedItem.ToString();
        var labelName = selectedFile.Split(".")[0];
        SelectedLabelName = labelName;
        DialogResult = true;
    }

}