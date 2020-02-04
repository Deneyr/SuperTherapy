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
    public class PrePhase : APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        private PrePhaseMoment moment;

        public PrePhase()
        {
            this.periodPhase = Time.FromSeconds(3);

            this.timeElapsed = Time.Zero;

            this.moment = PrePhaseMoment.START;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            AObject patient = world.GetObjectFromId("patient main");
            AObject toubib = world.GetObjectFromId("toubib main");

            AObject bubble = world.GetObjectFromId("bubble main");
            bubble.SetKinematicParameters(new Vector2f(-520f, -380f), new Vector2f(0f, 0f));

            patient.SetAnimationIndex(6);
            toubib.SetAnimationIndex(2);

            DialogueObject dialogue = world.GetObjectFromId("dialogue patient") as DialogueObject;
            dialogue.SetKinematicParameters(new Vector2f(-100f, -200f), new Vector2f(0f, 0f));

            this.periodPhase = Time.FromSeconds(2);
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
                    case PrePhaseMoment.START:
                        AObject bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(1);

                        AObject toubib = world.GetObjectFromId("toubib main");
                        toubib.SetAnimationIndex(3);

                        this.moment = PrePhaseMoment.BUBBLE_APPEARED;
                        this.periodPhase = Time.FromSeconds(1.2f);
                        this.timeElapsed = Time.Zero;
                        break;
                    case PrePhaseMoment.BUBBLE_APPEARED:
                        DialogueObject dialogue = world.GetObjectFromId("dialogue coming") as DialogueObject;
                        dialogue.SetKinematicParameters(new Vector2f(-380f, dialogue.GetHeight(-150)), new Vector2f(0f, 0f));
                        dialogue.LaunchDialogue(1);

                        AObject queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(100f, 100f), new Vector2f(0f, 0f));

                        bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(2);
                        this.moment = PrePhaseMoment.START_TALKING;
                        break;
                    case PrePhaseMoment.TEXT_APPEARED:
                        dialogue = world.GetObjectFromId("dialogue coming") as DialogueObject;
                        dialogue.ResetDialogue();

                        bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(3);

                        queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(2);
                        this.moment = PrePhaseMoment.END;
                        break;
                    case PrePhaseMoment.END:
                        this.NodeState = NodeState.NOT_ACTIVE;
                        break;
                }
            }
        }

        protected override void OnInternalGameEvent(OfficeWorld world, AObject lObject, AObject lObjectTo, string details)
        {
            DialogueObject dialogue = world.GetObjectFromId("dialogue coming") as DialogueObject;

            if (dialogue == lObject)
            {
                this.timeElapsed = Time.Zero;
                this.periodPhase = Time.FromSeconds(1);
                this.moment = PrePhaseMoment.TEXT_APPEARED;
            }
        }
    }

    public enum PrePhaseMoment
    {
        START,
        START_TALKING,
        BUBBLE_APPEARED,
        TEXT_APPEARED,
        END
    }
}
