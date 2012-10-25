using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Clockwork_Age_Editor
{
    class BoundingBoxRenderer
    {
        // Initialize an array of indices for the box. 12 lines require 24 indices
        static short[] bBoxIndices = {
	        0, 1, 1, 2, 2, 3, 3, 0, // Front edges
	        4, 5, 5, 6, 6, 7, 7, 4, // Back edges
	        0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
        };

        public static BasicEffect boxEffect = new BasicEffect(AssetManager.Singleton.m_GraphicsDevice);

        public static void Draw(BoundingBox box)
        {
            Vector3[] corners = box.GetCorners();
            VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

            // Assign the 8 box vertices
            for (int i = 0; i < corners.Length; i++)
            {
                primitiveList[i] = new VertexPositionColor(corners[i], Color.White);
            }

            /* Set your own effect parameters here */

            boxEffect.World = Matrix.Identity;
            boxEffect.View = Camera.View;
            boxEffect.Projection = Camera.Projection;
            boxEffect.TextureEnabled = false;

            // Draw the box with a LineList
            foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                AssetManager.Singleton.m_GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList, primitiveList, 0, 8,
                    bBoxIndices, 0, 12);
            }

        }
    }
}
