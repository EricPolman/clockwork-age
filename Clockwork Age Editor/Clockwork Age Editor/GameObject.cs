using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Clockwork_Age_Editor
{
    class GameObject : Node
    {
        public Effect m_Effect;
        public Texture2D m_Texture;
        public Model m_Model;

        public BoundingSphere m_BoundingSphere;
        public BoundingBox m_BoundingBox;

        public string m_sName;

        float rotation;

        GraphicsDevice GRAPHICS_DEVICE = AssetManager.Singleton.m_GraphicsDevice;
        Matrix worldRotation, worldScale;
        
        public GameObject(string name, Model model, Effect effect, Texture2D texture, Vector3 position)
        {
            Name = name;
            Text = name;
            m_vPosition = position;
            m_sName = name;
            m_Model = model;
            m_Texture = texture;
            m_Effect = effect;
            m_BoundingSphere = model.Meshes[0].BoundingSphere;

            worldRotation = Matrix.Identity;
            worldScale = Matrix.CreateScale(1);
            
            SetEffect(m_Effect);

            BoundingBox box = UpdateBoundingBox(model, GetWorld());
            m_BoundingSphere.Center = (box.Min + box.Max) * 0.5f;
            m_BoundingSphere.Radius *= 0.008f;
            m_BoundingBox = box;
        }

        public void SetEffect(Effect effect)
        {
            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public void Draw()
        {
            Matrix[] tranforms = new Matrix[m_Model.Bones.Count];
            m_Model.CopyAbsoluteBoneTransformsTo(tranforms);

            foreach(ModelMesh mesh in m_Model.Meshes)
            {
                
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    
                    if (m_Texture == null)
                    {
                        m_Effect.CurrentTechnique = m_Effect.Techniques["DiffuseWithoutTexture"];
                    }
                    else
                    {
                        m_Effect.CurrentTechnique = m_Effect.Techniques["DiffuseWithTexture"];
                        m_Effect.Parameters["ModelTexture"].SetValue(m_Texture);
                    }
                    m_Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * GetWorld());
                    m_Effect.Parameters["View"].SetValue(Camera.View);
                    m_Effect.Parameters["Projection"].SetValue(Camera.Projection);
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(mesh.ParentBone.Transform * Matrix.CreateScale(0.01f) * worldScale * worldRotation));
                    m_Effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }

                mesh.Draw();
            }           
        }

        public virtual Matrix GetWorld()
        {
            return Matrix.CreateScale(0.01f) * worldScale * worldRotation * Matrix.CreateTranslation(m_vPosition);
        }

        protected BoundingBox UpdateBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            return new BoundingBox(min, max);
        }

        public string[] Export()
        {
            string[] exportData = new string[4];
            exportData[0] = "Models/" + m_sName;
            exportData[1] = m_Effect.Name;

            return exportData;
        }

        public void SetName(string name)
        {
            Name = name;
            Text = name;
            m_sName = name;
        }
    }
}
