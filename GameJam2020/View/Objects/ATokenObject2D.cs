using GameJam2020.View.Animations;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public abstract class ATokenObject2D: AObject2D
    {

        public ATokenObject2D()
        {
            this.text.OutlineColor = Color.White;
        }

        public override void DrawIn(RenderWindow window)
        {

            IAnimation animation = AObject2D.animationManager.GetAnimationFromAObject2D(this);
            if (animation != null)
            {
                animation.Visit(this);
            }

            animation = AObject2D.zoomAnimationManager.GetAnimationFromAObject2D(this);
            if (animation != null)
            {
                animation.Visit(this);
            }

            this.text.Position = this.sprite.Position;
            this.text.Scale = this.sprite.Scale;

            window.Draw(this.text);
        }

    }
}
