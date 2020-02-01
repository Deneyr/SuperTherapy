using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Objects
{
    public class LayerObject2D: AObject2D
    {
        private List<AObject2D> objects2D;

        public LayerObject2D()
        {
            this.objects2D = new List<AObject2D>();
        }

        public override void DrawIn(RenderWindow window)
        {
            foreach (AObject2D object2D in this.objects2D)
            {
                object2D.DrawIn(window);
            }
        }

        public void AddObject2D(AObject2D object2D)
        {
            this.objects2D.Add(object2D);
        }

        public void RemoveObject2D(AObject2D object2D)
        {
            this.objects2D.Remove(object2D);
        }
    }
}
