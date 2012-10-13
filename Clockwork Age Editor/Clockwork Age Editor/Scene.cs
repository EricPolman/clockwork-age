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
        public ModelManager modelManager;
        public ContentManager Content;
        public Camera camera;
        private Vector2 dimensions;
        public Vector2 Dimensions { get { return dimensions; } set { dimensions = value;} }
        GraphicsDevice graphicsDevice;
        string name = "Scenes/default.scene";

        public Scene(string name)
        {
            this.name = name;
            modelManager = ModelManager.Singleton;
            Clockwork_Age_Editor.Selector.Singleton.camera = camera;
        }
        public Scene()
        {
            modelManager = ModelManager.Singleton;
            name = ModelManager.env + name;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            PlaceModels(Content);
            camera = new Camera(new Vector3(0, 0, 40), Vector3.Zero, Vector3.Up, dimensions);
            this.Content = Content;
        }

        public void update(float deltaTime)
        {
            modelManager.update(deltaTime);
            camera.update(deltaTime);
        }

        public void draw()
        {
            modelManager.draw();
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

                Clockwork_Age_Editor.ModelManager.AssetInfo info;
                ModelManager.g_AssetTable.TryGetValue(assetName, out info);

                Model m;
                ModelManager.g_modelPrototypes.TryGetValue(info.name + "_model", out m);
                Texture2D tex;
                ModelManager.g_texturePrototypes.TryGetValue(info.name + "_diffuse", out tex);
                ModelManager.g_models.Add(new BasicModel(info.name, m, Content.Load<Effect>("Effects/Diffuse.fx"), tex, graphicsDevice, assetPosition));
            }

            sr.Close();
            fs.Close();
        }

        public void AddModel(string modelName)
        {
            Clockwork_Age_Editor.ModelManager.AssetInfo info;
            ModelManager.g_AssetTable.TryGetValue(modelName, out info);
            Model m;
            ModelManager.g_modelPrototypes.TryGetValue(info.name + "_model", out m);
            Texture2D tex;
            ModelManager.g_texturePrototypes.TryGetValue(info.name + "_diffuse", out tex);
            ModelManager.g_models.Add(new BasicModel(info.name, m, Content.Load<Effect>("Effects/Diffuse.fx"), tex, graphicsDevice, Vector3.Zero));
        }
    }
}
