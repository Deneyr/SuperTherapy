using GameJam2020.Model.Events;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class BubbleObject : AObject
    {
        public BubbleObject() : base("bubble")
        {
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {

            if(this.previousAnimationIndex != this.currentAnimationIndex)
            {
                if (this.currentAnimationIndex == 1)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.OPEN_BUBBLE, string.Empty));
                }
                else if(this.currentAnimationIndex == 3)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.CLOSE_BUBBLE, string.Empty));
                }
            }

            base.UpdateLogic(officeWorld, timeElapsed);
        }
    }
}
