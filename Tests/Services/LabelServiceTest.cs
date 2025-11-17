using PrintSoftware.Domain.Label;
using PrintSoftware.Domain.Label.LabelElements;
using PrintSoftware.Services;

namespace Tests
{
    [TestClass]
    public sealed class LabelServiceTest
    {
        private Label _testLabel;
        private LabelService _labelService;

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
            _labelService = new LabelService();
        }

        [TestMethod]
        public void UpdateLabelElementDataTest()
        {
            _labelService.UpdateLabelDataElement("title", "This is the title");
            
            var textElement = _testLabel.LabelElements
                .OfType<TextElement>()
                .FirstOrDefault(e => e.Name == "title");
            
            Assert.AreEqual("This is the title", textElement.Content);
        }
    }
}
