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
    public class QueueTalkObject2D: AObject2D
    {
        public QueueTalkObject2D()
        {
            Time periode = Time.FromMilliseconds(300);

            IntRect[] testAnim = AObject2D.CreateAnimation(0, 0, 200, 200, 4);

            this.sprite.Scale = new Vector2f(0.5f, 0.5f);

            Animation animation = new Animation(testAnim, periode, AnimationType.LOOP);

            this.AddAnimation(animation);
        }
    }
}
