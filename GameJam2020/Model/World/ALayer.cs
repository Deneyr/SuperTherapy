using GameJam2020.Model.World.Objects;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World
{
    public class ALayer
    {
        private List<AObject> objectsInsideLayer;

        public ALayer()
        {
            this.objectsInsideLayer = new List<AObject>();
        }

        public void AddObject(OfficeWorld world, AObject lObject)
        {
            this.objectsInsideLayer.Add(lObject);

            world.NotifyObjectCreated(this, lObject);
        }

        public virtual void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {
            foreach(AObject lObject in this.objectsInsideLayer)
            {
                lObject.UpdateLogic(officeWorld, timeElapsed);
            }
        }

        public void Dispose(OfficeWorld world)
        {
            foreach(AObject lObject in this.objectsInsideLayer)
            {
                lObject.Dispose(world);

                world.NotifyObjectDestroyed(this, lObject);
            }

            this.objectsInsideLayer.Clear();
        }
    }
}
