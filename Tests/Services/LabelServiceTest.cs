using PrintSoftware.Domain.Label;
using PrintSoftware.Domain.Label.LabelElements;
using PrintSoftware.Interfaces;
using PrintSoftware.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Services
{
    [TestClass]
    public sealed class LabelServiceTest
    {
        private LabelService _labelService;

        [TestInitialize]
        public void TestInitialize()
        {
            Label label = new Label("test label", 110, 110, "mm");
            label.LabelElements = new List<LabelElement>()
            {
                new TextElement("title", 427, 47, "", "0", 20),
                new TextElement(77, 234, "0", 0, 12, "Name: "), 
                new TextElement("name", 295, 231, "Flor Stellamans", "0", 16),
                new CircleElement(933, 590, 260, 12), 
                new BoxElement(33, 178, 1199, 7, 6)
            };
            _labelService = new LabelService();
            _labelService.CurrentLabel = label;
        }

        [TestMethod]
        public void CreateLabelTest()
        {
            _labelService.GetLabel("TestLabel");
            
            Assert.AreEqual("TestLabel", _labelService.CurrentLabel.Name);
            Assert.HasCount(13,  _labelService.CurrentLabel.LabelElements);
        }
        
        [TestMethod]
        public void CreateLabelNameNotFoundTest()
        {
            _labelService.GetLabel("Label does not exist");
            
            Assert.AreEqual("Label does not exist", _labelService.CurrentLabel.Name);
            Assert.HasCount(0,  _labelService.CurrentLabel.LabelElements);
        }
        
        [TestMethod]
        public void UpdateLabelElementDataTest()
        {
            _labelService.UpdateLabelDataElement("title", "This is the title");
            
            var titleElement = _labelService.CurrentLabel.LabelElements
                .OfType<TextElement>()
                .FirstOrDefault(e => e.Name == "title");
            
            Assert.AreEqual("This is the title", titleElement.Content);
        }

        [TestMethod]
        public void UpdateLabelDataTest()
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "title", "This is the title 2" },
                { "name", "Rolf Snamallets" },
            };
            
            var titleElement = _labelService.CurrentLabel.LabelElements
                .OfType<TextElement>()
                .FirstOrDefault(e => e.Name == "title");
            
            var nameElement = _labelService.CurrentLabel.LabelElements
                .OfType<TextElement>()
                .FirstOrDefault(e => e.Name == "name");
            
            _labelService.UpdateLabelData(data);
            
            Assert.AreEqual("This is the title 2", titleElement.Content);
            Assert.AreEqual("Rolf Snamallets", nameElement.Content);
        }

        [TestMethod]
        public void UpdateLabelElementWithWrongNameTest()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                _labelService.UpdateLabelDataElement("name2", "this is invalid");
            });
            
            Assert.AreEqual("LabelElement: 'name2' not found", ex.Message);
        }
    }
}
