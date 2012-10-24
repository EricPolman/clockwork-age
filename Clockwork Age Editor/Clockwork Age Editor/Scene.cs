using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace Clockwork_Age_Editor
{
    class Scene
    {
        public AssetManager assetManager;
        public ContentManager Content;
        public Camera camera;
        private Vector2 dimensions;
        public Vector2 Dimensions { get { return dimensions; } set { dimensions = value;} }
        GraphicsDevice graphicsDevice;
        string name = "Scenes/default.scene";
        SampleGrid grid;

        public Scene(string name)
        {
            this.name = name;
            assetManager = AssetManager.Singleton;
            Selector.Singleton.camera = camera;
        }
        public Scene()
        {
            assetManager = AssetManager.Singleton;
            name = AssetManager.CONTENT_FOLDER + name;

        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            PlaceModels(Content);
            camera = new Camera(new Vector3(0, 0, 40), Vector3.Zero, Vector3.Up, dimensions);
            this.Content = Content;
            grid = new SampleGrid();
            grid.GridColor = Color.White;
            grid.GridScale = 5f;
            grid.GridSize = 32;
            // Set the grid to draw on the x/z plane around the origin
            grid.WorldMatrix = Matrix.Identity;
            grid.ProjectionMatrix = Camera.Projection;
            grid.LoadGraphicsContent(graphicsDevice);
            
        }

        public void Update(float deltaTime)
        {
            grid.ViewMatrix = Camera.View;
            assetManager.Update(deltaTime);
            camera.Update(deltaTime);
        }

        public void Draw()
        {
            grid.Draw();
            assetManager.Draw();
        }

        public string Export()
        {
            return assetManager.Export();
        }

        public void PlaceModels(ContentManager Content)
        {
            
            FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.None); 
            
            StreamReader sr = new StreamReader(fs);

            string scenedata = sr.ReadToEnd();
            string[] assets = scenedata.Split('\n');
            for (int i = 0; i < assets.Length; ++i)
            {
                if (assets[i].Length == 0) continue;

                string[] assetProperties = assets[i].Split('|');
                string[] strPos = assetProperties[1].Split('x');

                string assetName = assetProperties[0];
                Vector3 assetPosition = new Vector3(float.Parse(strPos[0]), float.Parse(strPos[1]), float.Parse(strPos[2]));

                AssetManager.Singleton.m_GameObjects.Add(new GameObject(assetName, Content.Load<Model>("Models/"+assetName), Content.Load<Effect>("Effects/Diffuse"), null, assetPosition));
            }

            sr.Close();
            fs.Close();
        }

        public void AddModel(string modelName)
        {
            AssetManager.Singleton.m_GameObjects.Add(new GameObject(modelName, Content.Load<Model>("Models/"+modelName), Content.Load<Effect>("Effects/Diffuse"), null, camera.m_vTarget));
        }
    }
}
