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
    public class PatientObject2D: AObject2D
    {
        public PatientObject2D()
        {
            Time periode = Time.FromMilliseconds(200);

            IntRect[] testAnim = AObject2D.CreateAnimation(0, 0, 399, 157, 5);
            Animation animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 399, 1139, 157, 5);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 399 * 2, 1139, 157, 2);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 399 * 3, 1139, 157, 5);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 399 * 4, 1139, 157, 5);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 399 * 5, 1139, 157, 1);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            //this.sprite.Scale = new Vector2f(0.5f, 0.5f);
        }
    }
}
