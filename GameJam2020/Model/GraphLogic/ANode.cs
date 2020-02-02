using GameJam2020.Model.World;
using GameJam2020.Model.World.Objects;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.GraphLogic
{
    public class ANode
    {
        public ANode NextNode
        {
            get;
            set;
        }

        public NodeState NodeState
        {
            get;
            set;
        }

        public ANode()
        {
            this.NextNode = null;

            this.NodeState = NodeState.NOT_ACTIVE;
        }

        public virtual void VisitStart(OfficeWorld world)
        {
            this.NodeState = NodeState.ACTIVE;

            world.InternalGameEvent += this.OnInternalGameEvent;
        }

        public virtual void VisitEnd(OfficeWorld world)
        {
            world.InternalGameEvent -= this.OnInternalGameEvent;
        }

        public virtual void UpdateLogic(OfficeWorld world, Time timeElapsed)
        {

        }

        protected virtual void OnInternalGameEvent(OfficeWorld world, AObject lObject, string details)
        {
            
        }

    }

    public enum NodeState
    {
        NOT_ACTIVE,
        ACTIVE
    }
}
