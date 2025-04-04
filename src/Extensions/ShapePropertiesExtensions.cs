﻿using DocumentFormat.OpenXml;
using A = DocumentFormat.OpenXml.Drawing;

namespace ShapeCrawler.Extensions;

internal static class ShapePropertiesExtensions
{
    internal static void AddNoFill(this OpenXmlCompositeElement pShapeProperties)
    {
        pShapeProperties.GetFirstChild<A.GradientFill>()?.Remove();
        pShapeProperties.GetFirstChild<A.SolidFill>()?.Remove();
        pShapeProperties.GetFirstChild<A.PatternFill>()?.Remove();
        pShapeProperties.GetFirstChild<A.NoFill>()?.Remove();
        pShapeProperties.GetFirstChild<A.BlipFill>()?.Remove();

        var aNoFill = pShapeProperties.GetFirstChild<A.NoFill>();
        if (aNoFill != null)
        {
            foreach (var child in aNoFill)
            {
                child.Remove();
            }
        }
        else
        {
            aNoFill = new A.NoFill();
            var aOutline = pShapeProperties.GetFirstChild<A.Outline>();
            if (aOutline != null)
            {
                pShapeProperties.InsertAfter(aNoFill, aOutline);
            }
            else
            {
                pShapeProperties.Append(aNoFill);
            }
        }
    }

    internal static void AddSolidFill(this OpenXmlCompositeElement pShapeProperties, string hex)
    {
        pShapeProperties.GetFirstChild<A.GradientFill>()?.Remove();
        pShapeProperties.GetFirstChild<A.PatternFill>()?.Remove();
        pShapeProperties.GetFirstChild<A.NoFill>()?.Remove();
        pShapeProperties.GetFirstChild<A.BlipFill>()?.Remove();

        var aSolidFill = pShapeProperties.GetFirstChild<A.SolidFill>();
        if (aSolidFill != null)
        {
            foreach (var child in aSolidFill)
            {
                child.Remove();
            }
        }
        else
        {
            aSolidFill = new A.SolidFill();
            var aOutline = pShapeProperties.GetFirstChild<A.Outline>();
            if (aOutline != null)
            {
                pShapeProperties.InsertBefore(aSolidFill, aOutline);
            }
            else
            {
                pShapeProperties.Append(aSolidFill);
            }
        }

        var aRgbColorModelHex = new A.RgbColorModelHex { Val = hex };

        aSolidFill.Append(aRgbColorModelHex);
    }

    internal static A.Outline AddOutline(this OpenXmlCompositeElement pSpPr)
    {
        var aOutline = pSpPr.GetFirstChild<A.Outline>();
        aOutline?.Remove();

        aOutline = new A.Outline(
            new A.SolidFill(
                new A.SchemeColor { Val = new EnumValue<A.SchemeColorValues>(A.SchemeColorValues.Text1) }));
        pSpPr.Append(aOutline);

        return aOutline;
    }
}