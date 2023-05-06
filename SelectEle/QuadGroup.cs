using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using SofistikDataTypes;
using sofCDB;
using System.Linq;


//importing DLLS
using System.Runtime.InteropServices;
using System.Reflection;
using Grasshopper.Kernel.Data;
using System.Collections;
using Grasshopper.Documentation;

namespace Sofistik_CDB.SelectEle
{
    public class QuadGroup : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuadGroup class.
        /// </summary>
        public QuadGroup()
          : base("Quad Group", "Quad Group",
            "Return Quad Element info for a Group.",
            "Sofi CDB reader", "SelectEle")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quad Data", "Q Data", "Q Data retrived from CDB", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Group Num", "Group Num", "Provide a group number (int)", GH_ParamAccess.item, 5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Header", "Header", "HeaderQuad Element info", GH_ParamAccess.list);
            pManager.AddGenericParameter("Quad Data", "Quad Data", "Quad Element info", GH_ParamAccess.tree);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_Goo> QuadInfo = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> QuadInfoOut = new GH_Structure<IGH_Goo>();
            List<string> header = new List<string> { "Elem Num", "Node1", "Node2", "Node3", "Node4", "T1", "T2", "T3" };
            //List<string> header = new List<string> {"Quad","FirNode","Node(2,3,4)","T1","T2","T3"};


            int GroupNum = 0;

            if (!DA.GetDataTree(0, out QuadInfo)) return;
            if (!DA.GetData(1, ref GroupNum)) return;

            int FilterVal1 = GroupNum * 10000; int FilterVal2 = (GroupNum + 1) * 10000;

            QuadInfoOut.Clear();
            int NumOfEle = QuadInfo.get_Branch(new GH_Path(0)).Count;

            IEnumerator B0 = QuadInfo.get_Branch(new GH_Path(0)).GetEnumerator();
            IEnumerator B1 = QuadInfo.get_Branch(new GH_Path(1)).GetEnumerator();
            IEnumerator B2 = QuadInfo.get_Branch(new GH_Path(2)).GetEnumerator();
            IEnumerator B3 = QuadInfo.get_Branch(new GH_Path(3)).GetEnumerator();
            IEnumerator B4 = QuadInfo.get_Branch(new GH_Path(4)).GetEnumerator();
            IEnumerator B5 = QuadInfo.get_Branch(new GH_Path(5)).GetEnumerator();
            IEnumerator B6 = QuadInfo.get_Branch(new GH_Path(6)).GetEnumerator();
            IEnumerator B7 = QuadInfo.get_Branch(new GH_Path(7)).GetEnumerator();

            while (B0.MoveNext())
            {
                B1.MoveNext(); B2.MoveNext(); B3.MoveNext(); B4.MoveNext(); B5.MoveNext(); B6.MoveNext(); B7.MoveNext();

                int Curr = 0;
                GH_Integer ghint = (GH_Integer)B0.Current;
                ghint.CastTo(ref Curr);

                if (Curr > FilterVal1 && Curr < FilterVal2)
                {
                    QuadInfoOut.Append((IGH_Goo)B0.Current, new GH_Path(0));
                    QuadInfoOut.Append((IGH_Goo)B1.Current, new GH_Path(1));
                    QuadInfoOut.Append((IGH_Goo)B2.Current, new GH_Path(2));
                    QuadInfoOut.Append((IGH_Goo)B3.Current, new GH_Path(3));
                    QuadInfoOut.Append((IGH_Goo)B4.Current, new GH_Path(4));
                    QuadInfoOut.Append((IGH_Goo)B5.Current, new GH_Path(5));
                    QuadInfoOut.Append((IGH_Goo)B6.Current, new GH_Path(6));
                    QuadInfoOut.Append((IGH_Goo)B7.Current, new GH_Path(7));
                }


            }

            DA.SetDataList(0, header);
            DA.SetDataTree(1, QuadInfoOut);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.QuadGroup;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1D8EFC0C-8310-485C-ADCF-6D41D4939B86"); }
        }
    }
}