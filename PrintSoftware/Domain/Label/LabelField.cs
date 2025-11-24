using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrintSoftware.Domain.Label;

public class LabelField : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string PlaceHolder { get; set; }
    public string Content { get; set; }

    public LabelField(string name, string placeHolder, string content)
    {
        Name = name;
        PlaceHolder = placeHolder;
        Content = content;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}