using GameJam2020.View.Animations;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public abstract class AInteractiveTokenObject2D: ATokenObject2D
    {
        protected Sprite answerRectangle;

        private float Initialwidth;
        private float InitialHeight;

        public AInteractiveTokenObject2D()
        {
            this.answerRectangle = new Sprite();
        }

        public override void DrawIn(RenderWindow window)
        {
            this.answerRectangle.Position = new Vector2f(this.text.Position.X - 5, this.text.Position.Y - 5);

            FloatRect boundsText = this.text.GetGlobalBounds();
            float police = this.text.CharacterSize;

            this.answerRectangle.Scale = new Vector2f(boundsText.Width / this.Initialwidth * 1.1f, Math.Max(boundsText.Height, police) / this.InitialHeight * 1.4f);

            window.Draw(this.answerRectangle);
            base.DrawIn(window);
        }

        public override void AssignTextures(List<Texture> textures)
        {
            if (textures.Count > 0)
            {
                this.answerRectangle.Texture = textures[0];

                this.Initialwidth = this.answerRectangle.Texture.Size.X;
                this.InitialHeight = this.answerRectangle.Texture.Size.Y;
            }
        }
    }
}
