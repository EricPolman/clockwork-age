using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Clockwork_Age_Editor
{
    public partial class Form1 : Form
    {
        ContentBuilder contentBuilder;
        ContentManager contentManager;

        public Form1()
        {
            InitializeComponent();
            Mouse.WindowHandle = xnaViewControl1.FindForm().Handle;
            xnaViewControl1.Focus();
            
            contentBuilder = ContentBuilder.Singleton;
            contentManager = new ContentManager(xnaViewControl1.Services, contentBuilder.OutputDirectory);
            //Application.Idle += delegate { Refresh(); };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadAssets();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            xnaViewControl1.Width = Width - xnaViewControl1.Location.X - 30;
            xnaViewControl1.Height = Height - xnaViewControl1.Location.Y - 50;
            Camera.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)xnaViewControl1.Width / (float)xnaViewControl1.Height, 0.001f, 1000);
        }

        /// <summary>
        /// Event handler for the Exit menu option.
        /// </summary>
        void ExitMenuClicked(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Event handler for the Open menu option.
        /// </summary>
        void OpenMenuClicked(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Scene";

            fileDialog.Filter = "Scene Files (*.scene)|*.scene";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                Clockwork_Age_Editor.ModelManager.g_models.Clear();

                Clockwork_Age_Editor.Scene sc = CreateScene(fileDialog.FileName);
                sc.Dimensions = new Microsoft.Xna.Framework.Vector2(xnaViewControl1.Width, xnaViewControl1.Height);
                xnaViewControl1.LoadScene(contentManager, sc);
            }
        }

        private Clockwork_Age_Editor.Scene CreateScene(string fileName)
        {
            //fileName = fileName.Replace("\\", "|");
            //string[] fileNameParts = fileName.Split('|');
            //string file = (fileNameParts[fileNameParts.Length - 1].Split('.'))[0];

            return new Clockwork_Age_Editor.Scene(fileName);
        }


        /// <summary>
        /// Loads a new 3D model file into the ModelViewerControl.
        /// </summary>

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "../../../../Content");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Model";

            fileDialog.Filter = "Model Files (*.fbx;*.x)|*.fbx;*.x|" +
                                "FBX Files (*.fbx)|*.fbx|" +
                                "X Files (*.x)|*.x|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //LoadModel(fileDialog.FileName);
            }
        }

        private void sceneFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Scene file|*.scene";
            saveDialog.Title = "Export the current scene";
            saveDialog.ShowDialog();

            if (saveDialog.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveDialog.OpenFile();
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(xnaViewControl1.m_Scene.Export());
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        private void LoadAssets()
        {
            xnaViewControl1.LoadContent(contentManager);
            List<string> strings = Clockwork_Age_Editor.ModelManager.Singleton.ListAssetTable();
            foreach (string s in strings)
            {
                listBox1.Items.Add(s);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem != null)
                if (listBox1.SelectedItem.ToString().Length > 0)
                    xnaViewControl1.m_Scene.AddModel(listBox1.SelectedItem.ToString());
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void xnaViewControl1_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            xnaViewControl1.Focus();

        }
    }
}
