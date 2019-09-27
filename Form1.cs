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
        public Form1()
        {
            InitializeComponent();
        }

        private void FolderBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != DialogResult.Cancel)
            {
                String usedPathToShowFiles = folderBrowserDialog.SelectedPath;
                textBoxFolderPath.Text = usedPathToShowFiles;
                listBoxDxfFiles.DataSource = DXFRendering.LOGICAL.DxfReadWrapper.listAllDxfFilesInFolder(usedPathToShowFiles);
            }
        }

        private void textBoxFolderPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string usedPathToShowFiles = textBoxFolderPath.Text;
                listBoxDxfFiles.DataSource = DXFRendering.LOGICAL.DxfReadWrapper.listAllDxfFilesInFolder(usedPathToShowFiles);
            }
        }

        private void listBoxDxfFiles_SelectedValueChanged(object sender, EventArgs e)
        {
            DXFRendering.LOGICAL.singleDXFListBoxItem value = (DXFRendering.LOGICAL.singleDXFListBoxItem)this.listBoxDxfFiles.SelectedItem;
            //retrieve the logical structure of dxf file
            DXFRendering.LOGICAL.completeDxfStruct obtainedStruct = DXFRendering.LOGICAL.DxfReadWrapper.processDxfFile(value.fullPath);
            DXFRendering.LOGICAL.MyDxfBoundingBox boundStruct = obtainedStruct.GetBoundingBox();
            //all these things are fine, but they are done in user control for drawing
            //get the graphical structure of file, prepared for display, with scale one-by-one (we apply scale factor later)
            //render the structure to bitmap using scale
            //rotate the bitmap and render it to another bitmap which will be displayed every time we redraw control
            userControlForPaint1.prepareGraphicalDataStruct(obtainedStruct);
        }
    }
}
