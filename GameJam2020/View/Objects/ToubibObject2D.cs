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
    public class ToubibObject2D: AObject2D
    {
        public ToubibObject2D()
        {
            Time periode = Time.FromMilliseconds(200);

            IntRect[] testAnim = AObject2D.CreateAnimation(0, 0, 417, 419, 4);
            Animation animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 419, 417, 419, 4);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 419 * 2, 417, 419, 4);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 419 * 3, 417, 419, 4);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            this.sprite.Scale = new Vector2f(0.5f, 0.5f);
        }
    }
}
