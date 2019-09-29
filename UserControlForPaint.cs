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
        public double currentscalefactor;
        public double currentanglevalue;

        private Bitmap bitmapInitial;
        private Bitmap bitmapRender;
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
        //step 1. render dxf data from unscaledGraphicalData to bitmapInitial. Also initialize bitmapInitial
        public void drawImageToBitmapUsingCurrentScaleFactor()
        {
            double bitmapInitialWidth = Math.Abs(this.unscaledGraphicalData.XLowerLeft - this.unscaledGraphicalData.XUpperRight);
            double bitmapInitialHeight = Math.Abs(this.unscaledGraphicalData.YLowerLeft-this.unscaledGraphicalData.YUpperRight);
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
                        ee.DrawLine(lineEntry.penStructure, (float)lineEntry.XStart, (float)lineEntry.YStart, (float)lineEntry.XEnd, (float)lineEntry.YEnd);
                    } else if (item is DXFRendering.GRAPHICAL.MyDxfArcForDisplay)
                    {
                        DXFRendering.GRAPHICAL.MyDxfArcForDisplay arcEntry = (item as DXFRendering.GRAPHICAL.MyDxfArcForDisplay);
                        ee.DrawArc(arcEntry.penStructure,(float)arcEntry.XUpper, (float)arcEntry.YUpper, (float)arcEntry.Width, (float)arcEntry.Height, (float)arcEntry.startAngle, (float)arcEntry.sweepAngle);
                    }
                }
            }
        }

        private void RealPaintingCanvas_Paint(object sender, PaintEventArgs e)
        {

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
