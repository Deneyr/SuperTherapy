using GameJam2020.View.Animations;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace GameJam2020.View.Objects
{
    public abstract class AObject2D
    {
        protected static AnimationManager animationManager;

        protected static ZoomAnimationManager zoomAnimationManager;

        protected Sprite sprite;

        protected Text text;

        private List<IAnimation> animationsList;

        private IntRect currentCanevas;

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();

            AObject2D.zoomAnimationManager = new ZoomAnimationManager();
        }

        public AObject2D()
        {
            this.sprite = new Sprite();
            this.text = new Text();

            this.animationsList = new List<IAnimation>();

            this.currentCanevas = new IntRect();

            Time periode = Time.FromMilliseconds(2000);
            ZoomAnimation zoomAnimation = new ZoomAnimation(1, 1.3f, periode, AnimationType.LOOP);
            this.AddAnimation(zoomAnimation);
        }

        public virtual void AssignTextures(List<Texture> textures)
        {
            this.sprite.Texture = textures[0];
        }

        public virtual void AssignFonts(List<Font> fonts)
        {
            this.text.Font = fonts[0];
        }

        public void SetText(string newText)
        {
            this.text.DisplayedString = newText;
        }

        public void AddAnimation(IAnimation animation)
        {
            this.animationsList.Add(animation);
        }

        public void SetCanevas(IntRect canevas)
        {
            this.currentCanevas = canevas;
        }

        public void SetFocus(bool setFocus)
        {
            IAnimation animation = this.animationsList[0];

            if (setFocus)
            {
                this.PlayAnimation(0);
            }
            else
            {
                animation.Stop();
                this.sprite.Scale = new Vector2f(1, 1);
            }
        }

        public void SetZoom(float zoom)
        {
            this.sprite.Scale = new Vector2f(zoom, zoom);
        }

        public void SetPosition(Vector2f newPosition, Vector2f sizeScreen)
        {
            Vector2f realPosition = new Vector2f(newPosition.X * sizeScreen.X, newPosition.Y * sizeScreen.Y);

            this.sprite.Position = realPosition;
        }

        public static IntRect[] CreateAnimation(int leftStart, int topStart, int width, int height, int nbFrame)
        {
            IntRect[] result = new IntRect[nbFrame];

            for(int i = 0; i < nbFrame; i++)
            {
                result[i] = new IntRect(leftStart + i * width, topStart, width, height);
            }

            return result;
        }

        public virtual void DrawIn(RenderWindow window)
        {

            IAnimation animation = AObject2D.animationManager.GetAnimationFromAObject2D(this);
            if(animation != null)
            {
                animation.Visit(this);
            }

            animation = AObject2D.zoomAnimationManager.GetAnimationFromAObject2D(this);
            if (animation != null)
            {
                animation.Visit(this);
            }

            this.sprite.TextureRect = this.currentCanevas;

            window.Draw(this.sprite);
        }

        public void PlayAnimation(int index)
        {
            IAnimation animation = this.animationsList[index];

            if (animation is ZoomAnimation)
            {
                AObject2D.zoomAnimationManager.PlayAnimation(this, animation as ZoomAnimation);
            }
            else
            {
                AObject2D.animationManager.PlayAnimation(this, animation);
            }
        }

        public static void StopAnimationManager()
        {
            AObject2D.animationManager.Play = false;
        }

        public static void UpdateZoomAnimationManager(Time deltaTime)
        {
            AObject2D.zoomAnimationManager.Run(deltaTime);
        }
    }
}
