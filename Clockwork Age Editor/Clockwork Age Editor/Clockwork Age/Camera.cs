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
        Vector3 pivot = Vector3.Zero;
        public Vector3 direction, up, target;
        const float MOVEMENT_SPEED = 15;
        const float ROTATION_SPEED = 1f;
        MouseState mouseState, oldMouseState;

        public Camera(Vector3 pos, Vector3 target, Vector3 up, Vector2 dimensions)
        {
            oldMouseState = Mouse.GetState();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)dimensions.X / (float)dimensions.Y, 0.001f, 1000);
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

            if (keyboardState.IsKeyDown(Keys.A))
            {
                position += Vector3.Cross(up, direction) * MOVEMENT_SPEED * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                position -= Vector3.Cross(up, direction) * MOVEMENT_SPEED * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                Vector3 newDir = new Vector3(direction.X, 0, direction.Y);
                position += newDir * MOVEMENT_SPEED * deltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                Vector3 newDir = new Vector3(direction.X, 0, direction.Y);
                position -= newDir * MOVEMENT_SPEED * deltaTime;
            }            
            
            float mouseXDisplacement = mouseState.X - oldMouseState.X;
            float mouseYDisplacement = mouseState.Y - oldMouseState.Y;

            CreateLookAt();

            oldMouseState = mouseState;
        }

        private void CreateLookAtWithPivot(Vector3 pivot)
        {
            view = Matrix.CreateLookAt(position, pivot, up);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(position, position + direction * MOVEMENT_SPEED, up);
        }
    }
}