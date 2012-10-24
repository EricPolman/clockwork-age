using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Clockwork_Age_Editor
{
    class Camera : Node
    {
        public static Matrix View { get; protected set; }
        public static Matrix Projection { get; set; }
        public Matrix m_mRotation = Matrix.Identity;

        public Vector3 m_vRotation = Vector3.Zero;

        public Vector3 m_vUp, m_vTarget;
        public CameraMan m_CameraMan;

        public Camera(Vector3 pos, Vector3 target, Vector3 up, Vector2 dimensions)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)dimensions.X / (float)dimensions.Y, 0.001f, 1000);
            m_vUp = up;
            m_vTarget = target;
            m_vPosition = pos;

            m_CameraMan = new CameraMan(this);

            CreateLookAt();
        }

        public override void Update(float deltaTime)
        {
            m_CameraMan.update(deltaTime);

            CreateLookAt();
        }

        private void CreateLookAt()
        {
            View = Matrix.CreateLookAt(m_vPosition, m_vTarget, m_vUp);
        }

        public static Ray GetMouseRay(Vector2 mousePosition, Viewport viewport)
        {
            Vector3 nearPoint = new Vector3(mousePosition, 0);
            Vector3 farPoint = new Vector3(mousePosition, 1);

            Vector3 nearPointWorld = viewport.Unproject(nearPoint, Projection, View, Matrix.Identity);
            Vector3 farPointWorld = viewport.Unproject(farPoint, Projection, View, Matrix.Identity);
            Vector3 direction = Vector3.Normalize(farPointWorld - nearPointWorld);

            Ray ray = new Ray(nearPointWorld, direction);
            return ray;
        }
    }
}