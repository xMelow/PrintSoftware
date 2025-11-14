using PrintSoftware.Domain.Label;
using PrintSoftware.Services;

namespace Tests
{
    [TestClass]
    public sealed class LabelServiceTest
    {
        private Label testLabel;
        private LabelService labelService;

        [TestInitialize]
        public void Initialize()
        {
            testLabel = new Label("test label", 110, 110, "mm");
            labelService = new LabelService();
        }
        
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
