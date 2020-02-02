using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public class BubbleTutoObject2D: AObject2D
    {
        public BubbleTutoObject2D()
        {
            this.sprite.Scale = new Vector2f(0.5f, 0.5f);
        }

        public override void AssignTextures(List<Texture> textures)
        {
            if (textures.Count > 0)
            {
                this.sprite.Texture = textures[0];

                this.SetCanevas(new IntRect(0, 0, (int)this.sprite.Texture.Size.X, (int)this.sprite.Texture.Size.Y));
            }
        }
    }
}
