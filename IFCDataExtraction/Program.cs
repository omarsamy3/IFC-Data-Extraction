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

                // var col = stepModel.Instances.OfType<IfcRoot>().Where(ins => ins.GlobalId == "0Loq$KqEX8kx0FfhUNoU4z").FirstOrDefault() as IfcColumnStandardCase;             
                //var col_material = ((col.HasAssociations.FirstOrDefault() as IfcRelAssociatesMaterial).RelatingMaterial as IfcMaterialProfileSetUsage).ForProfileSet;
                //var product_all = stepModel.Instances;
                //var product = (stepModel.Instances.OfType<IfcColumn>().FirstOrDefault().Representation.Representations.FirstOrDefault().Items.FirstOrDefault() as IfcExtrudedAreaSolid).SweptArea as IfcRectangleProfileDef;
                
                var product = (stepModel.Instances.OfType<IfcColumnStandardCase>().LastOrDefault().Representation.Representations.FirstOrDefault().Items.FirstOrDefault() as IfcExtrudedAreaSolid).SweptArea as IfcIShapeProfileDef;
                var FlangeThickness = product.FlangeThickness;
                var WebThickness = product.WebThickness;
                var X = product.Position.Location.X;
                var y = product.Position.Location.Y;
                
                //var xDim = product.XDim;
                //var yDim = product.YDim;

                //Console.WriteLine($"XDim={xDim}");
                //Console.WriteLine($"YDim={yDim}");

                //var product = stepModel.Instances.OfType<IfcMaterial>().Where(mt => mt.Name == "4000Psi").FirstOrDefault().HasProperties.FirstOrDefault().Properties;


                //open transaction for changes
                // using (var txn = stepModel.BeginTransaction("Doors modification"))
                // {

                //(product.Where(pr => pr.Name == "MassDensity").FirstOrDefault() as IfcPropertySingleValue).NominalValue = new IfcMassDensityMeasure(50000);
                //product.XDim = 600;
                //product.YDim = 600;
                //product.ProfileName = "600x600";
                //commit changes
                //txn.Commit();
                //}

                //stepModel.SaveAs($"{filePath}{fileName}material_moded.ifc");
                //Console.WriteLine(product);

                ////open XML file
                //using (var xmlModel = IfcStore.Open("SampleHouse.ifcxml"))
                //{
                //    //just have a look that it contains the same number of entities and walls.
                //    var stepCount = stepModel.Instances.Count;
                //    var xmlCount = xmlModel.Instances.Count;

                //    var stepWallsCount = stepModel.Instances.CountOf<IIfcWall>();
                //    var xmlWallsCount = xmlModel.Instances.CountOf<IIfcWall>();

                //    Console.WriteLine($"STEP21 file has {stepCount} entities. XML file has {xmlCount} entities.");
                //    Console.WriteLine($"STEP21 file has {stepWallsCount} walls. XML file has {xmlWallsCount} walls.");

                //}
            }
        }
    }
}

