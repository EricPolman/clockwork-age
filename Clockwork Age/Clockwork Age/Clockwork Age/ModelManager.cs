using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.IO;


namespace Clockwork_Age
{
    class ModelManager
    {
        List<BasicModel> models = new List<BasicModel>();
        GraphicsDevice graphicsDevice;
        string sceneName = "default";

        public ModelManager(GraphicsDevice graphicsDevice, string sceneName)
        {
            this.graphicsDevice = graphicsDevice;
            this.sceneName = sceneName;
        }

        public void LoadContent(ContentManager Content)
        {
            //models.Add(new BasicModel(Content.Load<Model>("eleckast_mesh"), Content.Load<Effect>("Effects/Diffuse"), Content.Load<Texture2D>("eleckast"), graphicsDevice));
            FileStream fs = new FileStream("Content/Scenes/testscene.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs);
            
            string scenedata = sr.ReadToEnd();
            string[] assets = scenedata.Split('\n');
            for (int i = 0; i < assets.Length; ++i)
            {
                if (assets[i].Length == 0) continue;
                string[] assetProperties = assets[i].Split('|');
                string[] strPos = assetProperties[4].Split('x');
                Vector3 assetPosition = new Vector3(float.Parse(strPos[0]),float.Parse(strPos[1]),float.Parse(strPos[2]));
                
                models.Add(new BasicModel(Content.Load<Model>(assetProperties[0]), Content.Load<Effect>("Effects/Diffuse"), Content.Load<Texture2D>(assetProperties[1]), graphicsDevice, assetPosition));
            }
        }

        public void update(float deltaTime)
        {
            for (int i = 0; i < models.Count; ++i)
            {
                models[i].update(deltaTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            
            for (int i = 0; i < models.Count; ++i)
            {
                models[i].draw(spriteBatch);
            }
        }
    }
}
