using System;
using System.Collections;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Geometry.SpatialTrees;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using GHCustomControls;

namespace Sofistik_CDB
{
    public class NodeDataTree : GHCustomComponent
    {


        PushButton pushButton;
        /// <summary>
        /// Initializes a new instance of the NodeDataTree class.
        /// </summary>
        public NodeDataTree()
          : base("NodeDataTree", "NodeDataTree",
              "Split Node Data",
              "Sofi CDB reader", "DT")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Node Data", "Node Data", "Node Data retrived from CDB", GH_ParamAccess.tree);
            
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            
            pManager.AddGenericParameter("Index", "Index", "Node Index", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Node Num", "Node Num", "Node Num", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Coordinates", "Coordinates", "Coordinates", GH_ParamAccess.tree);
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Structure<IGH_Goo> NodeInfo = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> INDEX = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> NUM = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> COOR = new GH_Structure<IGH_Goo>();

            if (!DA.GetDataTree(0, out NodeInfo)) return;

            IEnumerator B0 = NodeInfo.get_Branch(new GH_Path(0)).GetEnumerator();
            IEnumerator B1 = NodeInfo.get_Branch(new GH_Path(1)).GetEnumerator();
            IEnumerator B2 = NodeInfo.get_Branch(new GH_Path(2)).GetEnumerator();


            while (B1.MoveNext())
            {
                B0.MoveNext(); B2.MoveNext();

                INDEX.Append((IGH_Goo)B0.Current);
                NUM.Append((IGH_Goo)B1.Current);
                COOR.Append((IGH_Goo)B2.Current);

            }

            DA.SetDataList(0, INDEX);
            DA.SetDataTree(1, NUM);
            DA.SetDataTree(2, COOR);


        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.NodeDataTree;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("111E5E30-F2CC-45F3-AE5A-78CD85BB3659"); }
        }
    }
}