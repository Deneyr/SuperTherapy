using GameJam2020.Model.World;
using GameJam2020.Model.World.Objects;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.GraphLogic
{
    public class TutoPhase : APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        private bool isValidated;

        public TutoPhase()
        {
            this.timeElapsed = Time.Zero;

            this.periodPhase = Time.FromSeconds(1);

            this.isValidated = false;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            /*AObject patient = world.GetObjectFromId("patient");

            AObject test = world.GetObjectFromId("test");

            patient.SetAnimationIndex(1);
            toubib.SetAnimationIndex(1);

            DialogueObject dialogue = world.GetObjectFromId("dialogue") as DialogueObject;

            dialogue.SetKinematicParameters(new Vector2f(-100f, -200f), new Vector2f(0f, 0f));
            dialogue.LaunchDialogue();*/
        }

        public override void VisitEnd(OfficeWorld world)
        {
            base.VisitEnd(world);
        }

        protected override void OnInternalGameEvent(AObject lObject, string details)
        {
            if(details.Equals("association"))
            {
                this.isValidated = true;
                this.timeElapsed = Time.Zero;
            }
        }

        public override void UpdateLogic(OfficeWorld world, Time timeElapsed)
        {
            if (this.isValidated)
            {
                this.timeElapsed += timeElapsed;

                if (this.timeElapsed > periodPhase)
                {
                    this.NodeState = NodeState.NOT_ACTIVE;
                }
            }
        }
    }
}
