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
using Xbim.Ifc4.GeometryResource;
using System.Collections.Generic;
using System.IO;
using Xbim.Common.Geometry;
using System.Runtime.InteropServices.ComTypes;
using Xbim.Common.Model;

namespace IFCDataExtraction
{
    class Program
    {
        //Point Struct to represent the element coordinates.
        public struct Point
        {
            public double X;
            public double Y;
            public double Z;

            public void print()
            {
                Console.WriteLine($"({X}, {Y}, {Z})");
            }
        }
        public struct ModelData
        {
            public Point StartPoint;
            public Point EndPoint;
            public string Name;
        }
        public static ModelData GetColumnData(IfcStore stepModel)
        {
            //Get the Column Data for determine the coordinates.
            var ColumnRepresentaion = (stepModel.Instances.OfType<IIfcColumn>().ToList()[0].Representation.Representations.FirstOrDefault().Items.FirstOrDefault() as IIfcExtrudedAreaSolid);
            var ColumnCoordinates = ((stepModel.Instances.OfType<IIfcColumn>().ToList()[0].ObjectPlacement as IIfcLocalPlacement).RelativePlacement as IIfcAxis2Placement3D).Location as IIfcCartesianPoint;
            var ColumnDirection = (((stepModel.Instances.OfType<IIfcColumn>().ToList()[0].ObjectPlacement as IIfcLocalPlacement).RelativePlacement as IIfcAxis2Placement3D).Axis).Z;


            var ColumnX1 = ColumnCoordinates.X;
            var ColumnY1 = ColumnCoordinates.Y;
            var ColumnZ1 = ColumnCoordinates.Z;
            Point ColumnStartPoint = new Point() { X = ColumnX1, Y = ColumnY1, Z = ColumnZ1 };

            var ColumnDepth = (double)ColumnRepresentaion.Depth; //The column height.

            var ColumnX2 = ColumnX1;
            var ColumnY2 = ColumnY1;
            var ColumnZ2 = ColumnDirection == -1 ? ColumnZ1 - ColumnDepth : ColumnZ1 + ColumnDepth;
            Point ColumnEndPoint = new Point() { X = ColumnX2, Y = ColumnY2, Z = ColumnZ2 };


            //Get the column name.
            string ColumnName = stepModel.Instances.OfType<IIfcColumn>().FirstOrDefault().Name;

            ModelData Model = new ModelData
            {
                StartPoint = ColumnStartPoint,
                EndPoint = ColumnEndPoint,
                Name = ColumnName
            };

            return Model;
        }

        public static ModelData GetBeamData(IfcStore stepModel)
        {
            //Get the Beam Data to determine the coordinates.
            var BeamRepresentaion = (stepModel.Instances.OfType<IIfcBeam>().FirstOrDefault().Representation.Representations.FirstOrDefault().Items.FirstOrDefault() as IIfcExtrudedAreaSolid);
            var BeamCoordinates = ((stepModel.Instances.OfType<IIfcBeam>().FirstOrDefault().ObjectPlacement as IIfcLocalPlacement).RelativePlacement as IIfcAxis2Placement3D).Location as IIfcCartesianPoint;
            var BeamDirection = ((stepModel.Instances.OfType<IIfcBeam>().FirstOrDefault().ObjectPlacement as IIfcLocalPlacement).RelativePlacement as IIfcAxis2Placement3D).Axis;

            var BeamX1 = BeamCoordinates.X;
            var BeamY1 = BeamCoordinates.Y;
            var BeamZ1 = BeamCoordinates.Z;

            Point BeamStartPoint = new Point() { X = BeamX1, Y = BeamY1, Z = BeamZ1 };

            var BeamDepth = (double)BeamRepresentaion.Depth; //The beam length.

            var BeamX2 = BeamX1;
            var BeamY2 = BeamY1;
            var BeamZ2 = BeamZ1;

            //Check the direction of the beam
            var DirectionX = BeamDirection.X;
            var DirectionY = BeamDirection.Y;

            var Direction = GetDirection(DirectionX, DirectionY);

            switch (Direction)
            {
                case 1:
                    BeamX2 = BeamX1 + BeamDepth;
                    break;
                case -1:
                    BeamX2 = BeamX1 - BeamDepth;
                    break;
                case 2:
                    BeamY2 = BeamY1 + BeamDepth;
                    break;
                case -2:
                    BeamY2 = BeamY1 - BeamDepth;
                    break;
            }
            Point BeamEndPoint = new Point() { X = BeamX2, Y = BeamY2, Z = BeamZ2 };

            //Get the column name.
            string BeamName = stepModel.Instances.OfType<IIfcBeam>().FirstOrDefault().Name;

            //Get the BeamModelData
            ModelData Model = new ModelData
            {
                StartPoint = BeamStartPoint,
                EndPoint = BeamEndPoint,
                Name = BeamName
            };

            return Model;
        }



    //Get the direction of the beam to determine the end point.
    public static double GetDirection(double x, double y)
        {
            if (x != 0) return x;

            if (y != 0) return y * 2;

            return 0;
        }
        static void Main(string[] args)
        {

            //Put the IFC file in the IFC_Files Folder, and change the fileName variable to your file name || change the filePath variable to your file path.
            string Path = @"..\..\..\IFC_File\";
            //string fileName = "CMC02";
            //string fileName = "CMC02(ifc4)";
            string fileName = "test";
            //string fileName = "test(ifc2x3)";
            string filePath = $"{Path}{fileName}.ifc";

            //open STEP file
            using (var stepModel = IfcStore.Open(filePath))
            {
                //stepModel.SaveAs($"{Path}{fileName}.ifcxml");
                var version = stepModel.SchemaVersion;

                //Get Column Data
                var Column = GetColumnData(stepModel);

                //Get Beam Data
                var Beam = GetBeamData(stepModel);


                //Print the results.
                Console.Write($"Column({Column.Name}) Start Point: ");
                Column.StartPoint.print();

                Console.Write($"Column({Column.Name}) End Point: ");
                Column.EndPoint.print();

                Console.Write($"Beam({Beam.Name}) Start Point: ");
                Beam.StartPoint.print();

                Console.Write($"Beam({Beam.Name}) End Point: ");
                Beam.EndPoint.print();

                Console.WriteLine($"File IFC Version: {version}");
                Console.ReadKey();
            }
            
        }
    }
}

