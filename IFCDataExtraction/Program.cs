using System;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.SharedBldgElements;
using System.Linq;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.ProfileResource;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MaterialResource;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.ProductExtension;

namespace IFCDataExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"..\..\..\IFC_File\";
            string fileName = "CMC02(ifc4)";
            //open STEP file
            using (var stepModel = IfcStore.Open($"{filePath}{fileName}.ifc"))
            {
                //save as XML
                //stepModel.SaveAs($"{filePath}{fileName}.ifcxml");

                var product = (stepModel.Instances.OfType<IfcColumnStandardCase>().LastOrDefault().Representation.Representations.FirstOrDefault().Items.FirstOrDefault() as IfcExtrudedAreaSolid).SweptArea as IfcIShapeProfileDef;
                var FlangeThickness = product.FlangeThickness;
                var WebThickness = product.WebThickness;
                var X = product.Position.Location.X;
                var y = product.Position.Location.Y;
            }
        }
    }
}

