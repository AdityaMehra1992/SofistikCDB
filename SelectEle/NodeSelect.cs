using System;
using System.Collections;
using System.Collections.Generic;
using Grasshopper.Documentation;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Sofistik_CDB.SelectEle
{
    public class NodeSelect : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NodeSelect class.
        /// </summary>
        public NodeSelect()
          : base("NodeSelect", "NodeSelect",
              "Return Node Data",
              "Sofi CDB reader", "SelectEle")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Node Data", "Node Data", "Node Data retrived from CDB", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Node Num", "Node Num", "Provide a Node number (int)", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Header", "Header", "HeaderQuad Element info", GH_ParamAccess.list);
            pManager.AddGenericParameter("Node Data", "Node Data", "Node Element info", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_Goo> NodeInfo = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> NodeInfoOut = new GH_Structure<IGH_Goo>();
            List<string> header = new List<string> { "Index", "Node Num", "Coordinates" };
            List<int> NodesSele = new List<int>();


            if (!DA.GetDataTree(0, out NodeInfo)) return;
            if (!DA.GetDataList(1, NodesSele)) return;

            NodeInfoOut.Clear();

            int NumOfEle = NodeInfo.get_Branch(new GH_Path(0)).Count;
            int NodesSeleLen = NodesSele.Count;

            IEnumerator B0 = NodeInfo.get_Branch(new GH_Path(0)).GetEnumerator();
            IEnumerator B1 = NodeInfo.get_Branch(new GH_Path(1)).GetEnumerator();
            IEnumerator B2 = NodeInfo.get_Branch(new GH_Path(2)).GetEnumerator();


            IGH_Goo[] C0 = new IGH_Goo[NodesSele.Count];
            IGH_Goo[] C1 = new IGH_Goo[NodesSele.Count];
            IGH_Goo[] C2 = new IGH_Goo[NodesSele.Count];


            while (B1.MoveNext())
            {
                B0.MoveNext(); B2.MoveNext();

                int Curr = 0;
                GH_Integer ghint = (GH_Integer)B1.Current;
                ghint.CastTo(ref Curr);

                if (NodesSele.Contains(Curr))
                {
                    List<int> outputindexs = new List<int>();
                    checkindex(Curr, 0, NodesSele, ref outputindexs);
                    foreach (int index in outputindexs)
                    {
                        C0[index] = (IGH_Goo)B0.Current;
                        C1[index] = (IGH_Goo)B1.Current;
                        C2[index] = (IGH_Goo)B2.Current;

                    }

                }

            }
            NodeInfoOut.AppendRange(C0, new GH_Path(0));
            NodeInfoOut.AppendRange(C1, new GH_Path(1));
            NodeInfoOut.AppendRange(C2, new GH_Path(2));

            DA.SetDataList(0, header);
            DA.SetDataTree(1, NodeInfoOut);
        }

        void checkindex(int Curr, int CurrentIndex, List<int> NodesSele, ref List<int> outputindexs)
        {
            int index = NodesSele.IndexOf(Curr, CurrentIndex);
            if (index != -1)
            {
                outputindexs.Add(index);
                checkindex(Curr, index + 1, NodesSele, ref outputindexs);
            }

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.NodeSelect;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("83EEB7D4-590D-4468-A16D-76B2F530D401"); }
        }
    }
}