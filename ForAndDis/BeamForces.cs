using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Sofistik_CDB.ForAndDis
{
    public class BeamForces : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BeamForces class.
        /// </summary>
        public BeamForces()
          : base("Beam Forces", "Beam Forces",
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
            pManager.AddTextParameter("Header", "Header", "Header Beam Element info", GH_ParamAccess.tree);
            pManager.AddGenericParameter("NodalForces", "NodeForces", "BeamNodeForces", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Eigen Forces", "Eigen Forces", "Beam Eigen Forces", GH_ParamAccess.tree);

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
            GH_Structure<IGH_Goo> BeamForces = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> BeamEigen = new GH_Structure<IGH_Goo>();

            Header.AppendRange(new List<GH_String> { new GH_String("BeamNum"), new GH_String("DisX"), new GH_String("N"), new GH_String("VY"), new GH_String("VZ"), new GH_String("T"), new GH_String("MY"), new GH_String("MZ"), new GH_String("War"), new GH_String("2TM"), new GH_String("DLX"), new GH_String("DLY"), new GH_String("DLZ"), new GH_String("RotX"), new GH_String("RotY"), new GH_String("RotZ"), new GH_String("Twist") }, new GH_Path(0));
            Header.AppendRange(new List<GH_String> { new GH_String("BeamNum"), new GH_String("Iden"), new GH_String("DisX"), new GH_String("N"), new GH_String("VY"), new GH_String("VZ"), new GH_String("T"), new GH_String("MY"), new GH_String("MZ"), new GH_String("N-Rel"), new GH_String("VY-Rel"), new GH_String("VZ-Rel"), new GH_String("T-Rel"), new GH_String("MY-Rel"), new GH_String("MZ-Rel") }, new GH_Path(1));

            sofCDB.Program.BeamForces(ref BeamForces, ref BeamEigen, LC, path);

            DA.SetDataTree(0, Header);
            DA.SetDataTree(1, BeamForces);
            DA.SetDataTree(2, BeamEigen);

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
                return Properties.Resources.BeamF ;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("A1D1768A-4133-4DAE-93AA-6BF18845B932"); }
        }
    }
}