using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class TimerTokenObject : AToken
    {
        public TimerTokenObject(AToken previousToken, string text) : base(previousToken, "timerToken", text)
        {
        }
    }
}
