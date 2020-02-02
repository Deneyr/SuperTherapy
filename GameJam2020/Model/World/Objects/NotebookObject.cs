using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class NotebookObject : AObject
    {
        private Time duration;
        private Time elapsedTime;


        public NotebookObject() : base("notebook")
        {

            this.duration = Time.Zero;
        }

        public void SetTransition(Vector2f from, Vector2f speed, Time duration)
        {
            this.SetKinematicParameters(from, speed);

            this.duration = duration;
            this.elapsedTime = Time.Zero;
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {
            if(this.duration != Time.Zero)
            {
                if (this.duration < this.elapsedTime)
                {
                    this.duration = Time.Zero;
                    this.elapsedTime = Time.Zero;

                    this.SetKinematicParameters(this.position, new Vector2f(0, 0));
                }
                else
                {
                    this.elapsedTime += timeElapsed;
                }
            }

            base.UpdateLogic(officeWorld, timeElapsed);
        }
    }
}
