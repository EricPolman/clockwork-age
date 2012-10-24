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
        ContentManager contentManager;

        public static TreeView g_SceneView;

        public Form1()
        {
            InitializeComponent();
            Mouse.WindowHandle = xnaViewControl1.FindForm().Handle;
            xnaViewControl1.Focus();
            
            contentManager = new ContentManager(xnaViewControl1.Services, AssetManager.CONTENT_FOLDER + "Binaries/");

            //Application.Idle += delegate { Refresh(); };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            g_SceneView = treeView2;
            
            LoadAssets();
            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            xnaViewControl1.Width = Width - xnaViewControl1.Location.X - 30;
            xnaViewControl1.Height = Height - xnaViewControl1.Location.Y - 50;
            Camera.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)xnaViewControl1.Width / (float)xnaViewControl1.Height, 0.001f, 1000);
            //if (xnaViewControl1.GraphicsDevice != null)
            //Selector.Singleton.m_Viewport = new Viewport(xnaViewControl1.Location.X, xnaViewControl1.Location.Y, xnaViewControl1.Width, xnaViewControl1.Height);
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
                AssetManager.Singleton.m_GameObjects.Clear();

                Scene sc = CreateScene(fileDialog.FileName);
                sc.Dimensions = new Microsoft.Xna.Framework.Vector2(xnaViewControl1.Width, xnaViewControl1.Height);
                xnaViewControl1.LoadScene(contentManager, sc);
            }
        }

        private Scene CreateScene(string fileName)
        {
            //fileName = fileName.Replace("\\", "|");
            //string[] fileNameParts = fileName.Split('|');
            //string file = (fileNameParts[fileNameParts.Length - 1].Split('.'))[0];

            return new Scene(fileName);
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
            treeView1.Nodes.Add(new TreeNode("Assets"));
            LoadFilesOfDirectory(AssetManager.CONTENT_FOLDER + "Binaries/", treeView1.Nodes[0]);
        }

        void LoadFilesOfDirectory(string directory, TreeNode node)
        {
            foreach (string dir in Directory.GetDirectories(directory))
            {
                TreeNode newNode = new TreeNode(dir.Replace(directory, ""));
                node.Nodes.Add( newNode );
                LoadFilesOfDirectory(dir, newNode);
                
            }
            foreach (string file in Directory.GetFiles(directory))
            {
                string fileName = file; 
                fileName = fileName.Replace(".xnb", "");
                fileName = fileName.Replace(directory, "");
                if (fileName[0] == '\\')
                    fileName = fileName.Replace("\\", "");

                node.Nodes.Add(new TreeNode(fileName));
            }
        }

        

        private void xnaViewControl1_Click(object sender, EventArgs e)
        {
            xnaViewControl1.Focus();

        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {

        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Parent != null)
            {
                if (treeView1.SelectedNode.Parent.Text == "Models")
                {
                    xnaViewControl1.m_Scene.AddModel(treeView1.SelectedNode.Text);
                }
                else if (treeView1.SelectedNode.Parent.Text == "Textures")
                {
                    if (Selector.Singleton.m_Selection != null)
                    {
                        Selector.Singleton.m_Selection.m_Texture = contentManager.Load<Texture2D>(treeView1.SelectedNode.Parent.Text + "/" + treeView1.SelectedNode.Text);
                    }
                }
            }
        }

        private void treeView2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView2.SelectedNode != null)
            {
                Selector.Singleton.m_Selection = (GameObject)treeView2.SelectedNode;
            }
        }
    }
}
