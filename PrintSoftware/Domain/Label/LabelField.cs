using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PrintSoftware.Domain.Label;

public class LabelField : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string PlaceHolder { get; set; }
    
    private string _content;
    public string Content
    {
        get => _content;
        set
        {
            if (_content != value)
            {
                _content = value;
                OnPropertyChanged();
            }
        }
    }
    
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