using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public class HeaderTokenObject2D: ATokenObject2D
    {
        public HeaderTokenObject2D()
        {
            this.text.Style = Text.Styles.Bold;

            this.text.CharacterSize = 110;
        }
    }
}
