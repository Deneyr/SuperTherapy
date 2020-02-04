using GameJam2020.Model.Events;
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
    public class StartPhase: APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        public StartPhase()
        {
            this.periodPhase = Time.FromSeconds(3);

            this.timeElapsed = Time.Zero;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            AObject patient = world.GetObjectFromId("patient main");
            AObject toubib = world.GetObjectFromId("toubib main");

            //AObject test = world.GetObjectFromId("test");

            patient.SetAnimationIndex(1);
            toubib.SetAnimationIndex(2);

            DialogueObject dialogue = world.GetObjectFromId("dialogue patient") as DialogueObject;    

            dialogue.SetKinematicParameters(new Vector2f(-100f, -200f), new Vector2f(0f, 0f));

            this.periodPhase = Time.FromSeconds(3);
            this.timeElapsed = Time.Zero;
        }

        public override void VisitEnd(OfficeWorld world)
        {
            base.VisitEnd(world);
        }

        public override void UpdateLogic(OfficeWorld world, Time timeElapsed)
        {
            this.timeElapsed += timeElapsed;

            if(this.timeElapsed > periodPhase)
            {
                this.NodeState = NodeState.NOT_ACTIVE;
            }
        }
    }
}
