// MetalForming Inc.
// Copyright (c) 2022 All Rights Reserved
// Author: Kurt Jacobson
// Date: 04/28/2022


using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using Inventor;
using IxMilia;
using IxMilia.Dxf;
using IxMilia.Dxf.Entities;


namespace InventorFlatExport
{
    public partial class FormMainWindow : Form
    {

        Inventor.Application inventor = null;
        IDictionary<string, SheetmetalPart> partDict = new Dictionary<string, SheetmetalPart>();

        public FormMainWindow()
        {
            InitializeComponent();

            this.labelPartPath.Text = "No Part Selected";
            this.labelPartOccur.Text = "0";
            this.buttonExportDxf.Enabled = false;
        }

        private void buttonRefreshView_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            InitInventor(btn);
        }

        private string colorToRgb(System.Drawing.Color color)
        {
            return String.Format("{0};{1};{2}", color.R, color.G, color.B);    
        }


        private void InitInventor(Button button)
        {

            PartDocument oDoc = null;

            try
            {
                inventor = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Make sure AutoDesk Inventor is installed and running.", "Inventor Error!");
                return;
            }
            
            PopulateTreeView();

            //// export in new thread
            //oDoc = (PartDocument)inventor.ActiveDocument;
            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            //    ExportFlatDXF(oDoc);
            //}).Start();
        }

        private void PopulateTreeView()
        {
            if (inventor == null) return;

            Document actDoc = inventor.ActiveDocument;
            if (actDoc == null) return;

            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();

            string partId = null;

            // active document is a inventor part
            if (actDoc.DocumentType == DocumentTypeEnum.kPartDocumentObject)
            {
                PartDocument doc = (PartDocument)actDoc;
                partId = actDoc.InternalName;
                TreeNode newNode = treeView1.Nodes.Add(doc.DisplayName);
                newNode.Name = partId;
                partDict[partId] = new SheetmetalPart(doc);

                doc.DocumentEvents.OnClose += DocumentEvents_OnClose;
            }


            // active document is an inventor assembly
            if (actDoc.DocumentType == DocumentTypeEnum.kAssemblyDocumentObject)
            {
                AssemblyDocument asmDoc = (AssemblyDocument)actDoc;
                AssemblyComponentDefinition asmDeff = asmDoc.ComponentDefinition;


                ComponentOccurrencesEnumerator leafOcc = asmDeff.Occurrences.AllLeafOccurrences;

                TreeNode node = treeView1.Nodes.Add(asmDoc.DisplayName);


                foreach (ComponentOccurrence occ in leafOcc)
                {
                    PartDocument doc = occ.Definition.Document;

                    partId = doc.InternalName;

                    if (partDict.ContainsKey(partId))
                    {
                        partDict[partId].PartOccurences++;
                    }
                    else
                    {
                        TreeNode newNode = node.Nodes.Add(doc.DisplayName);
                        newNode.Name = partId;
                        partDict[partId] = new SheetmetalPart(doc);

                        doc.DocumentEvents.OnClose += DocumentEvents_OnClose;
                    }
                }
            }

            treeView1.EndUpdate();
            treeView1.ExpandAll();
        }

        private void ExportFlatDXF(PartDocument oDoc)
        {
            SheetMetalComponentDefinition smCompDef = null;

            if (oDoc == null)
            {
                MessageBox.Show("Part document is null.", "Export error!");
            }

            if (oDoc.DocumentType == DocumentTypeEnum.kPartDocumentObject)
            {
                Console.WriteLine("Is Inventor Part document");

                if (oDoc.SubType == "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}")
                {
                    Console.WriteLine("Is sheet metal component!");

                    // we MUST class as a sheet metal comp def
                    smCompDef = (SheetMetalComponentDefinition)oDoc.ComponentDefinition;
                }
                else
                {
                    MessageBox.Show("The selected item is not a valid Sheetmetal component.", "Export Error!");
                    return;
                }

                // Unfold if we don't already have a flat pattern
                if (!smCompDef.HasFlatPattern)
                {
                    try
                    {
                        smCompDef.Unfold();
                    }
                    catch
                    {
                        MessageBox.Show("Inventor encountered an error while unfolding the part.\n"
                                      + "Please correct the issue with the model and try again.", "Unfold error!");
                        return;
                    }
                }

                // set export length units
                UnitsOfMeasure originalUnits = oDoc.UnitsOfMeasure;
                oDoc.UnitsOfMeasure.LengthUnits = UnitsTypeEnum.kInchLengthUnits;

                FlatPattern fPatt = smCompDef.FlatPattern;
                //fPatt.Edit();

                string sOut = "FLAT PATTERN DXF?AcadVersion=2007"
                            // Outer Profile Layer
                            + "&OuterProfileLayer=" + Properties.DxfSettings.Default.OuterProfileLayer
                            + "&OuterProfileLayerColor=" + colorToRgb(Properties.DxfSettings.Default.OuterProfileLayerColor)
                            + "&OuterProfileLineType=" + ((decimal)LineTypeEnum.kDefaultLineType)

                            // Interior Profile Layer
                            + "&InteriorProfilesLayer=InnerOutlines"
                            + "&InteriorProfilesLayerColor=" + colorToRgb(Properties.DxfSettings.Default.InteriorProfilesLayerColor)
                            + "&InteriorProfilesLineType=" + ((decimal)LineTypeEnum.kDefaultLineType)

                            // Bend Up Layer
                            //+ "&BendUpLayer=not_used"
                            //+ "&BendUpLayerColor=0;0;255"
                            //+ "&BendUpLineType=" + ((decimal)LineTypeEnum.kDashedLineType)

                            // Bend Down Layer
                            //+ "&BendDownLayer=not_used"
                            //+ "&BendDownLayerColor=255;0;191"
                            //+ "&BendDownLineType=" + ((decimal)LineTypeEnum.kDashedLineType)

                            // Layers to hide
                            + "&InvisibleLayers=IV_TANGENT;IV_ARC_CENTERS;IV_BEND;IV_BEND_DOWN;IV_UNCONSUMED_SKETCHES;not_used"

                            // Other Export Settings
                            //+ "&MergeProfilesIntoPolyline=True"
                            //+ "&SimplifySplines=True"
                            //+ "&AdvancedLegacyExport=True"
                            //+ "&SplineTolerance=0.01"
                            ;

                string dxfOutName = System.IO.Path.GetFileNameWithoutExtension(oDoc.DisplayName) + ".dxf";
                //string dxfOutDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                string dxfOutDir = System.IO.Path.GetDirectoryName(oDoc.FullFileName);
                string dxfOutPath = System.IO.Path.Combine(dxfOutDir, dxfOutName);

                DataIO oDataIO = oDoc.ComponentDefinition.DataIO;
                oDataIO.WriteDataToFile(sOut, dxfOutPath);

                UnitsOfMeasure docUnits = oDoc.UnitsOfMeasure;
                string uString = docUnits.GetStringFromType(docUnits.LengthUnits);


                // read DXF and add bend lines
                var file = DxfFile.Load(dxfOutPath);
                file.Header.Version = DxfAcadVersion.R2007;
                file.ApplicationIds.Add(new DxfAppId("POS3000_V3_PRODUCT"));
                file.ApplicationIds.Add(new DxfAppId("POS3000_V3_BENDINGLINE"));

                // convert from internal units (CM) to document units
                double toDocUnits(double value) {
                    return docUnits.ConvertUnits(value, UnitsTypeEnum.kCentimeterLengthUnits, docUnits.LengthUnits);
                }

                // get sheet thickness from model
                var sheetThickness = toDocUnits(smCompDef.Thickness.Value);
                var materialName = smCompDef.Material.Name;

                // ToDo: Find a better way to get the Outline layer
                foreach (DxfLayer layer in file.Layers) {

                    if (layer.Name == "Outline")
                    {
                        // add XData with product info
                        layer.XData["POS3000_V3_PRODUCT"] = new DxfXDataApplicationItemCollection
                        (
                            new DxfXDataString(String.Format("Thickness={0:F4}", sheetThickness)),
                            new DxfXDataString(String.Format("MaterialId=({0})", materialName))
                        );
                    }
                }

                var bUpLineColor = Properties.DxfSettings.Default.BendUpLayerColor.ToArgb();
                var bDownLineColor = Properties.DxfSettings.Default.BendDownLayerColor.ToArgb();
                var bLineType = "Dashed";

                // Add dashed line type to DXF
                var dashedLineType = new DxfLineType("test");
                dashedLineType.Elements.Add(new DxfLineTypeElement() { DashDotSpaceLength = .5});
                dashedLineType.Elements.Add(new DxfLineTypeElement() { DashDotSpaceLength = .25});
                file.LineTypes.Add(dashedLineType);

                foreach (FlatBendResult oBend in fPatt.FlatBendResults) {

                    // we only want bends on the top face, so continue if on bottom face
                    if (oBend.IsOnBottomFace) continue;

                    var bAngle = oBend.Angle * 180 / Math.PI;
                    var bRadius = toDocUnits(oBend.InnerRadius);

                    var bLineColor = bUpLineColor;

                    // down bends have a negative bend angle, and different color
                    if (oBend.IsDirectionUp)
                    {
                        bAngle *= -1.0;
                        bLineColor = bDownLineColor;
                    }

                    // different line type for regular bends and hems
                    if (Math.Abs(bAngle) > 179 )
                    {
                        bLineType = "Divide";
                    }
                    else
                    {
                        bLineType = "Dashed";
                    }

                    Point startPoint = oBend.Edge.StartVertex.Point;
                    Point stopPoint = oBend.Edge.StopVertex.Point;

                    // bend start and end points
                    var x1 = toDocUnits(startPoint.X);
                    var y1 = toDocUnits(startPoint.Y);
                    var x2 = toDocUnits(stopPoint.X);
                    var y2 = toDocUnits(stopPoint.Y);

                    // shorten bend lines
                    var d = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

                    // new starting point
                    var t = 0.1 / d;
                    x1 = (1 - t) * x1 + t * x2;
                    y1 = (1 - t) * y1 + t * y2;

                    // new stopping point
                    t = (d - 0.1) / d;
                    x2 = (1 - t) * x1 + t * x2;
                    y2 = (1 - t) * y1 + t * y2;

                    // create new line on BENDLINES layer
                    var bLine = new DxfLine(new DxfPoint(x1, y1, 0.0), new DxfPoint(x2, y2, 0.0));
                    bLine.LineTypeName = bLineType;
                    bLine.Color24Bit = bLineColor;
                    bLine.Layer = "BendingLines";

                    // add XData with bend info
                    bLine.XData["POS3000_V3_BENDINGLINE"] = new DxfXDataApplicationItemCollection(
                        new DxfXDataString(String.Format("BendAngleDeg={0:F3}", bAngle)),
                        new DxfXDataString(String.Format("InnerRadius={0:F4}", bRadius)),
                        new DxfXDataString(String.Format("K_Factor={0:F3}", oBend.kFactor))
                        //new DxfXDataString(String.Format("BendShortening=-.5"))
                    );

                    // add bend line to DXF
                    file.Entities.Add(bLine);
                }

                // safe DXF file
                file.Save(dxfOutPath);

                // restore original units of measure
                oDoc.UnitsOfMeasure.LengthUnits = originalUnits.LengthUnits;
            }
        }

        private void DocumentEvents_OnClose(EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        {
            MessageBox.Show("The document was closed");
            throw new NotImplementedException();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (partDict.TryGetValue(e.Node.Name, out SheetmetalPart part))
            {
                this.labelPartPath.Text = part.FileName;
                this.labelPartOccur.Text = part.PartOccurences.ToString();
                this.buttonExportDxf.Enabled = true;
            }
            else
            {
                this.labelPartPath.Text = "No Part Selected";
                this.labelPartOccur.Text = "0";
                this.buttonExportDxf.Enabled = false;
            }
        }

        private void buttonExportDxf_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            if (selectedNode == null) return;

            if (partDict.TryGetValue(selectedNode.Name, out SheetmetalPart part))
            {
               ExportFlatDXF(part.Document);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // close out the form
            this.Close();
        }

        private void dXFExportSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDxfSettings settingsForm = new FormDxfSettings();
            settingsForm.ShowDialog();
        }
    }
}


public class SheetmetalPart
{
    private PartDocument doc = null;
    private int occurances = 1;

    public SheetmetalPart(PartDocument doc)
    {
        this.doc = doc;
    }

    public PartDocument Document
    {
        get { return this.doc; }
    }

    public string Name
    {
        get { return this.doc.DisplayName; }
    }

    public string FileName
    {
        get { return this.doc.FullFileName; }
    }

    public string MaterialThikness
    {
        get { return this.doc.FullFileName; }
    }

    public int PartOccurences
    {
        get { return this.occurances; }
        set { this.occurances = value; }
    }
}
