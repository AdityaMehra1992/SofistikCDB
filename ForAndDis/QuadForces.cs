using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Sofistik_CDB.ForAndDis
{
    public class QuadForces : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuadForces class.
        /// </summary>
        public QuadForces()
          : base("QuadForces", "QuadForces",
              "Returns Node Forces, Element Forces and Gauss Point forces for all effects (N,M & V).",
              "Sofi CDB reader", "CDB Adapters")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {

            pManager.AddTextParameter("CDB Path", "CDB Path", "Provide CDB Path", GH_ParamAccess.item);
            pManager.AddIntegerParameter("LC", "LC", "LoadCase Number (int)", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {

            pManager.AddTextParameter("Header", "Header", "HeaderQuad Element info", GH_ParamAccess.tree);
            pManager.AddGenericParameter("NodeForces", "NodeForces", "QuadNodeForces", GH_ParamAccess.tree);
            pManager.AddGenericParameter("EleForces", "EleForces", "Quad Element Forces", GH_ParamAccess.tree);
            pManager.AddGenericParameter("GaussPointForces", "GaussPointForces", "GaussPointForces", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            int LC = 1;
            if (!DA.GetData(0, ref path)) return;
            if (!DA.GetData(1, ref LC)) return;

            GH_Structure<IGH_Goo> Header = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> NodeForces = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> EleForces = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> GaussPointForces = new GH_Structure<IGH_Goo>();

            Header.AppendRange(new List<GH_String> { new GH_String("GRP"), new GH_String("NodeNum"), new GH_String("M_mxx"), new GH_String("M_myy"), new GH_String("T_mxy"), new GH_String("VX"), new GH_String("VY"), new GH_String("NX"), new GH_String("NY"), new GH_String("NXY"), new GH_String("VX signed"), new GH_String("VY signed") }, new GH_Path(0));
            Header.AppendRange(new List<GH_String> { new GH_String("QuadNum"), new GH_String("M_mxx"), new GH_String("M_myy"), new GH_String("T_mxy"), new GH_String("VX"), new GH_String("VY"), new GH_String("NX"), new GH_String("NY"), new GH_String("NXY") }, new GH_Path(1));




            sofCDB.Program.QuadForcesNodes(ref NodeForces, ref EleForces, LC, path);

            DA.SetDataTree(0, Header);
            DA.SetDataTree(1, NodeForces);
            DA.SetDataTree(2, EleForces);
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.QuadForce;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("D1357E26-781E-44EF-9462-4BEBF5E98EFC"); }
        }
    }
}