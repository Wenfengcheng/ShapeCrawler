using System.IO;
using FluentAssertions;
using NUnit.Framework;
using ShapeCrawler.Tests.Unit.Helpers;

namespace ShapeCrawler.Tests.Integration;

public class PresentationITests : SCTest
{
    [Test]
    public void Open_should_not_throw_exception()
    {
        // Arrange
        var originFilePath = Path.GetTempFileName();
        var savedAsFilePath = Path.GetTempFileName();
        var pptx = TestAsset("001.pptx");
        File.WriteAllBytes(originFilePath, pptx.ToArray());
        var pres = new Presentation(originFilePath);
        pres.SaveAs(savedAsFilePath);

        // Act-Assert
        new Presentation(originFilePath);
        
        // Clean up
        File.Delete(originFilePath);
        File.Delete(savedAsFilePath);
    }
    
    [Test]
    public void SaveAs_should_not_change_the_Original_Path_when_it_is_saved_to_New_Stream()
    {
        // Arrange
        var originalPath = GetTestPath("001.pptx");
        var pres = new Presentation(originalPath);
        var textBox = pres.Slides[0].Shapes.GetByName<IShape>("TextBox 3").TextBox;
        var originalText = textBox!.Text;
        var newStream = new MemoryStream();
        textBox.Text = originalText + "modified";

        // Act
        pres.SaveAs(newStream);

        // Assert
        pres = new Presentation(originalPath);
        textBox = pres.Slides[0].Shapes.GetByName<IShape>("TextBox 3").TextBox;
        var autoShapeText = textBox!.Text;
        autoShapeText.Should().BeEquivalentTo(originalText);
            
        // Clean
        File.Delete(originalPath);
    }
           
    [Test]
    public void SaveAs_should_not_change_the_Original_Stream_when_it_is_saved_to_New_Path()
    {
        // Arrange
        var originalFile = GetTestPath("001.pptx");
        var pres = new Presentation(originalFile);
        var textBox = pres.Slides[0].Shapes.GetByName<IShape>("TextBox 3").TextBox;
        var originalText = textBox!.Text;
        var newPath = Path.GetTempFileName();
        textBox.Text = originalText + "modified";

        // Act
        pres.SaveAs(newPath);

        // Assert
        pres = new Presentation(originalFile);
        textBox = pres.Slides[0].Shapes.GetByName<IShape>("TextBox 3").TextBox;
        var autoShapeText = textBox!.Text;
        autoShapeText.Should().BeEquivalentTo(originalText);
            
        // Clean
        File.Delete(newPath);
    }
}