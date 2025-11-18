using PrintSoftware.Domain.Label;
using PrintSoftware.Domain.Label.LabelElements;

namespace Tests.Domain;

[TestClass]
public class LabelTest
{
    private Label _label;

    [TestInitialize]
    public void TestInitialize()
    {
        var elements = new List<LabelElement>()
        {
            new TextElement("title", 427, 47, "Header", "0", 20),
            new QRCodeElement("qr", 110, 234, "this is the QR"),
            new CircleElement(250, 123, 10, 2),
            new BoxElement(233, 444, 555, 666, 2),
            new BarElement(943, 23, 1111, 2222),
            // image element
            // barcode element
        };
        _label = new Label("Test Label", 10, 10, "mm", elements);
    }

    [TestMethod]
    public void CreateLabelTsplTest()
    {
        string tspl =  _label.CreateLabelTspl();
        string expectedTspl = 
                           $"CLS" + "\n" +
                           $"TEXT 427,47,\"0\",0,20,20,\"Header\"" + "\n" +
                           $"QRCODE 110,234,L,5,A,0,M2,S7,\"this is the QR\"" + "\n" +
                           $"CIRCLE 250,123,10,2" + "\n" +
                           $"BOX 233,444,555,666,2" + "\n" +
                           $"BAR 943,23,1111,2222" + "\n";
        
        Assert.AreEqual(expectedTspl, tspl);
    }
}