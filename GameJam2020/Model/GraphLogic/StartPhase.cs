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
            this.periodPhase = Time.FromSeconds(100);

            this.timeElapsed = Time.Zero;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            /*AObject patient = world.GetObjectFromId("patient");
            AObject toubib = world.GetObjectFromId("toubib");*/

            AObject test = world.GetObjectFromId("test");

            DialogueObject dialogue = world.GetObjectFromId("dialogue") as DialogueObject;

            test.SetAnimationIndex(1);
            test.SetKinematicParameters(new Vector2f(0, 0), new Vector2f(10, 0));

            test.IsFocused = true;

            dialogue.SetKinematicParameters(new Vector2f(-100f, -100f), new Vector2f(0f, 0f));
            dialogue.LaunchDialogue();
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
            else if(this.timeElapsed > periodPhase / 2 && testBool)
            {
                AObject test = world.GetObjectFromId("test");

                test.SetKinematicParameters(new Vector2f(0.5f, 0.5f), new Vector2f(-0.05f, 0));

                test.IsFocused = false;

                testBool = false;
            }
        }

        bool testBool = true;
    }
}
