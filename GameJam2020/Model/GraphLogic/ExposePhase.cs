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
    public class ExposePhase: APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        private bool isSuccess;

        private ExposePhaseMoment moment;

        public ExposePhase()
        {
            this.periodPhase = Time.FromSeconds(1);

            this.timeElapsed = Time.Zero;

            this.moment = ExposePhaseMoment.START;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            DialogueObject dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
            this.isSuccess = dialogue.IsSuccess;

            AObject toubib = world.GetObjectFromId("toubib main");

            toubib.SetAnimationIndex(3);

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
                    case ExposePhaseMoment.START:
                        DialogueObject dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
                        dialogue.LaunchDialogue(2);

                        AObject queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(100f, 100f), new Vector2f(0f, 0f));

                        AObject bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(2);

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(2);
                        this.moment = ExposePhaseMoment.BUBBLE_APPEARED;
                        break;
                    case ExposePhaseMoment.END_DIALOGUE:
                        dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
                        dialogue.ResetDialogue();

                        bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(3);

                        queueTalk = world.GetObjectFromId("queueTalk main");
                        queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));
                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(2);
                        this.moment = ExposePhaseMoment.END;
                        break;
                    case ExposePhaseMoment.END:
                        this.NodeState = NodeState.NOT_ACTIVE;
                        break;
                }
            }
        }

        protected override void OnInternalGameEvent(OfficeWorld world, AObject lObject, AObject lObjectTo, string details)
        {
            DialogueObject dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;

            if (dialogue == lObject)
            {
                AObject patient = world.GetObjectFromId("patient main");
                if (this.isSuccess)
                {
                    patient.SetAnimationIndex(8);
                }
                else
                {
                    patient.SetAnimationIndex(7);
                }

                this.timeElapsed = Time.Zero;
                this.periodPhase = Time.FromSeconds(2);
                this.moment = ExposePhaseMoment.END_DIALOGUE;
            }
        }
    }

    public enum ExposePhaseMoment
    {
        START,
        BUBBLE_APPEARED,
        END_DIALOGUE,
        END
    }
}
