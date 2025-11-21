using System.Windows.Media.Imaging;
using PrintSoftware.Domain.Label;

namespace PrintSoftware.Interfaces;

public interface ILabelPreviewController
{
    RenderTargetBitmap CreateLabelPreview(Label label);
    BitmapSource RenderDynamicElements();
    BitmapSource RenderDynamicElement(string fieldTag);
    BitmapSource RenderStaticElements();
}