using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Sofistik_CDB
{
    public class Sofistik_CDBInfo : GH_AssemblyInfo
    {
        public override string Name => "Sofistik CDB";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("FA4003D3-4B6E-4996-9A88-CCFB6E2F5093");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}