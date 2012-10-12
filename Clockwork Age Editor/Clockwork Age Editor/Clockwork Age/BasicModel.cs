using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Clockwork_Age
{
    class BasicModel : Node
    {
        Effect effect;
        GraphicsDevice graphicsDevice;
        Matrix worldRotation, worldTranslation, worldScale;
        public Model model { get; protected set; }
        Texture2D texture;
        public string name;

        public BasicModel(string name, Model m, Effect effect, Texture2D tex, GraphicsDevice graphicsDevice, Vector3 position)
        {
            this.position = position;
            this.name = name;
            model = m;
            texture = tex;
            this.graphicsDevice = graphicsDevice;
            worldRotation = Matrix.Identity;
            worldTranslation = Matrix.CreateTranslation(this.position);
            worldScale = Matrix.CreateScale(1);
            this.effect = effect;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        public virtual void update(float deltaTime)
        {

        }

        public void draw()
        {
            Matrix[] tranforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(tranforms);

            foreach(ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(GetWorld() * mesh.ParentBone.Transform);
                    effect.Parameters["View"].SetValue(Camera.view);
                    effect.Parameters["Projection"].SetValue(Camera.projection);
                    effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(GetWorld() * mesh.ParentBone.Transform)));
                    effect.Parameters["ModelTexture"].SetValue(texture);
                }

                mesh.Draw();
            }           
        }

        public virtual Matrix GetWorld()
        {
            return Matrix.CreateScale(0.01f) * worldScale * worldRotation * worldTranslation;
        }

    }
}
