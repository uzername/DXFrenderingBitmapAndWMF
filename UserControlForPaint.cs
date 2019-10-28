using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXFRenderingBitmap
{
    public partial class UserControlForPaint : UserControl
    {
        public Control realPaintingCanvas = new Control();
        private DXFRendering.GRAPHICAL.CompleteDxfDrawingStruct unscaledGraphicalData = new DXFRendering.GRAPHICAL.CompleteDxfDrawingStruct();
        private const int marginForDrawControl = 5;

        // how to scale the dxf display and how to rotate it
        /// <summary>
        /// the scale factor. do not squish value in percnt here, divide it by 100 beforehand
        /// </summary>
        public double currentscalefactor;
        /// <summary>
        /// angle value in radians goes here 
        /// </summary>
        public double currentanglevalue;

        private Bitmap bitmapInitial = null;
        private Bitmap bitmapRender = null;
        /// <summary>
        /// contains descriptions of layers found in dxf files and how to draw them. 
        /// String key is the layer name, tupple: byte for draw order, pen for a drawing Pen
        /// </summary>
        public Dictionary<String, Tuple<byte, Pen>> collectionOfLayerDefinitions = null;

        public void setupCollectionOfLayerDefinitions()
        {
            collectionOfLayerDefinitions = new Dictionary<string, Tuple<byte, Pen>>();
            Pen BluePenAlu = new Pen(new SolidBrush(Color.Blue), 3.0f);
            Pen YellowPenExtProf = new Pen(new SolidBrush(Color.DarkOrange), 1.0f);
            YellowPenExtProf.DashPattern = new float[] { 3.0f, 5.0f, 3.0f, 7.0f, 3.0f };
            Pen RedPenPvc = new Pen(new SolidBrush(Color.Red), 1.0f);
            Pen OrangePenThermalBridge = new Pen(new SolidBrush(Color.OrangeRed), 1.0f);
            Pen genericPen = new Pen(Color.Black);
            collectionOfLayerDefinitions.Add("EXT prof", new Tuple<byte, Pen>(1, YellowPenExtProf));
            collectionOfLayerDefinitions.Add("ALU", new Tuple<byte, Pen>(2, BluePenAlu));
            collectionOfLayerDefinitions.Add("PVC", new Tuple<byte, Pen>(3, RedPenPvc));
            collectionOfLayerDefinitions.Add("Thermal bridge", new Tuple<byte, Pen>(4, OrangePenThermalBridge));
            collectionOfLayerDefinitions.Add("0", new Tuple<byte, Pen>(5, genericPen));
            collectionOfLayerDefinitions.Add("AM_0", new Tuple<byte, Pen>(6, genericPen));
            collectionOfLayerDefinitions.Add("AM_1", new Tuple<byte, Pen>(7, genericPen));
        }

        public UserControlForPaint()
        {
            InitializeComponent();
            this.SuspendLayout();
            realPaintingCanvas.Left = 0 + marginForDrawControl;
            realPaintingCanvas.Top = 0 + marginForDrawControl;
            realPaintingCanvas.Parent = this;
            realPaintingCanvas.Width = this.Width - marginForDrawControl;
            realPaintingCanvas.Height = this.Height - marginForDrawControl;
            realPaintingCanvas.Padding = new Padding(marginForDrawControl);
            realPaintingCanvas.BackColor = Color.LightCyan;
            realPaintingCanvas.Paint += RealPaintingCanvas_Paint;
            this.ResumeLayout();
            this.currentscalefactor = 1.0;
            this.currentanglevalue = 0.0;
        }

        //step 0. get the file structure from dxf and gradually turn it into drawing struct
        public void prepareGraphicalDataStruct(DXFRendering.LOGICAL.completeDxfStruct in_structFromFile)
        {
            unscaledGraphicalData = new DXFRendering.GRAPHICAL.CompleteDxfDrawingStruct();
            int lngthOfStruct = in_structFromFile.getSize();
            for (int i = 0; i < lngthOfStruct; i++) { //read and convert entries from supplied struct to display struct with data ready to display
                DXFRendering.LOGICAL.DXFdrawingEntry someEntry = in_structFromFile.getItemByIndex(i);
                Pen usedPen = null;
                if ((collectionOfLayerDefinitions != null) && (collectionOfLayerDefinitions.ContainsKey(someEntry.layerIdentifier)))
                {
                    usedPen = collectionOfLayerDefinitions[someEntry.layerIdentifier].Item2;
                }
                else
                {
                    usedPen = new Pen(Color.Black);
                }
                DXFRendering.LOGICAL.MyDxfBoundingBox tmpboundbox = someEntry.GetBoundingBox();
                DXFRendering.GRAPHICAL.DXFentryForDisplay tmpEntry2 = null;
                if (someEntry is DXFRendering.LOGICAL.MyDxfLine) {
                    tmpEntry2 = new DXFRendering.GRAPHICAL.MyDxfLineForDisplay(
                        (someEntry as DXFRendering.LOGICAL.MyDxfLine).XStart,
                        (someEntry as DXFRendering.LOGICAL.MyDxfLine).YStart,
                        (someEntry as DXFRendering.LOGICAL.MyDxfLine).XEnd,
                        (someEntry as DXFRendering.LOGICAL.MyDxfLine).YEnd,usedPen);
                } else if (someEntry is DXFRendering.LOGICAL.MyDxfArc) {
                    DXFRendering.LOGICAL.MyDxfArc castArc = someEntry as DXFRendering.LOGICAL.MyDxfArc;
                    tmpEntry2 = new DXFRendering.GRAPHICAL.MyDxfArcForDisplay(castArc.XCenter, castArc.YCenter, castArc.Radius, castArc.StartAngleDegree, castArc.EndAngleDegree, usedPen);
                }
                // distilled it
                this.unscaledGraphicalData.addSingleEntry(tmpEntry2, tmpboundbox.XLowerLeft, tmpboundbox.YLowerLeft, tmpboundbox.XUpperRight, tmpboundbox.YUpperRight);
            }
        }
        //step 1. render dxf data from unscaledGraphicalData to bitmapInitial. Also initialize bitmapInitial. currentscalefactor should be set up beforehand
        public void drawImageToBitmapUsingCurrentScaleFactor()
        {
            double bitmapInitialWidth = Math.Abs(this.unscaledGraphicalData.XLowerLeft - this.unscaledGraphicalData.XUpperRight);
            double bitmapInitialHeight = Math.Abs(this.unscaledGraphicalData.YLowerLeft-this.unscaledGraphicalData.YUpperRight);
            //you know, dxf file might not be located at 0; 0 as lower Left. So I apply movement coefficients
            double unscaledOffsetX = this.unscaledGraphicalData.XLowerLeft;
            double unscaledOffsetY = this.unscaledGraphicalData.YLowerLeft;
            double newWidthPrescaled = currentscalefactor * bitmapInitialWidth;
            double newHeightPrescaled = currentscalefactor * bitmapInitialHeight;
            bitmapInitial = new Bitmap((int)newWidthPrescaled, (int)newHeightPrescaled);
            using (Graphics ee = Graphics.FromImage(bitmapInitial))
            {
                ee.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                ee.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                // RENDERING CODE GOES HERE !
                foreach (DXFRendering.GRAPHICAL.DXFentryForDisplay item in unscaledGraphicalData)
                {
                    if (item is DXFRendering.GRAPHICAL.MyDxfLineForDisplay)
                    {
                        DXFRendering.GRAPHICAL.MyDxfLineForDisplay lineEntry = (item as DXFRendering.GRAPHICAL.MyDxfLineForDisplay);
                        ee.DrawLine(lineEntry.penStructure, (float)((lineEntry.XStart-unscaledOffsetX)*currentscalefactor), (float)((lineEntry.YStart-unscaledOffsetY)*currentscalefactor), 
                            (float)((lineEntry.XEnd-unscaledOffsetX)*currentscalefactor), (float)((lineEntry.YEnd-unscaledOffsetY)*currentscalefactor));
                    } else if (item is DXFRendering.GRAPHICAL.MyDxfArcForDisplay)
                    {
                        DXFRendering.GRAPHICAL.MyDxfArcForDisplay arcEntry = (item as DXFRendering.GRAPHICAL.MyDxfArcForDisplay);
                        ee.DrawArc(arcEntry.penStructure,
                            (float)((arcEntry.XUpper - unscaledOffsetX)*currentscalefactor), (float)((arcEntry.YUpper-unscaledOffsetY)*currentscalefactor), 
                            (float)(arcEntry.Width*currentscalefactor), (float)(arcEntry.Height*currentscalefactor), 
                            (float)arcEntry.startAngle, (float)arcEntry.sweepAngle);
                    }
                }
            }

            


        }

        private Bitmap RotateImageUsingCurrentRotationAngle(Bitmap in_bitmapRender)
        {
            //First, re-center the image in a larger image that has a margin/frame
            //to compensate for the rotated image's increased size
            float angle = (float)currentanglevalue;
            Bitmap rotateMe = in_bitmapRender.Clone(new Rectangle(0,0,in_bitmapRender.Width, in_bitmapRender.Height),in_bitmapRender.PixelFormat);
            /*
            double ALPHA_DEG = currentanglevalue;
            double BETA_DEG = 90 - currentanglevalue;
            double ALPHA_RAD = ConvertDegreesToRadians(ALPHA_DEG);
            double BETA_RAD = ConvertDegreesToRadians(BETA_DEG);
            */
            double ALPHA_RAD = currentanglevalue;
            double BETA_RAD = Math.PI/2.0 - currentanglevalue;
            double newHeight = Math.Abs(Math.Abs(rotateMe.Height * Math.Sin(ALPHA_RAD)) + Math.Abs(rotateMe.Width * Math.Sin(BETA_RAD)));
            double newWidth = Math.Abs(Math.Abs(rotateMe.Height * Math.Cos(ALPHA_RAD)) + Math.Abs(rotateMe.Width * Math.Cos(BETA_RAD)));
            double offsetHeight = (newHeight - rotateMe.Height) / 2.0;
            double offsetWidth = (newWidth - rotateMe.Width) / 2.0;
            System.Diagnostics.Debug.Assert(newHeight >= 0);
            System.Diagnostics.Debug.Assert(newWidth >= 0);
            var bmp = new Bitmap((int)newWidth, (int)newHeight);

            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawImageUnscaled(rotateMe, (int)offsetWidth, (int)offsetHeight, bmp.Width, bmp.Height);

            rotateMe = bmp;

            //Now, actually rotate the image
            Bitmap rotatedImage = new Bitmap(rotateMe.Width, rotateMe.Height);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(rotateMe.Width / 2, rotateMe.Height / 2);   //set the rotation point as the center into the matrix
                g.RotateTransform(angle);                                        //rotate
                g.TranslateTransform(-rotateMe.Width / 2, -rotateMe.Height / 2); //restore rotation point into the matrix
                g.DrawImage(rotateMe, new Point(0, 0));                          //draw the image on the new bitmap
            }

            return rotatedImage;
        }
        // step 2. rotate bitmapInitial and render it to bitmapRender
        public void drawImageToBitmapUsingCurrentAngle()  {
            if (bitmapInitial != null)
            {
                bitmapRender = RotateImageUsingCurrentRotationAngle(bitmapInitial);
            }
        }

        private void RealPaintingCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (bitmapRender != null)
            {
                
                e.Graphics.DrawImage(bitmapRender, new Point(0, 0));
            }
        }

        private void UserControlForPaint_Resize(object sender, EventArgs e)
        {
            // https://stackoverflow.com/questions/19782208/refresh-scroll-bars-on-winform
            // hmmmm~ When scrollbarr are appearing on some dimension, 
            // it means that when user scrolls then Top or Left position may change and become <0 whether we want this or no
            bool relayoutNeeded = false;
            if (!(realPaintingCanvas.Left < 0))
            {
                int realPaintingCanvasLeft = this.Width / 2 - realPaintingCanvas.Width / 2;
                if (realPaintingCanvasLeft < 0) { realPaintingCanvas.Left = 0; relayoutNeeded = true; }
                else { realPaintingCanvas.Left = realPaintingCanvasLeft; }
            }
            if (!(realPaintingCanvas.Top < 0))
            {
                int realPaintingCanvasTop = this.Height / 2 - realPaintingCanvas.Height / 2;
                if (realPaintingCanvasTop < 0) { realPaintingCanvas.Top = 0; relayoutNeeded = true; }
                else { realPaintingCanvas.Top = realPaintingCanvasTop; }
            }
            if (relayoutNeeded) { this.PerformLayout(); }
        }
    }
}
