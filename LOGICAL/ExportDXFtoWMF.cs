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
        /// <summary>
        /// returns number of pen to use in wmf file for some figure from dxf based on its layer. Used in exportCompleteDrawingStructToWMF
        /// </summary>
        /// <param name="in_name"></param>
        /// <returns>some integer. If it is less zero then no brush is selected and entry is about to be discarded</returns>
        private static int getindexOfPenByLayerName(String in_name) {
            int retvalue = 0;
            switch (in_name.ToLower())
            {
                case "pvc":
                    { retvalue = 1; break; }
                case "alu":
                    { retvalue = 2; break; }
                case "ext prof":
                case "ext prof.":
                    { /*retvalue = 3;*/ retvalue = -1; break; }
                case "termal bridge":
                case "termic bridge":
                case "thermal bridge":
                    { /*retvalue = 4;*/ retvalue = -1; break; }

                default:
                    break;
            }
            return retvalue;
        }
        public static void exportCompleteDrawingStructToWMF(string in_pathToWMFFile, double in_scalefactor, DXFRendering.LOGICAL.completeDxfStruct in_dxfStruct)
        {
            MyDxfBoundingBox theBoundingBox = in_dxfStruct.GetBoundingBox();
            double bitmapInitialWidth = Math.Abs(theBoundingBox.XLowerLeft - theBoundingBox.XUpperRight);
            double bitmapInitialHeight = Math.Abs(theBoundingBox.YLowerLeft - theBoundingBox.YUpperRight);
            //you know, dxf file might not be located at 0; 0 as lower Left. So I apply movement coefficients
            double unscaledOffsetX = theBoundingBox.XLowerLeft;
            double unscaledOffsetY = theBoundingBox.YLowerLeft;
            double newWidthPrescaled = in_scalefactor * bitmapInitialWidth;
            double newHeightPrescaled = in_scalefactor * bitmapInitialHeight;
            int wdth = (int)Math.Ceiling(newWidthPrescaled)+1 ;
            int hght = (int)Math.Ceiling(newHeightPrescaled)+1;

            var wmf = new WmfDocument();
            wmf.Width = wdth;
            wmf.Height = hght;
            //https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-wmf/828e1864-7fe7-42d8-ab0a-1de161b32f27
            // 1440 ?
            wmf.Format.Unit = 288;
            wmf.AddCreatePenIndirect(Color.Black, PenStyle.PS_SOLID, 1); //0: generic pen, black
            wmf.AddCreatePenIndirect(Color.Red, PenStyle.PS_SOLID, 1);   //1: PVC pen, red
            wmf.AddCreatePenIndirect(Color.Blue, PenStyle.PS_SOLID, 1);  //2: ALU pen, blue
            wmf.AddCreatePenIndirect(Color.Yellow, PenStyle.PS_SOLID, 1);//3: EXT Prof pen, yellow
            wmf.AddCreatePenIndirect(Color.Orange, PenStyle.PS_SOLID, 1);//4: Thermic bridge pen, orange
            
            int allElementsLength = in_dxfStruct.getSize();
            // similar to prepareGraphicalDataStruct
            for (int i = 0; i < allElementsLength; i++)
            {
                DXFRendering.LOGICAL.DXFdrawingEntry someEntry = in_dxfStruct.getItemByIndex(i);
                // I use y candidates to perform flipping vertically. It is possible to redirect Y axy, but that WMF entry is barely supported
                if (someEntry is DXFRendering.LOGICAL.MyDxfLine)
                {
                    MyDxfLine lineEntry = someEntry as MyDxfLine;
                    int indx = getindexOfPenByLayerName(lineEntry.layerIdentifier);
                    if (indx >= 0)
                    {
                        wmf.AddSelectObject(indx);
                        int y1candidate = (int)Math.Ceiling((lineEntry.YStart - unscaledOffsetY) * in_scalefactor);
                        int y2candidate = (int)Math.Ceiling((lineEntry.YEnd - unscaledOffsetY) * in_scalefactor);
                        wmf.AddLine(
                        new Point(
                            (int)Math.Ceiling((lineEntry.XStart - unscaledOffsetX) * in_scalefactor), 
                            -y1candidate + hght ),
                        new Point(
                            (int)Math.Ceiling((lineEntry.XEnd - unscaledOffsetX) * in_scalefactor), 
                            -y2candidate+hght)
                        );
                    }
                } else if (someEntry is DXFRendering.LOGICAL.MyDxfArc)  {
                    DXFRendering.LOGICAL.MyDxfArc castArc = someEntry as DXFRendering.LOGICAL.MyDxfArc;
                    //https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-wmf/742097b4-5879-4c36-b57e-77e7cc152253
                    int indx = getindexOfPenByLayerName(castArc.layerIdentifier);
                    if (indx >= 0)
                    {
                        wmf.AddSelectObject(indx);
                        int yRectangleCandidate = (int)Math.Ceiling((castArc.YCenter - castArc.Radius - unscaledOffsetY) * in_scalefactor);
                        int dimCandidate = (int)Math.Ceiling(2 * castArc.Radius * in_scalefactor);
                        Rectangle boundRectangle = new Rectangle(
                            (int)Math.Ceiling((castArc.XCenter - castArc.Radius - unscaledOffsetX) * in_scalefactor),
                            -yRectangleCandidate+hght - dimCandidate ,
                            dimCandidate ,
                            dimCandidate);
                        int yPnt1Candidate = (int)Math.Ceiling((castArc.YCenter + castArc.Radius * Math.Sin(castArc.StartAngleRad) - unscaledOffsetY) * in_scalefactor);
                        Point pnt1 = new Point(
                            (int)Math.Ceiling((castArc.XCenter + castArc.Radius * Math.Cos(castArc.StartAngleRad) - unscaledOffsetX) * in_scalefactor),
                            -yPnt1Candidate + hght
                            );
                        int yPnt2Candidate = (int)Math.Ceiling((castArc.YCenter + castArc.Radius * Math.Sin(castArc.EndAngleRad) - unscaledOffsetY) * in_scalefactor);
                        Point pnt2 = new Point(
                            (int)Math.Ceiling((castArc.XCenter + castArc.Radius * Math.Cos(castArc.EndAngleRad) - unscaledOffsetX) * in_scalefactor),
                            -yPnt2Candidate+hght
                            );
                        wmf.AddArc(boundRectangle, pnt1, pnt2);
                        /*
                        wmf.AddRectangle(boundRectangle);
                        wmf.AddSelectObject(3);
                        wmf.AddLine(pnt1, pnt2);
                        */
                    }
                }
            }
            wmf.AddDeleteObject(0);
            wmf.AddDeleteObject(1);
            wmf.AddDeleteObject(2);
            wmf.AddDeleteObject(3);
            wmf.AddDeleteObject(4);
            wmf.Save(in_pathToWMFFile);
        }
    }
}
