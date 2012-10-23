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
        public AssetManager modelManager;
        public ContentManager Content;
        public Camera camera;
        private Vector2 dimensions;
        public Vector2 Dimensions { get { return dimensions; } set { dimensions = value;} }
        GraphicsDevice graphicsDevice;
        string name = "Scenes/default.scene";

        public Scene(string name)
        {
            this.name = name;
            modelManager = AssetManager.Singleton;
            Selector.Singleton.camera = camera;
        }
        public Scene()
        {
            modelManager = AssetManager.Singleton;
            name = AssetManager.CONTENT_FOLDER + name;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            PlaceModels(Content);
            camera = new Camera(new Vector3(0, 0, 40), Vector3.Zero, Vector3.Up, dimensions);
            this.Content = Content;
        }

        public void Update(float deltaTime)
        {
            modelManager.Update(deltaTime);
            camera.Update(deltaTime);
        }

        public void Draw()
        {
            modelManager.Draw();
        }

        public string Export()
        {
            return modelManager.Export();
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

                AssetManager.Singleton.m_Models.Add(new GameObject(assetName, Content.Load<Model>("Models/"+assetName), Content.Load<Effect>("Effects/Diffuse"), null, assetPosition));
            }

            sr.Close();
            fs.Close();
        }

        public void AddModel(string modelName)
        {

            AssetManager.Singleton.m_Models.Add(new GameObject(modelName, Content.Load<Model>(modelName), Content.Load<Effect>("Effects/Diffuse"), Content.Load<Texture2D>("Textures/brick1"), Vector3.Zero));
        }
    }
}
