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
    public class BubbleObject2D: AObject2D
    {
        public BubbleObject2D()
        {
            Time periode = Time.FromMilliseconds(200);

            IntRect[] testAnim = AObject2D.CreateAnimation(0, 0, 919, 338, 4);
            Animation animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 338, 919, 338, 4);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 338 * 2, 919, 338, 4);
            animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            this.sprite.Scale = new Vector2f(1.2f, 1.2f);
        }
    }
}
