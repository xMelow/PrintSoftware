namespace PrintSoftware.Interfaces;

public interface IDynamicElement
{
    string Name { get; set; }
    
    void UpdateContent(string newContent);
}