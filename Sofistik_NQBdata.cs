using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using SofistikDataTypes;
using sofCDB;


//importing DLLS
using System.Runtime.InteropServices;
using System.Reflection;
using Grasshopper.Kernel.Data;

namespace Sofistik_CDB
{
    public class Sofistik_NQBdata : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Sofistik_NQBdata()
          : base("Node/Quad/Beam", "Node/Quad/Beam",
            "Reads Node, Quad and beam data froms CDB.",
            "Sofi CDB reader", "CDB Adapters")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CDB Path", "CDB Path", "Provide CDB Path", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

            pManager.AddGenericParameter("Headers", "Headers", "Headers", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Node Data", "Node Data", "Node Data",GH_ParamAccess.tree);
            pManager.AddGenericParameter("Quad Data", "Quad Data", "Quad Data", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Beam Data", "Beam Data", "Beam Data", GH_ParamAccess.tree);
            

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string path = "";



            if (!DA.GetData(0, ref path)) return;


            //path = path;
            GH_Structure<IGH_Goo> Header = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> NodeDATA = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> QuadDATA = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> BeamDATA = new GH_Structure<IGH_Goo>();

            Header.AppendRange(new List<GH_String> { new GH_String("Index"), new GH_String("NodeNum"), new GH_String("Node[XYZ]") }, new GH_Path(0));
            Header.AppendRange(new List<GH_String> { new GH_String("Quad"), new GH_String("Node1"), new GH_String("Node2"), new GH_String("Node3"), new GH_String("Node4"), new GH_String("T1"), new GH_String("T2"), new GH_String("T3") }, new GH_Path(1));
            Header.AppendRange(new List<GH_String> { new GH_String("Beam"), new GH_String("Node1"), new GH_String("Node2"), new GH_String("T1"), new GH_String("T2"), new GH_String("T3") }, new GH_Path(2));
            sofCDB.Program.NoQuBe(ref NodeDATA, ref QuadDATA, ref BeamDATA, path);

            DA.SetDataTree(0, Header);
            DA.SetDataTree(1, NodeDATA);
            DA.SetDataTree(2, QuadDATA);
            DA.SetDataTree(3, BeamDATA);

        }


        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.NodeBeamQuad;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("B2C13319-DC52-4ED9-B5A5-1E82D0C9D832");
    }
}