using GameJam2020.View.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Animations
{
    public interface IAnimation
    {
        AnimationState State
        {
            get;
        }

        void Run();

        void Reset();

        void Stop();

        void Visit(AObject2D parentObject2D);
    }

    public enum AnimationState
    {
        STARTING,
        RUNNING,
        FINALIZING,
        ENDING
    }

    public enum AnimationType
    {
        ONETIME,
        LOOP
    }
}
