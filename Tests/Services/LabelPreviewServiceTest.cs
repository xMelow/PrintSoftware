using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrintSoftware.Domain.Label;
using PrintSoftware.Domain.Label.LabelElements;
using PrintSoftware.Services;

namespace Tests.Services;

[TestClass]
[TestSubject(typeof(LabelPreviewService))]
public class LabelPreviewServiceTest
{
    private Label _testLabel;
    private LabelPreviewService _labelPreviewService;

    [TestInitialize]
    public void TestInitialize()
    {
        _testLabel = new Label("test label", 110, 110, "mm");
        _testLabel.LabelElements = new List<LabelElement>()
        {
            new TextElement("title", 427, 47, "", "0", 20),
            new TextElement(77, 234, "0", 0, 12, "Name: "), 
            new TextElement("name", 295, 231, "Flor Stellamans", "0", 16),
            new CircleElement(933, 590, 260, 12), 
            new BoxElement(33, 178, 1199, 7, 6)
        };
        _labelPreviewService = new LabelPreviewService(_testLabel);
    }

    [TestMethod]
    public void RenderStaticLabelPreview()
    {
        var result = _labelPreviewService.RenderStaticLabelPreview();
        Assert.IsNotNull(result);
    }
    
    [TestMethod]
    public void RenderDynamicLabelPreview()
    {
        var result = _labelPreviewService.RenderDynamicLabelElements();
        Assert.IsNotNull(result);
    }
}