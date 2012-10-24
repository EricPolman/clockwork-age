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
    class AssetManager
    {
        public static string CONTENT_FOLDER = Environment.ExpandEnvironmentVariables("%CLKWRK%") + '\\';
        
        private static AssetManager _singleton;
        public static AssetManager Singleton { get { if (_singleton == null) { _singleton = new AssetManager(); } return _singleton; } }

        public List<GameObject> m_GameObjects = new List<GameObject>();
        public GraphicsDevice m_GraphicsDevice;

        public Dictionary<string, object> m_Assets = new Dictionary<string, object>();
        public Dictionary<object, string> m_AssetsInverse = new Dictionary<object, string>();


        public AssetManager()
        {

        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < m_GameObjects.Count; ++i)
            {
                m_GameObjects[i].Update(deltaTime);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < m_GameObjects.Count; ++i)
            {
                m_GameObjects[i].Draw();
                BoundingSphereRenderer.Render(m_GameObjects[i].m_BoundingSphere, m_GraphicsDevice, Camera.View, Camera.Projection, Color.Red);
            }
        }

        public string Export()
        {
            string result = "";

            /*foreach (BasicModel model in g_Models)
            {
                AssetInfo info;
                if (g_AssetTable.TryGetValue(model.name, out info))
                {
                    result += info.name + "|" + (model.m_vPosition.X + "x" + model.m_vPosition.Y + "x" + model.m_vPosition.Z) + "|||\r\n";
                }
            }*/

            return result;
        }
    }
}
