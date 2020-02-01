using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class AObject
    {
        private Vector2f position;

        private Vector2f speedVector;

        private int previousAnimationIndex;
        private int currentAnimationIndex;

        private bool previousIsFocused;
        private bool isFocused;

        private string id;

        public AObject(string id)
        {
            this.id = id;

            this.position = new Vector2f(0, 0);
            this.speedVector = new Vector2f(0, 0);

            this.previousAnimationIndex = -1;
            this.currentAnimationIndex = -1;

            this.previousIsFocused = false;
            this.isFocused = false;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public Vector2f Position
        {
            get
            {
                return this.Position;
            }
        }

        public Vector2f SpeedVector
        {
            get
            {
                return this.speedVector;
            }
        }

        public bool IsFocused
        {
            get
            {
                return this.isFocused;
            }

            set
            {
                this.isFocused = value;
            }
        }


        public void SetKinematicParameters(Vector2f position, Vector2f speedVector)
        {
            this.position = position;
            this.speedVector = speedVector;
        }

        public void SetAnimationIndex(int animationIndex)
        {
            this.currentAnimationIndex = animationIndex;
        }

        public virtual void UpdateLogic(OfficeWorld officeWorld, Time TimeElapsed)
        {
            if (this.isFocused != this.previousIsFocused)
            {
                officeWorld.NotifyObjectFocusChanged(this, this.isFocused);

                this.previousIsFocused = this.isFocused;
            }

            if (this.currentAnimationIndex != this.previousAnimationIndex)
            {
                officeWorld.NotifyObjectAnimationChanged(this, this.currentAnimationIndex);

                this.previousAnimationIndex = this.currentAnimationIndex;
            }

            if(this.speedVector != null 
                && (this.speedVector.X != 0 || this.speedVector.Y != 0))
            {
                this.position += this.speedVector * TimeElapsed.AsSeconds();

                officeWorld.NotifyObjectPositionChanged(this, this.position);
            }
        }

    }
}
