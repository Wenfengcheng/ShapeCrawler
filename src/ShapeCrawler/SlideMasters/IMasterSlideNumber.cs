﻿using DocumentFormat.OpenXml.Packaging;
using ShapeCrawler.Positions;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;

#pragma warning disable IDE0130
namespace ShapeCrawler;
#pragma warning restore IDE0130

/// <summary>
///     Represents a slide number.
/// </summary>
public interface IMasterSlideNumber : IPosition
{
    /// <summary>
    ///     Gets font.
    /// </summary>
    ISlideNumberFont Font { get; }
}

internal sealed class MasterSlideNumber : IMasterSlideNumber
{
    private readonly Position position;

    internal MasterSlideNumber(OpenXmlPart sdkTypedOpenXmlPart, P.Shape sdkPShape)
        : this(sdkPShape, new Position(sdkTypedOpenXmlPart, sdkPShape))
    {
    }

    private MasterSlideNumber(P.Shape sdkPShape, Position position)
    {
        this.position = position;
        var aDefaultRunProperties =
            sdkPShape.TextBody!.ListStyle!.Level1ParagraphProperties?.GetFirstChild<A.DefaultRunProperties>() !;
        this.Font = new SlideNumberFont(aDefaultRunProperties);
    }

    public ISlideNumberFont Font { get; }

    public decimal X
    {
        get => this.position.X();
        set => this.position.UpdateX(value);
    }

    public decimal Y
    {
        get => this.position.Y();
        set => this.position.UpdateY(value);
    }
}