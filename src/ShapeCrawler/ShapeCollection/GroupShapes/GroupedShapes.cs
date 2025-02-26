﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using ShapeCrawler.Texts;
using P = DocumentFormat.OpenXml.Presentation;
using Picture = ShapeCrawler.Drawing.Picture;

namespace ShapeCrawler.ShapeCollection.GroupShapes;

internal sealed record GroupedShapes : IShapes
{
    private readonly OpenXmlPart sdkTypedOpenXmlPart;
    private readonly IEnumerable<OpenXmlCompositeElement> pGroupElements;

    internal GroupedShapes(
        OpenXmlPart sdkTypedOpenXmlPart,
        IEnumerable<OpenXmlCompositeElement> pGroupElements)
    {
        this.sdkTypedOpenXmlPart = sdkTypedOpenXmlPart;
        this.pGroupElements = pGroupElements;
    }

    public int Count => this.GroupedShapesCore().Count;

    public IShape this[int index] => this.GroupedShapesCore()[index];

    public T GetById<T>(int id)
        where T : IShape => (T)this.GroupedShapesCore().First(shape => shape.Id == id);

    public T? TryGetById<T>(int id) 
        where T : IShape => (T?)this.GroupedShapesCore().FirstOrDefault(shape => shape.Id == id);

    T IShapes.GetByName<T>(string name) => (T)this.GroupedShapesCore().First(shape => shape.Name == name);

    T? IShapes.TryGetByName<T>(string name) 
        where T : default => (T?)this.GroupedShapesCore().FirstOrDefault(shape => shape.Name == name);

    public IShape GetByName(string name) => this.GroupedShapesCore().First(shape => shape.Name == name);
    public T Last<T>() 
        where T : IShape => (T)this.GroupedShapesCore().Last(shape => shape is T);

    public T GetByName<T>(string name) => (T)this.GroupedShapesCore().First(shape => shape.Name == name);

    public IEnumerator<IShape> GetEnumerator() => this.GroupedShapesCore().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    private List<IShape> GroupedShapesCore()
    {
        var groupedShapes = new List<IShape>();
        foreach (var pGroupShapeElement in this.pGroupElements)
        {
            IShape? shape = null;
            if (pGroupShapeElement is P.GroupShape pGroupShape)
            {
                shape = new GroupShape(this.sdkTypedOpenXmlPart, pGroupShape);
            }
            else if (pGroupShapeElement is P.Shape pShape)
            {
                if (pShape.TextBody is not null)
                {
                    shape = new GroupedShape(
                        pShape,
                        new AutoShape(
                            this.sdkTypedOpenXmlPart,
                            pShape,
                            new TextBox(this.sdkTypedOpenXmlPart, pShape.TextBody)));
                }
                else
                {
                    shape = new GroupedShape(
                        pShape,
                        new AutoShape(this.sdkTypedOpenXmlPart, pShape));
                }
            }
            else if (pGroupShapeElement is P.Picture pPicture)
            {
                var aBlip = pPicture.GetFirstChild<P.BlipFill>()?.Blip;
                var blipEmbed = aBlip?.Embed;
                if (blipEmbed is not null)
                {
                    shape = new Picture(this.sdkTypedOpenXmlPart, pPicture, aBlip!);
                }
            }

            if (shape != null)
            {
                groupedShapes.Add(shape);
            }
        }

        return groupedShapes;
    }
}