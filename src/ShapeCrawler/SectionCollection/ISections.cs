﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using ShapeCrawler.Exceptions;
using ShapeCrawler.ShapeCollection;
using P14 = DocumentFormat.OpenXml.Office2010.PowerPoint;

#pragma warning disable IDE0130
namespace ShapeCrawler;
#pragma warning restore IDE0130

/// <summary>
///     Represents a collection of presentation sections.
/// </summary>
public interface ISections : IReadOnlyCollection<ISection>
{
    /// <summary>
    ///     Gets the section by index.
    /// </summary>
    ISection this[int index] { get; }

    /// <summary>
    ///     Removes specified section.
    /// </summary>
    void Remove(ISection removingSection);

    /// <summary>
    ///     Gets section by section name.
    /// </summary>
    ISection GetByName(string sectionName);
}

internal sealed class Sections : ISections
{
    private readonly PresentationDocument sdkPresDocument;

    internal Sections(PresentationDocument sdkPresDocument)
    {
        this.sdkPresDocument = sdkPresDocument;
    }

    public int Count => this.SectionList().Count;
    
    public ISection this[int index] => this.SectionList()[index];
    
    public IEnumerator<ISection> GetEnumerator() => this.SectionList().GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public void Remove(ISection removingSection)
    {
        if (removingSection is not IRemoveable removeable)
        {
            throw new SCException("Section cannot be removed.");
        }

        var total = this.Count;
        removeable.Remove();

        if (total == 1)
        {
            this.sdkPresDocument.PresentationPart!.Presentation.PresentationExtensionList
                ?.Descendants<P14.SectionList>().First()
                .Remove();
        }
    }

    public ISection GetByName(string sectionName) => this.SectionList().First(section => section.Name == sectionName);
    
    private List<Section> SectionList()
    {
        var p14SectionList = this.sdkPresDocument.PresentationPart!.Presentation.PresentationExtensionList
            ?.Descendants<P14.SectionList>().FirstOrDefault();
        return p14SectionList == null
            ? []
            : [.. p14SectionList.OfType<P14.Section>().Select(p14Section => new Section(this.sdkPresDocument, p14Section))];
    }
}