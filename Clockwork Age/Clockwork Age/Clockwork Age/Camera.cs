using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Clockwork_Age
{
    class Camera : Node
    {
        public static Matrix view { get; protected set; }
        public static Matrix projection { get; protected set; }

        float rotationY = 0;
        float rotationZ = 0;

        Vector3 direction, up, target;
        const float MOVEMENT_SPEED = 15;
        const float ROTATION_SPEED = 1f;
        MouseState mouseState, oldMouseState;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
        {
            oldMouseState = Mouse.GetState();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height, 1, 1000);
            this.up = up;
            this.target = target;
            position = pos;
            direction = target - pos;
            direction.Normalize();
            CreateLookAt();
        }

        public override void update(float deltaTime)
        {
            MouseState mouseState = Mouse.GetState();
            
            KeyboardState keyboardState = Keyboard.GetState();
            position += direction * ((mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue) / 20) * 0.5f;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                position += Vector3.Cross(up, direction) * MOVEMENT_SPEED * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                position -= Vector3.Cross(up, direction) * MOVEMENT_SPEED * deltaTime;
            }            
            
            float mouseXDisplacement = mouseState.X - oldMouseState.X;
            float mouseYDisplacement = mouseState.Y - oldMouseState.Y;

            
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (mouseState.X > Game1.SCREEN_RECT.Width - 3)
                {
                    Mouse.SetPosition(Game1.SCREEN_RECT.X, mouseState.Y);
                    mouseState = Mouse.GetState();
                }
                else if (mouseState.X < Game1.SCREEN_RECT.X + 3)
                {
                    Mouse.SetPosition(Game1.SCREEN_RECT.X + Game1.SCREEN_RECT.Width, mouseState.Y);
                    mouseState = Mouse.GetState();
                }

                if (mouseState.Y > Game1.SCREEN_RECT.Height - 3)
                {
                    Mouse.SetPosition(mouseState.X, Game1.SCREEN_RECT.Y);
                    mouseState = Mouse.GetState();
                }
                else if (mouseState.Y < Game1.SCREEN_RECT.Y + 3)
                {
                    Mouse.SetPosition(mouseState.X, Game1.SCREEN_RECT.Y + Game1.SCREEN_RECT.Height);
                    mouseState = Mouse.GetState();
                }

                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(up, (-MathHelper.PiOver4 / 150) * mouseXDisplacement));
                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(Vector3.Cross(up, direction), (MathHelper.PiOver4 / 100) * mouseYDisplacement));
                direction.Normalize();
            }

            
            up = Vector3.Up;

            CreateLookAt();
            oldMouseState = mouseState;
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(position, position + direction * MOVEMENT_SPEED, up);
        }
    }
}