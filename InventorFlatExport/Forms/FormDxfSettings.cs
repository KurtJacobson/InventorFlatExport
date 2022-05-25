using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace InventorFlatExport
{
    public partial class FormDxfSettings : Form
    {
        public FormDxfSettings()
        {
            InitializeComponent();

            var settings = Properties.DxfSettings.Default;

            // Initialize values
            this.tbOuterProfileLayer.Text = settings.OuterProfileLayer;
            this.cbOuterProfileLineColor.SelectedColor = settings.OuterProfileLayerColor;
            //this.cbOuterProfileLineType.SelectedText = settings.OuterProfileLineType;

        }

        public object GetDxfSetting(string name)
        {
            // Get DXF settings
            return Properties.DxfSettings.Default[name];
        }

        public void SetDxfSetting(string name, object value)
        {
            // Set DXF settings
            Properties.DxfSettings.Default[name] = value;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // Commit changes to the settings
            var settings = Properties.DxfSettings.Default;

            settings.OuterProfileLayer = this.tbOuterProfileLayer.Text;
            settings.OuterProfileLayerColor = this.cbOuterProfileLineColor.SelectedColor;

            // Save settings
            settings.Save();
            
            // Close the settings window
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Close window without saving anything
            this.Close();
        }
    }
}
