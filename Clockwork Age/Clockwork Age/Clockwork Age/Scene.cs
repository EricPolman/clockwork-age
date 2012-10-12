using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Clockwork_Age
{
    class Scene
    {
        ModelManager modelManager;
        Camera camera;
        Game game;
        string name = "default";

        public Scene(Game game, string name)
        {
            this.game = game;
            this.name = name;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            modelManager = new ModelManager(graphicsDevice, name);
            modelManager.LoadContent(Content);

            camera = new Camera(game, new Vector3(0, 2, 10), Vector3.Forward, Vector3.Up);
        }

        public void update(float deltaTime)
        {
            modelManager.update(deltaTime);
            camera.update(deltaTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            modelManager.draw(spriteBatch);
        }
    }
}
