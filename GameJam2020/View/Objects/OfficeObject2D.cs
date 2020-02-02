using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public class OfficeObject2D: AObject2D
    {
        private List<Sprite> scenarySprites;

        public OfficeObject2D()
        {
            this.scenarySprites = new List<Sprite>();

            Sprite sprite = new Sprite();
            sprite.Scale = new Vector2f(0.5f, 0.5f);
            sprite.Position = new Vector2f(50, 120);
            this.scenarySprites.Add(sprite);

            this.sprite.Scale = new Vector2f(1f, 0.7f);
            this.sprite.Origin = new Vector2f(1920 / 2f, 1080 / 1.5f );
        }

        public override void AssignTextures(List<Texture> textures)
        {
            int i = 0;
            foreach(Texture texture in textures)
            {
                if (i == 0)
                {
                    this.sprite.Texture = textures[0];
                }
                else
                {
                    this.scenarySprites[i - 1].Texture = textures[i];
                }
                i++;
            }
        }

        public override void DrawIn(RenderWindow window)
        {
            window.Draw(this.sprite);

            foreach(Sprite sprite in this.scenarySprites)
            {
                window.Draw(sprite);
            }
        }
    }
}
