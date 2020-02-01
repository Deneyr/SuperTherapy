using GameJam2020.View.Objects;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameJam2020.View.Animations
{
    public class AnimationManager
    {
        public static int ANIMATION_MANAGER_PERIOD = 100;

        private Mutex mutex;

        private Thread mainThread;

        private volatile bool play;

        private Dictionary<AObject2D, IAnimation> animationsToPlay;

        public bool Play
        {
            get
            {
                return this.play;
            }
            set
            {
                this.play = value;
            }
        }

        public AnimationManager()
        {
            this.mutex = new Mutex();

            this.mainThread = new Thread(new ThreadStart(this.Run));

            this.Play = true;

            this.animationsToPlay = new Dictionary<AObject2D, IAnimation>();

            this.mainThread.Start();
        }

        private void Run()
        {
            while (this.Play)
            {
                List<AObject2D> finishedAnimation = new List<AObject2D>();

                this.mutex.WaitOne();

                foreach (KeyValuePair<AObject2D, IAnimation> keyValuePair in this.animationsToPlay)
                {
                    if(keyValuePair.Value.State == AnimationState.ENDING)
                    {
                        finishedAnimation.Add(keyValuePair.Key);
                    }
                    else
                    {
                        keyValuePair.Value.Run();
                    }
                }

                foreach (AObject2D object2D in finishedAnimation)
                {
                    this.animationsToPlay.Remove(object2D);
                }

                this.mutex.ReleaseMutex();

                Thread.Sleep(100);
            }
        }

        public IAnimation GetAnimationFromAObject2D(AObject2D object2D)
        {
            this.mutex.WaitOne();

            IAnimation animation = null;

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                animation = this.animationsToPlay[object2D];
            }

            this.mutex.ReleaseMutex();

            return animation;
        }

        public void PlayAnimation(AObject2D object2D, IAnimation animation)
        {
            this.mutex.WaitOne();

            animation.Reset();

            if (this.animationsToPlay.ContainsKey(object2D))
            {
                this.animationsToPlay[object2D] = animation;
            }
            else
            {
                this.animationsToPlay.Add(object2D, animation);
            }

            this.mutex.ReleaseMutex();
        }

    }
}
