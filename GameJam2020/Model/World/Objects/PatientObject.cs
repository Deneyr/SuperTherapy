using GameJam2020.Model.Events;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class PatientObject : AObject
    {
        public PatientObject() : base("patient")
        {
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {

            if (this.previousAnimationIndex != this.currentAnimationIndex)
            {
                if (this.previousAnimationIndex == 4)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.END_TALK, "dialoguePatientFail"));
                }

                if (this.currentAnimationIndex == 4)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.START_TALK, "dialoguePatientFail"));
                }

                if (this.previousAnimationIndex == 5)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.END_TALK, "dialoguePatientSuccess"));
                }

                if (this.currentAnimationIndex == 5)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.START_TALK, "dialoguePatientSuccess"));
                }

                if (this.previousAnimationIndex == 2)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.END_TALK, "dialoguePatient"));
                }

                if (this.currentAnimationIndex == 2)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.START_TALK, "dialoguePatient"));
                }

                if (this.currentAnimationIndex == 1)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.DOOR_OPEN, "dialoguePatient"));
                }

                if (this.currentAnimationIndex == 6)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.DOOR_KNOCK, "dialoguePatient"));
                }

                if (this.currentAnimationIndex == 7)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.VALIDATION, "fail"));
                }

                if (this.currentAnimationIndex == 8)
                {
                    officeWorld.NotifyGameStateChanged(officeWorld.CurrentLevel.LevelName, new GameEvent(EventType.VALIDATION, "success"));
                }
            }

            base.UpdateLogic(officeWorld, timeElapsed);
        }
    }
}
