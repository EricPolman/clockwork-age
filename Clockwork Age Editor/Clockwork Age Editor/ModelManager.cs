using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace Clockwork_Age_Editor
{
    class ModelManager
    {
        private static ModelManager _singleton;
        public static ModelManager Singleton { get { if (_singleton == null) { _singleton = new ModelManager(); } return _singleton; } }

        public static List<BasicModel> g_models = new List<BasicModel>();
        public static Dictionary<string, Model> g_modelPrototypes = new Dictionary<string, Model>();
        public static Dictionary<string, Texture2D> g_texturePrototypes = new Dictionary<string, Texture2D>();
        public static Dictionary<string, AssetInfo> g_AssetTable;

        public GraphicsDevice graphicsDevice;
        public static string env = Environment.ExpandEnvironmentVariables("%CLKWRK%") + '\\';
        Clockwork_Age_Editor.ContentBuilder contentBuilder = Clockwork_Age_Editor.ContentBuilder.Singleton;

        public struct AssetInfo
        {
            public string model;
            public string name;
            public string diffuseMap;
            public string normalMap;
            public string specularMap;
        }

        
        public ModelManager()
        {
            g_AssetTable = new Dictionary<string, AssetInfo>();
        }

        public void LoadContent(ContentManager Content)
        {
            Clockwork_Age_Editor.ContentBuilder.Singleton.Add(env + "Effects/Diffuse.fx", "Effects/Diffuse.fx", null, "EffectProcessor");
            BuildAssetDictionary(Content);
            
        }

        
        private void BuildAssetDictionary(ContentManager Content)
        {
            FileStream fs = new FileStream(env + "Data/all.assets", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs);

            string assetsData = sr.ReadToEnd();
            string[] assets = assetsData.Split('\n');
            for (int i = 0; i < assets.Length; ++i)
            {
                if (assets[i].Length == 0) continue;
                
                //compose assets
                string[] assetParts = assets[i].Split('|');

                AssetInfo assetInfo = new AssetInfo();
                assetInfo.name = assetParts[0];
                assetInfo.model = assetParts[1];
                assetInfo.diffuseMap = assetParts[2];
                if (assetParts[3] != "")
                    assetInfo.diffuseMap = assetParts[3];
                if (assetParts[4] != "")
                    assetInfo.diffuseMap = assetParts[4];
                g_AssetTable.Add(assetParts[0], assetInfo);

                Clockwork_Age_Editor.ContentBuilder.Singleton.Add(env + assetInfo.model, assetInfo.name + "_model", null, "ModelProcessor");
                Clockwork_Age_Editor.ContentBuilder.Singleton.Add(env + assetInfo.diffuseMap, assetInfo.name + "_diffuse", null, "TextureProcessor");

                string buildError = contentBuilder.Build();

                if (string.IsNullOrEmpty(buildError))
                {
                    g_modelPrototypes.Add(assetInfo.name + "_model", Content.Load<Model>(assetInfo.name + "_model"));
                    g_texturePrototypes.Add(assetInfo.name + "_diffuse", Content.Load<Texture2D>(assetInfo.name + "_diffuse"));

                }
                else
                {
                    // If the build failed, display an error message.
                    MessageBox.Show(buildError, "Error");
                }
            }

            sr.Close();
            fs.Close();
            
        }

        public void update(float deltaTime)
        {
            for (int i = 0; i < g_models.Count; ++i)
            {
                g_models[i].update(deltaTime);
            }
        }

        public void draw()
        {
            for (int i = 0; i < g_models.Count; ++i)
            {
                g_models[i].draw();
            }
        }

        public List<string> ListAssetTable()
        {
            List<KeyValuePair<string, AssetInfo>> list = g_AssetTable.ToList();
            List<string> strings = new List<string>();
            foreach (KeyValuePair<string, AssetInfo> kvp in list)
            {
                strings.Add(kvp.Key);
            }

            return strings;
        }

        public string Export()
        {
            string result = "";

            foreach (BasicModel model in g_models)
            {
                AssetInfo info;
                if (g_AssetTable.TryGetValue(model.name, out info))
                {
                    result += info.name + "|" + (model.m_vPosition.X + "x" + model.m_vPosition.Y + "x" + model.m_vPosition.Z) + "|||\r\n";
                }
            }

            return result;
        }
    }
}
