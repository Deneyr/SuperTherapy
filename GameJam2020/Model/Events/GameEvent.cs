using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.Events
{
    public class GameEvent
    {
        public EventType Type{
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public GameEvent()
        {
            this.Type = EventType.NULL;

            this.Details = string.Empty;
        }

    }

    public enum EventType
    {
        NULL,
        ACTION,
    }
}
