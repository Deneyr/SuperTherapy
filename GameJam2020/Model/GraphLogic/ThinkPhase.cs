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
            dialogueToubib.SetKinematicParameters(new Vector2f(-380f, dialogueToubib.GetHeight(-150)), new Vector2f(0f, 0f));

            AObject queueTalk = world.GetObjectFromId("queueTalk main");
            queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0f, 0f));

            AObject noteblock = world.GetObjectFromId("notebook main");
            (noteblock as NotebookObject).SetTransition(new Vector2f(-600, 600), new Vector2f(0, -150), Time.FromSeconds(3));

            AObject bubble = world.GetObjectFromId("bubble main");
            bubble.SetAnimationIndex(1);

            AObject timer = world.GetObjectFromId("timer main");
            timer.SetAnimationIndex(1);
            timer.SetKinematicParameters(new Vector2f(320, 200), new Vector2f(0f, 0f));

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
                        dialogue.LaunchDialogue(6);

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
                        dialogueAnswer.LaunchDialogue(6);

                        this.timeElapsed = Time.Zero;
                        this.periodPhase = Time.FromSeconds(world.CurrentLevel.Data.Timer);

                        AObject timer = world.GetObjectFromId("timer main");
                        timer.SetAnimationIndex(2);

                        this.moment = ThinkPhaseMoment.START_TIMER;
                        break;
                    case ThinkPhaseMoment.START_TIMER:

                        this.EndTimerAction(world);
                        break;
                    case ThinkPhaseMoment.END:
                        dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
                        dialogue.ValidateDialogue(world);

                        timer = world.GetObjectFromId("timer main");
                        timer.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0f, 0f));

                        this.NodeState = NodeState.NOT_ACTIVE;
                        break;
                }
            }

            if(this.moment == ThinkPhaseMoment.START_TIMER)
            {
                AToken timerToken = world.GetObjectFromId("timerToken main") as AToken;
                timerToken.DisplayText = ((int) (world.CurrentLevel.Data.Timer - this.timeElapsed.AsSeconds())).ToString();
            }
        }

        protected override void OnInternalGameEvent(OfficeWorld world, AObject lObject, AObject lObjectTo, string details)
        {
            if(this.moment == ThinkPhaseMoment.START_TIMER)
            {
                if (details.Equals("timerPassed"))
                {
                    this.EndTimerAction(world);
                }
                else if (details.Equals("association"))
                {
                    AToken tokenTo = lObjectTo as AToken;

                    if(lObject != null)
                    {
                        AToken tokenFrom = lObject as AToken;
                        tokenFrom.DisplayText = tokenFrom.Text;
                    }
                    tokenTo.DisplayText = string.Empty;
                }
            }
        }

        private void EndTimerAction(OfficeWorld world)
        {
            world.NotifyGameStateChanged(world.CurrentLevel.LevelName, new GameEvent(EventType.END_TIMER, string.Empty));

            DialogueObject dialogue = world.GetObjectFromId("dialogue toubib") as DialogueObject;
            dialogue.ResetDialogue();

            DialogueObject dialogueAnswer = world.GetObjectFromId("dialogue answer") as DialogueObject;
            dialogueAnswer.ResetDialogue();

            AObject noteblock = world.GetObjectFromId("notebook main");
            (noteblock as NotebookObject).SetTransition(new Vector2f(-600, 150), new Vector2f(0, 150), Time.FromSeconds(3));

            AObject bubble = world.GetObjectFromId("bubble main");
            bubble.SetAnimationIndex(3);

            AToken timerToken = world.GetObjectFromId("timerToken main") as AToken;
            timerToken.DisplayText = string.Empty;

            AObject queueDream = world.GetObjectFromId("queueDream main");
            queueDream.SetKinematicParameters(new Vector2f(1000, 1000), new Vector2f(0f, 0f));

            AObject timer = world.GetObjectFromId("timer main");
            timer.SetAnimationIndex(3);

            this.timeElapsed = Time.Zero;
            this.periodPhase = Time.FromSeconds(3);
            this.moment = ThinkPhaseMoment.END;
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

