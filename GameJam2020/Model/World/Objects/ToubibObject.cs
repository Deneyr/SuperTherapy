using GameJam2020.Model.Events;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class ToubibObject : AObject
    {
        public ToubibObject() : base("toubib")
        {
            
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {

            if (this.previousAnimationIndex != this.currentAnimationIndex)
            {
                if (this.previousAnimationIndex == 2)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.END_TALK, "dialogueReflexion"));
                }

                if (this.currentAnimationIndex == 2)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.START_TALK, "dialogueReflexion"));
                }

                if (this.previousAnimationIndex == 3)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.END_TALK, "dialogueToubib"));
                }

                if (this.currentAnimationIndex == 3)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.START_TALK, "dialogueToubib"));
                }
            }

            base.UpdateLogic(officeWorld, timeElapsed);
        }
    }
}
