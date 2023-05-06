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
    public class BeamGroup : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BeamGroup class.
        /// </summary>
        public BeamGroup()
          : base("BeamGroup", "BeamGroup",
              "Return Beam Element info for a Group num",
              "Sofi CDB reader", "SelectEle")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Beam Data", "Beam Data", "Beam Data retrived from CDB", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Group Num", "Group Num", "Provide a group number (int)", GH_ParamAccess.item, 5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Header", "Header", "HeaderQuad Element info", GH_ParamAccess.list);
            pManager.AddGenericParameter("Beam Data", "Beam Data", "Beam Element info", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_Goo> BeamInfo = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> BeamInfoOut = new GH_Structure<IGH_Goo>();
            List<string> header = new List<string> { "Elem Num", "Node1", "Node2", "T1", "T2", "T3" };

            int GroupNum = 0;

            if (!DA.GetDataTree(0, out BeamInfo)) return;
            if (!DA.GetData(1, ref GroupNum)) return;

            int FilterVal1 = GroupNum * 10000; int FilterVal2 = (GroupNum + 1) * 10000;

            BeamInfoOut.Clear();
            int NumOfEle = BeamInfo.get_Branch(new GH_Path(0)).Count;

            IEnumerator B0 = BeamInfo.get_Branch(new GH_Path(0)).GetEnumerator();
            IEnumerator B1 = BeamInfo.get_Branch(new GH_Path(1)).GetEnumerator();
            IEnumerator B2 = BeamInfo.get_Branch(new GH_Path(2)).GetEnumerator();
            IEnumerator B3 = BeamInfo.get_Branch(new GH_Path(3)).GetEnumerator();
            IEnumerator B4 = BeamInfo.get_Branch(new GH_Path(4)).GetEnumerator();
            IEnumerator B5 = BeamInfo.get_Branch(new GH_Path(5)).GetEnumerator();


            while (B0.MoveNext())
            {
                B1.MoveNext(); B2.MoveNext(); B3.MoveNext(); B4.MoveNext(); B5.MoveNext();

                int Curr = 0;
                GH_Integer ghint = (GH_Integer)B0.Current;
                ghint.CastTo(ref Curr);

                if (Curr > FilterVal1 && Curr < FilterVal2)
                {
                    BeamInfoOut.Append((IGH_Goo)B0.Current, new GH_Path(0));
                    BeamInfoOut.Append((IGH_Goo)B1.Current, new GH_Path(1));
                    BeamInfoOut.Append((IGH_Goo)B2.Current, new GH_Path(2));
                    BeamInfoOut.Append((IGH_Goo)B3.Current, new GH_Path(3));
                    BeamInfoOut.Append((IGH_Goo)B4.Current, new GH_Path(4));
                    BeamInfoOut.Append((IGH_Goo)B5.Current, new GH_Path(5));
                }

            }
            DA.SetDataList(0, header);
            DA.SetDataTree(1, BeamInfoOut);
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
                return Properties.Resources.BeamGroup;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0D0A00DD-E20A-40B8-BF1F-7EACF44F4CCA"); }
        }
    }
}