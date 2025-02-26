using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;

#if DEBUG
using System.Threading.Tasks;
#endif

#pragma warning disable IDE0130
namespace ShapeCrawler;
#pragma warning restore IDE0130

/// <summary>
///     Represents a slide.
/// </summary>
public interface ISlide
{
    /// <summary>
    ///     Gets or sets custom data. It returns <see langword="null"/> if custom data is not presented.
    /// </summary>
    string? CustomData { get; set; }
    
    /// <summary>
    ///     Gets referenced Slide Layout.
    /// </summary>
    ISlideLayout SlideLayout { get; }
    
    /// <summary>
    ///     Gets or sets slide number.
    /// </summary>
    int Number { get; set; }
    
    /// <summary>
    ///     Gets underlying instance of <see cref="DocumentFormat.OpenXml.Packaging.SlidePart"/>.
    /// </summary>
    SlidePart SdkSlidePart { get; }
    
    /// <summary>
    ///     Gets the shape collection.
    /// </summary>
    ISlideShapes Shapes { get; }

    /// <summary>
    ///     Gets slide notes as a single text frame.
    /// </summary>
    ITextBox? Notes { get; }

    /// <summary>
    ///     Gets the fill of the slide.
    /// </summary>
    IShapeFill Fill { get; }
    
    /// <summary>
    ///     List of all text frames on that slide.
    /// </summary>
    public IList<ITextBox> TextFrames();

    /// <summary>
    ///     Hides slide.
    /// </summary>
    void Hide();
    
    /// <summary>
    ///     Gets a value indicating whether the slide is hidden.
    /// </summary>
    bool Hidden();
    
    /// <summary>
    ///     Gets table by name.
    /// </summary>
    ITable Table(string name);
    
    /// <summary>
    ///     Gets picture by name.
    /// </summary>
    IPicture Picture(string picture);
    
    /// <summary>
    ///     Adds specified lines to the slide notes.
    /// </summary>
    void AddNotes(IEnumerable<string> lines);
    
    /// <summary>
    ///     Returns shape with specified name.
    /// </summary>
    /// <param name="name">Shape name.</param>
    /// <returns> An instance of <see cref="IShape"/>.</returns>
    IShape Shape(string name);

    /// <summary>
    ///     Returns shape with specified name.
    /// </summary>
    /// <typeparam name="T">Shape type.</typeparam>
    IShape Shape<T>(string name)
        where T : IShape;
}