//+============================================================================+
//| Company:   SOFiSTiK AG                                                     |
//| Version:   SOFiSTIK 2022                                                   |
//+============================================================================+

// The sofistik_daten.cs (SofistikDataTypes) can be found in the c# example folder
using Rhino.Geometry;
using SofistikDataTypes;
using System;
using System.Data;
using Grasshopper;
using Grasshopper.Kernel;
using System.Collections.Generic;


//importing DLLS
using System.Runtime.InteropServices;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Linq;

namespace sofCDB
{

    unsafe
    class Program
    {
        // In this example 64bit dlls are used (Visual Studio Platform 64bit)
        // If you are using Visual Studio, you need to use unsafe code blocks
        // Open "Project properties" -> "Build Tab" -> "Allow Unsafe Code"

        // sof_cdb_init
        [DllImport("sof_cdb_w-2022.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sof_cdb_init(
            string name_,
            int initType_
        );

        // sof_cdb_close
        [DllImport("sof_cdb_w-2022.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void sof_cdb_close(
            int index_);

        // sof_cdb_status
        [DllImport("sof_cdb_w-2022.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sof_cdb_status(
            int index_);

        // sof_cdb_flush
        [DllImport("sof_cdb_w-2022.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sof_cdb_flush(
            int index_);

        // sof_cdb_flush
        [DllImport("sof_cdb_w-2023.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sof_cdb_free(
            int kwh_,
            int kwl_);

        // sof_cdb_flush
        [DllImport("sof_cdb_w-2022.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void sof_cdb_kenq_ex(
            int index,
            ref int kwh_,
            ref int kwl_,
            int request_);

        // sof_cdb_get
        [DllImport("sof_cdb_w-2022.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sof_cdb_get(
            int index_,
            int kwh_,
            int kwl_,
            void* type_,
            ref int recLen_,
            int pos);

        // Define the path of the dlls
        public const int Index = 0;
        public const int Status = 0;
        public const string Directory1 = @"C:\Program Files\SOFiSTiK\2022\SOFiSTiK 2022\interfaces\64bit"; // provide path for \SOFiSTiK xxxx\interfaces\64bit
        public const string Directory2 = @"C:\Program Files\SOFiSTiK\2022\SOFiSTiK 2022";                   // provide path for sofistik folder

        static void Main(string[] args)
        {
        }
        /// <summary>
        /// Get Node Quad and beam data
        /// </summary>
        public static void NoQuBe(ref GH_Structure<IGH_Goo> NodeDATA, ref GH_Structure<IGH_Goo> QuadDATA, ref GH_Structure<IGH_Goo> BeamDATA, string cdbpath)
        {
            //double aad = 0;
            //int zz = Months;
            int index = Index;
            int status = Status;
            string directory1 = Directory1;
            string directory2 = Directory2;
            // Get the path
            string path = Environment.GetEnvironmentVariable("path");

            // Set the new path environment variable + SOFiSTiK dlls path
            path = directory1 + ";" + directory2 + ";" + path;

            // Set the path variable (to read the data from CDB)
            System.Environment.SetEnvironmentVariable("path", path);

            // Connect to CDB, int sof_cdb_init  ( char* FileName, int Index);
            // Always use index 99, for more details see cdbase.chm
            index = sof_cdb_init(cdbpath, 99);
            if (index < 0)
            {
                Console.WriteLine("ERROR: Index = " + index + " < 0 - see clib1.h for meaning of error code");
                return;
            }
            else if (index == 0)
            {
                Console.WriteLine("ERROR: Index = " + index + " - The file is not a database");
                return;
            }

            // Check if sof_cdb_flush is working
            status = sof_cdb_status(index);

            // data as cs_node
            cs_node dataNode;
            cs_quad dataQuad;
            cs_beam dataBeam;

            // get the length of the structure
            int dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_node));
            int dataQuadlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_quad));
            int dataBeamlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_beam));

            // the kwh = 20, kwl = 0, pos = 1
            while (sof_cdb_get(index, 20, 0, &dataNode, ref dataNodelen, 3) == 0)
            {

                NodeDATA.Append(new GH_Integer(dataNode.m_inr-1),new GH_Path(0));
                NodeDATA.Append(new GH_Integer(dataNode.m_nr), new GH_Path(1));
                NodeDATA.Append(new GH_Vector(new Vector3d(dataNode.m_xyz[0], dataNode.m_xyz[1], dataNode.m_xyz[2])), new GH_Path(2));

                dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_node));
            }

            while (sof_cdb_get(index, 200, 0, &dataQuad, ref dataQuadlen, 3) == 0)
            {
               
                QuadDATA.Append(new GH_Integer(dataQuad.m_nr), new GH_Path(0));
                QuadDATA.Append(new GH_Integer(dataQuad.m_node[0]), new GH_Path(1));
                QuadDATA.Append(new GH_Integer(dataQuad.m_node[1]), new GH_Path(2));
                QuadDATA.Append(new GH_Integer( dataQuad.m_node[2]), new GH_Path(3));
                QuadDATA.Append(new GH_Integer(dataQuad.m_node[3]), new GH_Path(4));

                QuadDATA.Append(new GH_Vector(new Vector3d(dataQuad.m_t[0], dataQuad.m_t[1], dataQuad.m_t[2])), new GH_Path(5));
                QuadDATA.Append(new GH_Vector(new Vector3d(dataQuad.m_t[3], dataQuad.m_t[4], dataQuad.m_t[5])), new GH_Path(6));
                QuadDATA.Append(new GH_Vector(new Vector3d(dataQuad.m_t[6], dataQuad.m_t[7], dataQuad.m_t[8])), new GH_Path(7));

                dataQuadlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_quad));
            }

            while (sof_cdb_get(index, 100, 0, &dataBeam, ref dataBeamlen, 3) == 0)
            {
               if (dataBeam.m_nr != 0)
                {
                    BeamDATA.Append(new GH_Integer(dataBeam.m_nr), new GH_Path(0));
                    BeamDATA.Append(new GH_Integer(dataBeam.m_node[0]), new GH_Path(1));
                    BeamDATA.Append(new GH_Integer(dataBeam.m_node[1]), new GH_Path(2));
                    BeamDATA.Append(new GH_Vector(new Vector3d(dataBeam.m_t[0], dataBeam.m_t[1], dataBeam.m_t[2])), new GH_Path(3));
                    BeamDATA.Append(new GH_Vector(new Vector3d(dataBeam.m_t[3], dataBeam.m_t[4], dataBeam.m_t[5])), new GH_Path(4));
                    BeamDATA.Append(new GH_Vector(new Vector3d(dataBeam.m_t[6], dataBeam.m_t[7], dataBeam.m_t[8])), new GH_Path(5));
                }
                dataBeamlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_beam));
            }

            sof_cdb_flush(index);
            sof_cdb_close(0);

        }

        public static void QuadForcesNodes(ref GH_Structure<IGH_Goo> NodeForces, ref GH_Structure<IGH_Goo> EleForces, int LC, string cdbpath)
        {

            int index = Index;
            int status = Status;
            string directory1 = Directory1;
            string directory2 = Directory2;
            // Get the path
            string path = Environment.GetEnvironmentVariable("path");

            // Set the new path environment variable + SOFiSTiK dlls path
            path = directory1 + ";" + directory2 + ";" + path;

            // Set the path variable (to read the data from CDB)
            System.Environment.SetEnvironmentVariable("path", path);


            // Connect to CDB, int sof_cdb_init  ( char* FileName, int Index);
            // Always use index 99, for more details see cdbase.chm
            index = sof_cdb_init(cdbpath, 99);


            // Check if sof_cdb_flush is working
            status = sof_cdb_status(index);

            // data as cs_node
            cs_quad_nfo dataNode;
            cs_quad_for dataQuad;

            // get the length of the structure
            int dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_quad_nfo));
            int dataQuadlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_quad_for));
            

            // the kwh = 211, kwl = LC, pos = 1
            while (sof_cdb_get(index, 211, LC, &dataNode, ref dataNodelen, 3) == 0)
            {
                if (dataNode.m_ng != 0)
                {
                    NodeForces.Append(new GH_Integer(dataNode.m_ng), new GH_Path(0));
                    NodeForces.Append(new GH_Integer(dataNode.m_nr), new GH_Path(1));
                    NodeForces.Append(new GH_Number(dataNode.m_mxx), new GH_Path(2));
                    NodeForces.Append(new GH_Number(dataNode.m_myy), new GH_Path(3));
                    NodeForces.Append(new GH_Number(dataNode.m_mxy), new GH_Path(4));
                    NodeForces.Append(new GH_Number(dataNode.m_vx), new GH_Path(5));
                    NodeForces.Append(new GH_Number(dataNode.m_vy), new GH_Path(6));
                    NodeForces.Append(new GH_Number(dataNode.m_nx), new GH_Path(7));
                    NodeForces.Append(new GH_Number(dataNode.m_ny), new GH_Path(8));
                    NodeForces.Append(new GH_Number(dataNode.m_nxy), new GH_Path(9));
                    NodeForces.Append(new GH_Number(dataNode.m_vx_v), new GH_Path(10));
                    NodeForces.Append(new GH_Number(dataNode.m_vy_v), new GH_Path(11));
                }
                dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_quad_nfo));
            }

            while (sof_cdb_get(index, 210, LC, &dataQuad, ref dataQuadlen, 3) == 0)
            {
                if (dataQuad.m_nr != 0)
                {
                    EleForces.Append(new GH_Integer(dataQuad.m_nr), new GH_Path(0));
                    EleForces.Append(new GH_Number(dataQuad.m_mxx), new GH_Path(1));
                    EleForces.Append(new GH_Number(dataQuad.m_myy), new GH_Path(2));
                    EleForces.Append(new GH_Number(dataQuad.m_mxy), new GH_Path(3));
                    EleForces.Append(new GH_Number(dataQuad.m_vx), new GH_Path(4));
                    EleForces.Append(new GH_Number(dataQuad.m_vy), new GH_Path(5));
                    EleForces.Append(new GH_Number(dataQuad.m_nx), new GH_Path(6));
                    EleForces.Append(new GH_Number(dataQuad.m_ny), new GH_Path(7));
                    EleForces.Append(new GH_Number(dataQuad.m_nxy), new GH_Path(8));
                }
                dataQuadlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_quad_for));
            }

            // use sof_cdb_flush() and sof_cdb_close()
            sof_cdb_flush(index);
            // close the CDB
            sof_cdb_close(0);
        }

        public static void BeamForces(ref GH_Structure<IGH_Goo> BeamForces, ref GH_Structure<IGH_Goo> BeamEigen, int LC, string cdbpath)
        {

            int index = Index;
            int status = Status;
            string directory1 = Directory1;
            string directory2 = Directory2;
            // Get the path
            string path = Environment.GetEnvironmentVariable("path");

            // Set the new path environment variable + SOFiSTiK dlls path
            path = directory1 + ";" + directory2 + ";" + path;

            // Set the path variable (to read the data from CDB)
            System.Environment.SetEnvironmentVariable("path", path);

            // Connect to CDB, int sof_cdb_init  ( char* FileName, int Index);
            // Always use index 99, for more details see cdbase.chm
            index = sof_cdb_init(cdbpath, 99);


            // Check if sof_cdb_flush is working
            status = sof_cdb_status(index);

            // data as cs_node
            cs_beam_for dataNode;
            cs_beam_crf dataEigen;

            // get the length of the structure
            int dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_beam_for));
            int dataEigenlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_beam_crf));

            // the kwh = 211, kwl = LC, pos = 1
            while (sof_cdb_get(index, 102, LC, &dataNode, ref dataNodelen, 3) == 0)
            {
                BeamForces.Append(new GH_Integer(dataNode.m_nr), new GH_Path(0));
                BeamForces.Append(new GH_Number(dataNode.m_x), new GH_Path(1));
                BeamForces.Append(new GH_Number(dataNode.m_n), new GH_Path(2));
                BeamForces.Append(new GH_Number(dataNode.m_vy), new GH_Path(3));
                BeamForces.Append(new GH_Number(dataNode.m_vz), new GH_Path(4));
                BeamForces.Append(new GH_Number(dataNode.m_mt), new GH_Path(5));
                BeamForces.Append(new GH_Number(dataNode.m_my), new GH_Path(6));
                BeamForces.Append(new GH_Number(dataNode.m_mz), new GH_Path(7));
                BeamForces.Append(new GH_Number(dataNode.m_mb), new GH_Path(8));
                BeamForces.Append(new GH_Number(dataNode.m_mt2), new GH_Path(9));
                BeamForces.Append(new GH_Number(dataNode.m_ux), new GH_Path(10));
                BeamForces.Append(new GH_Number(dataNode.m_uy), new GH_Path(11));
                BeamForces.Append(new GH_Number(dataNode.m_uz), new GH_Path(12));
                BeamForces.Append(new GH_Number(dataNode.m_phix), new GH_Path(13));
                BeamForces.Append(new GH_Number(dataNode.m_phiy), new GH_Path(14));
                BeamForces.Append(new GH_Number(dataNode.m_phiz), new GH_Path(15));
                BeamForces.Append(new GH_Number(dataNode.m_phiw), new GH_Path(16));
                

                dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_beam_for));
            }

            while (sof_cdb_get(index, 104, LC, &dataEigen, ref dataEigenlen, 3) == 0)
            {
                if (dataEigen.m_id == 0)
                {
                    BeamEigen.Append(new GH_Integer(dataEigen.m_nr), new GH_Path(0));
                    BeamEigen.Append(new GH_Number(dataEigen.m_id), new GH_Path(1));
                    BeamEigen.Append(new GH_Number(dataEigen.m_x), new GH_Path(2));
                    BeamEigen.Append(new GH_Number(dataEigen.m_sdni), new GH_Path(3));
                    BeamEigen.Append(new GH_Number(dataEigen.m_sdvy), new GH_Path(4));
                    BeamEigen.Append(new GH_Number(dataEigen.m_sdvz), new GH_Path(5));
                    BeamEigen.Append(new GH_Number(dataEigen.m_sdmt), new GH_Path(6));
                    BeamEigen.Append(new GH_Number(dataEigen.m_sdmy), new GH_Path(7));
                    BeamEigen.Append(new GH_Number(dataEigen.m_sdmz), new GH_Path(8));
                    BeamEigen.Append(new GH_Number(dataEigen.m_srni), new GH_Path(9));
                    BeamEigen.Append(new GH_Number(dataEigen.m_srvy), new GH_Path(10));
                    BeamEigen.Append(new GH_Number(dataEigen.m_srvz), new GH_Path(11));
                    BeamEigen.Append(new GH_Number(dataEigen.m_srmt), new GH_Path(12));
                    BeamEigen.Append(new GH_Number(dataEigen.m_srmy), new GH_Path(13));
                    BeamEigen.Append(new GH_Number(dataEigen.m_srmz), new GH_Path(14));
                }

                dataEigenlen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_beam_crf));
            }

            // use sof_cdb_flush() and sof_cdb_close()
            sof_cdb_flush(index);

            // close the CDB
            sof_cdb_close(0);

        }

        public static void NodeDisp(ref GH_Structure<IGH_Goo> NodeDisplacement, int LC, string cdbpath)
        {

            int index = Index;
            int status = Status;
            string directory1 = Directory1;
            string directory2 = Directory2;
            // Get the path
            string path = Environment.GetEnvironmentVariable("path");

            // Set the new path environment variable + SOFiSTiK dlls path
            path = directory1 + ";" + directory2 + ";" + path;

            // Set the path variable (to read the data from CDB)
            System.Environment.SetEnvironmentVariable("path", path);


            // Connect to CDB, int sof_cdb_init  ( char* FileName, int Index);
            // Always use index 99, for more details see cdbase.chm
            index = sof_cdb_init(cdbpath, 99);


            // Check if sof_cdb_flush is working
            status = sof_cdb_status(index);

            // data as cs_node
            cs_n_dispc dataNode;

            // get the length of the structure
            int dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_n_dispc));

            // the kwh = 211, kwl = LC, pos = 1
            while (sof_cdb_get(index, 24, LC, &dataNode, ref dataNodelen, 3) == 0)
            {
                NodeDisplacement.Append(new GH_Integer(dataNode.m_id), new GH_Path(0));
                NodeDisplacement.Append(new GH_Number(dataNode.m_ux), new GH_Path(1));
                NodeDisplacement.Append(new GH_Number(dataNode.m_uy), new GH_Path(2));
                NodeDisplacement.Append(new GH_Number(dataNode.m_uz), new GH_Path(3));
                NodeDisplacement.Append(new GH_Number(dataNode.m_urx), new GH_Path(4));
                NodeDisplacement.Append(new GH_Number(dataNode.m_ury), new GH_Path(5));
                NodeDisplacement.Append(new GH_Number(dataNode.m_urz), new GH_Path(6));
                NodeDisplacement.Append(new GH_Number(dataNode.m_urb), new GH_Path(7));
                dataNodelen = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cs_n_dispc));
            }
            // use sof_cdb_flush() and sof_cdb_close()
            sof_cdb_flush(index);

            // close the CDB
            sof_cdb_close(0);
        }
    }
    }

