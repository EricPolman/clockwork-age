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
            mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                Ray ray = Camera.GetMouseRay(new Vector2(mouseState.X, mouseState.Y), viewport, camera);
                float? dist = 0;
                foreach (BasicModel bm in ModelManager.g_models)
                {
                    foreach (ModelMesh mesh in bm.model.Meshes)
                    {
                        float? temp = ray.Intersects(mesh.BoundingSphere);
                        if (temp > ray.Intersects(mesh.BoundingSphere))
                        {
                            dist = ray.Intersects(mesh.BoundingSphere);
                        }
                        
                    }
                }

            }

            oldMouseState = mouseState;
        }




    }
}
