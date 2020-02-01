using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public class SanctuaryTokenObject2D : ATokenObject2D
    {
        public SanctuaryTokenObject2D()
        {
            this.text.OutlineColor = Color.White;

            this.text.FillColor = Color.Cyan;

            this.text.OutlineThickness = 1;

            this.text.CharacterSize = 40;

            this.PlayAnimation(0);
        }
    }
}
