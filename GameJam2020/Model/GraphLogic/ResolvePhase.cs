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
    public class ResolvePhase: APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        private bool isSuccess;

        private ResolvePhaseMoment moment;

        public ResolvePhase()
        {
            this.periodPhase = Time.FromSeconds(3);

            this.timeElapsed = Time.Zero;

            this.moment = ResolvePhaseMoment.START;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            DialogueObject dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
            this.isSuccess = dialogue.IsSuccess;

            AObject toubib = world.GetObjectFromId("toubib main");
            toubib.SetAnimationIndex(1);

            AObject patient = world.GetObjectFromId("patient main");
            patient.SetAnimationIndex(2);

            AObject bubble = world.GetObjectFromId("bubble main");
            bubble.SetAnimationIndex(1);

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
                    case ResolvePhaseMoment.START:
                        DialogueObject dialogue;

                        if (this.isSuccess)
                        {
                            dialogue = world.GetObjectFromId("dialogue successAnswer") as DialogueObject;
                        }
                        else
                        {
                            dialogue = world.GetObjectFromId("dialogue failAnswer") as DialogueObject;
                        }
                        dialogue.SetKinematicParameters(new Vector2f(-300f, -150f), new Vector2f(0f, 0f));
                        dialogue.LaunchDialogue(2);

                        AObject queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(-200f, 120f), new Vector2f(0f, 0f));

                        AObject bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(2);

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(2);
                        this.moment = ResolvePhaseMoment.BUBBLE_APPEARED;
                        break;
                    /*case ResolvePhaseMoment.END_DIALOGUE:
                        dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
                        dialogue.ResetDialogue();

                        bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(3);

                        queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));
                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(2);
                        this.moment = ResolvePhaseMoment.END;
                        break;*/
                    case ResolvePhaseMoment.END:
                        this.NodeState = NodeState.NOT_ACTIVE;
                        break;
                }
            }
        }

        protected override void OnInternalGameEvent(OfficeWorld world, AObject lObject, string details)
        {
            DialogueObject dialogue;
            AObject patient = world.GetObjectFromId("patient main");

            if (this.isSuccess)
            {
                dialogue = world.GetObjectFromId("dialogue successAnswer") as DialogueObject;
                patient.SetAnimationIndex(5);
            }
            else
            {
                dialogue = world.GetObjectFromId("dialogue failAnswer") as DialogueObject;
                patient.SetAnimationIndex(4);
            }

            if (dialogue == lObject)
            {
                dialogue.ResetDialogue();

                AObject bubble = world.GetObjectFromId("bubble main");
                bubble.SetAnimationIndex(3);

                AObject queueTalk = world.GetObjectFromId("queueTalk main");
                queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));

                this.timeElapsed = Time.Zero;
                this.periodPhase = Time.FromSeconds(2);
                this.moment = ResolvePhaseMoment.END;
            }
        }
    }

    public enum ResolvePhaseMoment
    {
        START,
        BUBBLE_APPEARED,
        END_DIALOGUE,
        END
    }
}
