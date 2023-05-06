using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Sofistik_CDB.ForAndDis
{
    public class NodeDisp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NodeDisp class.
        /// </summary>
        public NodeDisp()
          : base("NodeDisp", "NodeDisp",
              "Returns Node displacements",
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
            pManager.AddTextParameter("Header", "Header", " Node output info", GH_ParamAccess.tree);
            pManager.AddGenericParameter("NodeDisp", "NodeDisp", "NodeDisp", GH_ParamAccess.tree);
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
            GH_Structure<IGH_Goo> NodeDisplacement = new GH_Structure<IGH_Goo>();
            Header.AppendRange(new List<GH_String> { new GH_String("Node"), new GH_String("UX"), new GH_String("UY"), new GH_String("UZ"), new GH_String("URX"), new GH_String("URY"), new GH_String("URZ"), new GH_String("URB") }, new GH_Path(0));

            sofCDB.Program.NodeDisp(ref NodeDisplacement, LC, path);

            DA.SetDataTree(0, Header);
            DA.SetDataTree(1, NodeDisplacement);

        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.NODI;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("EDD725FA-1E74-4B54-A717-C261A7865922"); }
        }
    }
}