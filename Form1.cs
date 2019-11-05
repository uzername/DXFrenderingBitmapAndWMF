using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXFRenderingBitmap
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialogForDXF;
        SaveFileDialog saveFileDialogForDXForWMF;
        public Form1()
        {
            InitializeComponent();
            openFileDialogForDXF = new OpenFileDialog();
            openFileDialogForDXF.Filter = "DXF files (*.dxf)|*.dxf";
            saveFileDialogForDXForWMF = new SaveFileDialog();
            saveFileDialogForDXForWMF.Filter = "Bitmap Image|*.bmp|Wmf Image|*.wmf";

        }

        
        private void handleFile(String in_PathToDXF)
        {
            //retrieve the logical structure of dxf file
            DXFRendering.LOGICAL.completeDxfStruct obtainedStruct = DXFRendering.LOGICAL.DxfReadWrapper.processDxfFile(in_PathToDXF);
            DXFRendering.LOGICAL.MyDxfBoundingBox boundStruct = obtainedStruct.GetBoundingBox();
            //all these things are fine, but they are done in user control for drawing
            //get the graphical structure of file, prepared for display, with scale one-by-one (we apply scale factor later)
            //render the structure to bitmap using scale
            //rotate the bitmap (= not used in this edition) and render it to another bitmap which will be displayed every time we redraw control 
            userControlForPaint1.setupCollectionOfLayerDefinitions();
            userControlForPaint1.prepareGraphicalDataStruct(obtainedStruct);
            //calculate 
            double wdxf = Math.Abs(obtainedStruct.GetBoundingBox().XLowerLeft - obtainedStruct.GetBoundingBox().XUpperRight);
            double hdxf = Math.Abs(obtainedStruct.GetBoundingBox().YLowerLeft - obtainedStruct.GetBoundingBox().YUpperRight);
            double wimg = userControlForPaint1.Width;
            double himg = userControlForPaint1.Height;
            double scalew = wimg / wdxf; double scaleh = himg / hdxf;
            double scaleCurrent = Math.Min(scalew, scaleh);
            userControlForPaint1.currentscalefactor = scaleCurrent;
            
            userControlForPaint1.drawImageToBitmapUsingCurrentScaleFactor();
            this.userControlForPaint1.Refresh();
        }
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogForDXF.ShowDialog() == DialogResult.OK) {
                String filePath = openFileDialogForDXF.FileName;
                textBox1.Text = filePath;
                handleFile(filePath);
            }
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter)||(e.KeyCode == Keys.Tab))  {
                handleFile(textBox1.Text);
            }
        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            saveFileDialogForDXForWMF.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialogForDXForWMF.FileName != "")
            {
                String usedFname = saveFileDialogForDXForWMF.FileName;
                textBox2.Text = usedFname;
                
            }
        }

        private void buttonPerformSaving_Click(object sender, EventArgs e)
        {
            this.userControlForPaint1.saveGraphicalDataToFile(textBox2.Text);
        }
    }
}
