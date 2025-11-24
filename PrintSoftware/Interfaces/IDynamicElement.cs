namespace PrintSoftware.Interfaces;

public interface IDynamicElement
{
    string Name { get; set; }
    string Content { get; set; }
    void UpdateContent(string newContent);
}