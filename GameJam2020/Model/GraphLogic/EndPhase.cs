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
    public class EndPhase: APhaseNode
    {
        private Time periodPhase;
        private Time timeElapsed;

        private bool isValidated;

        public EndPhase()
        {
            this.timeElapsed = Time.Zero;

            this.periodPhase = Time.FromSeconds(1);

            this.isValidated = false;
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            world.NotifyGameStateChanged(world.CurrentLevel.LevelName, new GameEvent(EventType.START, string.Empty));
        }

        public override void VisitEnd(OfficeWorld world)
        {
            base.VisitEnd(world);
        }

        protected override void OnInternalGameEvent(OfficeWorld world, AObject lObject, AObject lObjectTo, string details)
        {
            if (details.Equals("association"))
            {
                AToken token = (lObjectTo as AToken);

                /*DialogueObject dialogue = world.GetObjectFromId((lObject as FieldToken).AssociatedToken) as DialogueObject;
                dialogue.ResetDialogue();*/
                token.DisplayText = string.Empty;

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
