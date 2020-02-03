using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Graphics.Text;

namespace GameJam2020.View.Objects
{
    public class TimerTokenObject2D: ATokenObject2D
    {
        public TimerTokenObject2D()
        {
            this.text.Style = Styles.Bold;

            this.text.CharacterSize = 70;

            this.PlayAnimation(0);
        }
    }
}
