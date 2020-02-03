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
    public class BubbleHeaderObject2D: AObject2D
    {
        public BubbleHeaderObject2D()
        {
            /*Time periode = Time.FromMilliseconds(600);

            IntRect[] testAnim = AObject2D.CreateAnimation(1000, 0, 1000, 400, 2);

            this.sprite.Scale = new Vector2f(0.5f, 0.5f);

            Animation animation = new Animation(testAnim, periode, AnimationType.LOOP);

            this.AddAnimation(animation);*/

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

            this.sprite.Scale = new Vector2f(0.6f, 0.6f);
        }
    }
}
