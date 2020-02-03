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
    public class ExplainPhase: APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        private StartPhaseMoment moment;

        public ExplainPhase()
        {
            this.periodPhase = Time.FromSeconds(1);

            this.timeElapsed = Time.Zero;

            this.moment = StartPhaseMoment.START;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            AObject patient = world.GetObjectFromId("patient main");
            AObject toubib = world.GetObjectFromId("toubib main");

            AObject bubble = world.GetObjectFromId("bubble main");
            bubble.SetKinematicParameters(new Vector2f(-520f, -380f), new Vector2f(0f, 0f));

            patient.SetAnimationIndex(2);
            toubib.SetAnimationIndex(1);

            bubble.SetAnimationIndex(1);

            DialogueObject dialogue = world.GetObjectFromId("dialogue patient") as DialogueObject;
            dialogue.SetKinematicParameters(new Vector2f(-380f, dialogue.GetHeight(-150)), new Vector2f(0f, 0f));

            this.periodPhase = Time.FromSeconds(1.2f);
            this.timeElapsed = Time.Zero;
        }

        public override void VisitEnd(OfficeWorld world)
        {
            base.VisitEnd(world);
        }

        public override void UpdateLogic(OfficeWorld world, Time timeElapsed)
        {
            this.timeElapsed += timeElapsed;

            if (this.timeElapsed > periodPhase)
            {
                switch (this.moment)
                {
                    case StartPhaseMoment.START:
                        DialogueObject dialogue = world.GetObjectFromId("dialogue patient") as DialogueObject;
                        dialogue.LaunchDialogue(5);

                        AObject queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(-200f, 120f), new Vector2f(0f, 0f));

                        AObject bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(2);

                        this.moment = StartPhaseMoment.BUBBLE_APPEARED;
                        break;
                    case StartPhaseMoment.TEXT_APPEARED:
                        dialogue = world.GetObjectFromId("dialogue patient") as DialogueObject;
                        dialogue.ResetDialogue();

                        bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(3);

                        queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(2);
                        this.moment = StartPhaseMoment.END;
                        break;
                    case StartPhaseMoment.END:
                        this.NodeState = NodeState.NOT_ACTIVE;
                        break;
                }
            }
        }

        protected override void OnInternalGameEvent(OfficeWorld world, AObject lObject, string details)
        {
            DialogueObject dialogue = world.GetObjectFromId("dialogue patient") as DialogueObject;

            if (dialogue == lObject)
            {
                this.timeElapsed = Time.Zero;
                this.periodPhase = Time.FromSeconds(3);
                this.moment = StartPhaseMoment.TEXT_APPEARED;
            }
        }
    }

    public enum StartPhaseMoment
    {
        START,
        BUBBLE_APPEARED,
        TEXT_APPEARED,
        END
    }
}
