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

            IntRect[] testAnim = AObject2D.CreateAnimation(0, 0, 574, 227, 5);
            Animation animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 227, 574, 226, 5);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 227 * 2, 574, 226, 2);
            animation = new Animation(testAnim, periode, AnimationType.LOOP);
            this.AddAnimation(animation);

            periode = Time.FromMilliseconds(300);
            testAnim = AObject2D.CreateAnimation(0, 227 * 3, 574, 226, 5);
            animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 227 * 4, 574, 226, 5);
            animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            testAnim = AObject2D.CreateAnimation(0, 227 * 5, 574, 226, 5);
            animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            // Angry & Happy
            testAnim = new IntRect[]{
                new IntRect(0, 227 * 2, 574, 226),
                new IntRect(0, 227 * 3, 574, 226),
            };
            animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            testAnim = new IntRect[]{
                new IntRect(0, 227 * 2, 574, 226),
                new IntRect(0, 227 * 4, 574, 226),
            };
            animation = new Animation(testAnim, periode, AnimationType.ONETIME);
            this.AddAnimation(animation);

            //this.sprite.Scale = new Vector2f(0.5f, 0.5f);
        }
    }
}
