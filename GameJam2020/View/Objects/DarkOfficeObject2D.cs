using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public class DarkOfficeObject2D: OfficeObject2D
    {
        public DarkOfficeObject2D()
        {
            this.sprite.Scale = new SFML.System.Vector2f(1f, 1f);
        }
    }
}
