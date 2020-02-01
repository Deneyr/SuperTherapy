using GameJam2020.Model.Events;
using GameJam2020.Model.GraphLogic;
using GameJam2020.Model.World.Objects;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World
{
    public class OfficeWorld
    {
        public event Action<List<string>> ResourcesToLoad;

        public event Action<ALayer, AObject> ObjectCreated;

        public event Action<ALayer, AObject> ObjectDestroyed;

        public event Action<ALayer> LayerCreated;

        public event Action<ALayer> LayerDestroyed;

        public event Action<AObject, Vector2f> ObjectPositionChanged;

        public event Action<AObject, string> ObjectTextChanged;

        public event Action<AObject, int> ObjectAnimationChanged;

        public event Action<AObject, bool> ObjectFocusChanged;

        public event Action<AObject, string> InternalGameEvent;


        private Dictionary<string, AObject> objectsById;
        private List<ALayer> layers;

        private LevelNode levelNode;

        public OfficeWorld()
        {
            this.objectsById = new Dictionary<string, AObject>();

            this.layers = new List<ALayer>();

            // test 
            this.levelNode = new LevelNode("test.xml");
        }

        public void StartLevel()
        {
            this.levelNode.VisitStart(this);
        }

        public void AddLayer(ALayer layer)
        {
            this.layers.Add(layer);

            this.NotifyLayerCreated(layer);
        }

        public void FlushLayers()
        {
            foreach(ALayer layer in this.layers)
            {
                layer.Dispose(this);

                this.NotifyLayerDestroyed(layer);
            }

            this.layers.Clear();

            this.objectsById.Clear();
        }

        public void AddObject(AObject lObject, int indexLayer)
        {
            this.objectsById.Add(lObject.Id, lObject);

            this.layers[indexLayer].AddObject(this, lObject);
        }

        public AObject GetObjectFromId(string Id)
        {
            return this.objectsById[Id];
        }

        public virtual void UpdateLogic(Time timeElapsed)
        {
            if(this.levelNode != null)
            {
                this.levelNode.UpdateLogic(this, timeElapsed);

                if(this.levelNode.NodeState == NodeState.NOT_ACTIVE)
                {
                    this.levelNode.VisitEnd(this);

                    if (this.levelNode.NextNode == null)
                    {
                        this.levelNode = null;
                    }
                    else
                    {
                        this.levelNode = this.levelNode.NextNode as LevelNode;
                    }
                }
            }

            foreach(ALayer layer in this.layers)
            {
                layer.UpdateLogic(this, timeElapsed);
            }
        }


        public void NotifyResourcesToLoad(List<string> resourcesToLoad)
        {
            if(this.ResourcesToLoad != null)
            {
                this.ResourcesToLoad(resourcesToLoad);
            }
        }

        public void NotifyObjectCreated(ALayer layer, AObject lObject)
        {
            if (this.ObjectCreated != null)
            {
                this.ObjectCreated(layer, lObject);
            }
        }

        public void NotifyObjectDestroyed(ALayer layer, AObject lObject)
        {
            if (this.ObjectDestroyed != null)
            {
                this.ObjectDestroyed(layer, lObject);
            }
        }

        public void NotifyLayerCreated(ALayer lLayer)
        {
            if (this.LayerCreated != null)
            {
                this.LayerCreated(lLayer);
            }
        }

        public void NotifyLayerDestroyed(ALayer lLayer)
        {
            if (this.LayerDestroyed != null)
            {
                this.LayerDestroyed(lLayer);
            }
        }

        public void NotifyObjectAnimationChanged(AObject lObject, int animationIndex)
        {
            if (this.ObjectAnimationChanged != null)
            {
                this.ObjectAnimationChanged(lObject, animationIndex);
            }
        }

        public void NotifyObjectPositionChanged(AObject lObject, Vector2f newPosition)
        {
            if (this.ObjectPositionChanged != null)
            {
                this.ObjectPositionChanged(lObject, newPosition);
            }
        }

        public void NotifyObjectFocusChanged(AObject lObject, bool newFocus)
        {
            if (this.ObjectFocusChanged != null)
            {
                this.ObjectFocusChanged(lObject, newFocus);
            }
        }

        public void NotifyObjectTextChanged(AObject lObject, string newText)
        {
            if (this.ObjectTextChanged != null)
            {
                this.ObjectTextChanged(lObject, newText);
            }
        }

        public void NotifyInternalGameEvent(AObject lObject, string details)
        {
            if (this.InternalGameEvent != null)
            {
                this.InternalGameEvent(lObject, details);
            }
        }
    }
}
