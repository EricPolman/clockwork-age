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
        MouseState mouseState, oldMouseState;
        public GraphicsDevice graphicsDevice;
        public GameObject selection;
        Viewport viewport;
        public Camera camera;

        public Selector()
        {
            oldMouseState = Mouse.GetState();
            
        }

        public void LoadContent(GraphicsDevice gfx)
        {
            graphicsDevice = gfx;
            viewport = graphicsDevice.Viewport;
        }

        public void update(float deltaTime)
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && !Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            {
                if (mouseState.X - XnaViewControl.g_LocationX < 0 || mouseState.Y - XnaViewControl.g_LocationY < 0) return;

                Ray ray = Camera.GetMouseRay(new Vector2(mouseState.X - XnaViewControl.g_LocationX, mouseState.Y - XnaViewControl.g_LocationY), viewport);
                GameObject temp = null;

                foreach (GameObject gameObject in AssetManager.Singleton.m_GameObjects)
                {
                    if (gameObject.m_BoundingSphere.Intersects(ray) != null)
                    {
                        temp = gameObject;
                    }
                }

                selection = temp;
            }
        }
    }
}
