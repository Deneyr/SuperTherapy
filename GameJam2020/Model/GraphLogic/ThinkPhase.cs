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
    public class ThinkPhase: APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        private ThinkPhaseMoment moment;

        public ThinkPhase()
        {
            this.periodPhase = Time.FromSeconds(1);

            this.timeElapsed = Time.Zero;

            this.moment = ThinkPhaseMoment.START;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            AObject patient = world.GetObjectFromId("patient main");
            AObject toubib = world.GetObjectFromId("toubib main");

            patient.SetAnimationIndex(3);
            toubib.SetAnimationIndex(2);

            DialogueObject dialogue = world.GetObjectFromId("dialogue patient") as DialogueObject;
            dialogue.ResetDialogue();
            //dialogue.SetKinematicParameters(new Vector2f(-100f, -200f), new Vector2f(0f, 0f));

            DialogueObject dialogueToubib = world.GetObjectFromId("dialogue toubib") as DialogueObject;
            dialogueToubib.SetKinematicParameters(new Vector2f(-300f, -150f), new Vector2f(0f, 0f));

            AObject queueTalk = world.GetObjectFromId("queueTalk main");
            queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0f, 0f));

            AObject noteblock = world.GetObjectFromId("notebook main");
            (noteblock as NotebookObject).SetTransition(new Vector2f(-600, 600), new Vector2f(0, -150), Time.FromSeconds(3));

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
                    case ThinkPhaseMoment.START:
                        DialogueObject dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
                        dialogue.LaunchDialogue(3);

                        AObject queueDream = world.GetObjectFromId("queueDream main");
                        queueDream.SetKinematicParameters(new Vector2f(100f, 100f), new Vector2f(0f, 0f));

                        AObject bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(2);

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(2);
                        this.moment = ThinkPhaseMoment.BUBBLE_APPEARED;
                        break;
                    case ThinkPhaseMoment.BUBBLE_APPEARED:

                        DialogueObject dialogueAnswer = world.GetObjectFromId("dialogue answer") as DialogueObject;
                        dialogueAnswer.SetKinematicParameters(new Vector2f(-550f, 200f), new Vector2f(0f, 0f));
                        dialogueAnswer.LaunchDialogue(4);

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(world.CurrentLevel.Data.Timer);
                        this.moment = ThinkPhaseMoment.START_TIMER;
                        break;
                    case ThinkPhaseMoment.START_TIMER:
                        dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
                        dialogue.ResetDialogue();

                        dialogueAnswer = world.GetObjectFromId("dialogue answer") as DialogueObject;
                        dialogueAnswer.ResetDialogue();

                        AObject noteblock = world.GetObjectFromId("notebook main");
                        (noteblock as NotebookObject).SetTransition(new Vector2f(-600, 150), new Vector2f(0, 150), Time.FromSeconds(3));

                        bubble = world.GetObjectFromId("bubble main");
                        bubble.SetAnimationIndex(3);

                        queueDream = world.GetObjectFromId("queueDream main");
                        queueDream.SetKinematicParameters(new Vector2f(1000, 1000), new Vector2f(0f, 0f));

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(3);
                        this.moment = ThinkPhaseMoment.END;
                        break;
                    case ThinkPhaseMoment.END:
                        dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
                        dialogue.ValidateDialogue(world);

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
                //this.NodeState = NodeState.NOT_ACTIVE;
            }
        }
    }

    public enum ThinkPhaseMoment
    {
        START,
        BUBBLE_APPEARED,
        START_TIMER,
        END
    }
}

