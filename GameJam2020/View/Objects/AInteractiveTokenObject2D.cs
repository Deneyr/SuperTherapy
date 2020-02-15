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

        private float initialWidth;
        private float initialHeight;

        public AInteractiveTokenObject2D()
        {
            this.answerRectangle = new Sprite();
        }

        public override void DrawIn(RenderWindow window)
        {
            if (string.IsNullOrEmpty(this.text.DisplayedString) == false)
            {
                this.answerRectangle.Position = new Vector2f(this.text.Position.X - 7f, this.text.Position.Y - 5);

                FloatRect boundsText = this.text.GetGlobalBounds();
                float police = this.text.CharacterSize;

                this.answerRectangle.Scale = new Vector2f((boundsText.Width + 14) / this.initialWidth, (Math.Max(boundsText.Height, police) + 10) / this.initialHeight);
            }
            else
            {
                this.answerRectangle.Scale = new Vector2f(0, 0);
            }

            window.Draw(this.answerRectangle);
            base.DrawIn(window);
        }

        public override void AssignTextures(List<Texture> textures)
        {
            if (textures.Count > 0)
            {
                this.answerRectangle.Texture = textures[0];

                this.initialWidth = this.answerRectangle.Texture.Size.X;
                this.initialHeight = this.answerRectangle.Texture.Size.Y;
            }
        }

        public override Vector2f DeltaSpriteText
        {
            get
            {
                return new Vector2f(-7f, -5f);
            }
        }

        public override FloatRect SpriteGlobalBounds
        {
            get
            {
                return this.answerRectangle.GetGlobalBounds();
            }
        }
    }
}
