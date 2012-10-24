using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Clockwork_Age_Editor;

namespace Clockwork_Age_Editor
{
    class Selector
    {
        private static Selector _singleton;
        public static Selector Singleton { get { if (_singleton == null) { _singleton = new Selector(); } return _singleton; } }
        MouseState m_MouseState, m_OldMouseState;

        public GraphicsDevice m_GraphicsDevice;
        public GameObject m_Selection;
        public Viewport m_Viewport;
        public Camera m_Camera;

        public Selector()
        {
            m_OldMouseState = Mouse.GetState();
            
        }

        public void LoadContent(GraphicsDevice gfx)
        {
            m_GraphicsDevice = gfx;
            m_Viewport = m_GraphicsDevice.Viewport;
        }

        public void update(float deltaTime)
        {
            m_OldMouseState = m_MouseState;
            m_MouseState = Mouse.GetState();
            m_Viewport = m_GraphicsDevice.Viewport;

            if (m_MouseState.LeftButton == ButtonState.Pressed && m_OldMouseState.LeftButton == ButtonState.Released && !Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            {
                if (m_MouseState.X - XnaViewControl.g_LocationX < 0 || m_MouseState.Y - XnaViewControl.g_LocationY < 0) return;

                Ray ray = Camera.GetMouseRay(new Vector2(m_MouseState.X - XnaViewControl.g_LocationX, m_MouseState.Y - XnaViewControl.g_LocationY), m_Viewport);
                GameObject temp = null;

                foreach (GameObject gameObject in AssetManager.Singleton.m_GameObjects)
                {
                    if (gameObject.m_BoundingSphere.Intersects(ray) != null)
                    {
                        temp = gameObject;
                    }
                }

                m_Selection = temp;
            }
        }
    }
}
