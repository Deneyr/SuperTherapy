using GameJam2020.View.Animations;
using SFML.Graphics;
using SFML.System;

namespace GameJam2020.View.Objects
{
    class TestObject2D: AObject2D
    {

        public TestObject2D()
        {
            this.sprite.Position = new Vector2f(100, 100);

            this.sprite.Origin = new Vector2f(30, 36);

            Time periode = Time.FromMilliseconds(200);

            IntRect[] testAnim = AObject2D.CreateAnimation(0, 182, 60, 77, 9);

            Animation animation = new Animation(testAnim, periode, AnimationType.LOOP);

            this.AddAnimation(animation);
        }
    }
}
