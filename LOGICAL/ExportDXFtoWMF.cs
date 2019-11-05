using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXFRendering.LOGICAL;
using Oxage.Wmf;
using Oxage.Wmf.Records;
using System.Drawing;

namespace DXFRenderingBitmap.WMF
{
    public class ExportDXFtoWMF
    {
        public static void exportCompleteDrawingStructToWMF(string in_pathToWMFFile, double in_scalefactor, DXFRendering.LOGICAL.completeDxfStruct in_dxfStruct)
        {
            var wmf = new WmfDocument();
            wmf.Width = 1000;
            wmf.Height = 1000;
            wmf.Format.Unit = 288;
            wmf.AddCreatePenIndirect(Color.Black, PenStyle.PS_SOLID, 1); //0: generic pen, black
            wmf.AddCreatePenIndirect(Color.Red, PenStyle.PS_SOLID, 1);   //1: PVC pen, red
            wmf.AddCreatePenIndirect(Color.Blue, PenStyle.PS_SOLID, 1);  //2: ALU pen, blue
            wmf.AddCreatePenIndirect(Color.Yellow, PenStyle.PS_SOLID, 1);//3: EXT Prof pen, yellow
            wmf.AddCreatePenIndirect(Color.Orange, PenStyle.PS_SOLID, 1);//4: Thermic bridge pen, orange

            wmf.AddDeleteObject(0);
            wmf.AddDeleteObject(1);
            wmf.AddDeleteObject(2);
            wmf.AddDeleteObject(3);
            wmf.AddDeleteObject(4);
            wmf.Save(in_pathToWMFFile);
        }
    }
}
