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

        public event Action<AObject, AObject, Vector2f> TextPositionChanged;

        public event Action<AObject, AObject, AObject, Vector2f> TextUpdated;

        public event Action<AObject, string> ObjectTextChanged;

        public event Action<AObject, bool> ObjectTextStateChanged;

        public event Action<AObject, int> ObjectAnimationChanged;

        public event Action<AObject, bool> ObjectFocusChanged;

        public event Action<string, GameEvent> GameStateChanged;

        public event Action<OfficeWorld, AObject, string> InternalGameEvent;


        private Dictionary<string, AObject> objectsById;
        private List<ALayer> layers;

        private LevelNode levelNode;

        public OfficeWorld()
        {
            this.objectsById = new Dictionary<string, AObject>();

            this.layers = new List<ALayer>();

            // Level creation
            TutoLevelNode tutoLevelNode = new TutoLevelNode();
            LevelNode mainLevelNode = new LevelNode("test.xml");

            // Level links
            tutoLevelNode.NextNode = mainLevelNode;

            this.levelNode = tutoLevelNode;
        }

        public LevelNode CurrentLevel
        {
            get
            {
                return this.levelNode;
            }
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
            this.objectsById.Add(lObject.Alias, lObject);

            this.layers[indexLayer].AddObject(this, lObject);

            if (lObject is DialogueObject)
            {
                (lObject as DialogueObject).AddTokenToWorld(this, indexLayer);
            }
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
                        this.levelNode.VisitStart(this);
                    }
                }
            }

            foreach(ALayer layer in this.layers)
            {
                layer.UpdateLogic(this, timeElapsed);
            }
        }

        public void OnMouseDownOnObject(AObject lObject)
        {
            if (lObject is AnswerToken)
            {
                lObject.IsFocused = true;

                this.NotifyGameStateChanged(this.CurrentLevel.LevelName, new GameEvent(EventType.PICK_WORD, string.Empty));
            }
        }

        public void OnMouseUpOnObject(AObject lAnswer, AObject lField)
        {
            if (lAnswer is AnswerToken)
            {
                AnswerToken answerToken = lAnswer as AnswerToken;

                // test field token
                if (lField != null)
                {
                    FieldToken fieldToken = lField as FieldToken;

                    fieldToken.AssociatedToken = answerToken;

                    fieldToken.ChangeDisplayText(answerToken.Text);

                    this.NotifyGameStateChanged(this.CurrentLevel.LevelName, new GameEvent(EventType.INSERT_WORD, string.Empty));
                }
                else
                {
                    this.NotifyGameStateChanged(this.CurrentLevel.LevelName, new GameEvent(EventType.DROP_WORD, string.Empty));
                }

                answerToken.IsFocused = false;
                answerToken.SetKinematicParameters(answerToken.InitialPosition, new Vector2f(0, 0));
            }
        }

        public void OnMouseDragOnObject(AObject lObject, Vector2f mousePosition)
        {
            if (lObject is AnswerToken)
            {
                lObject.SetKinematicParameters(mousePosition, new Vector2f(0, 0));
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

        public void NotifyTextPositionChanged(AObject lObject, AObject previousObject, Vector2f newPosition)
        {
            if (this.TextPositionChanged != null)
            {
                this.TextPositionChanged(lObject, previousObject, newPosition);
            }
        }

        public void NotifyTextUpdated(AObject lObject, AObject previousObject, AObject associatedObject, Vector2f newPosition)
        {
            if (this.TextUpdated != null)
            {
                this.TextUpdated(lObject, previousObject, associatedObject,  newPosition);
            }
        }

        public void NotifyObjectTextStateChanged(AObject lObject, bool state)
        {
            if (this.ObjectTextStateChanged != null)
            {
                this.ObjectTextStateChanged(lObject, state);
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

        public void NotifyGameStateChanged(string levelName, GameEvent gameState)
        {
            if (this.GameStateChanged != null)
            {
                this.GameStateChanged(levelName, gameState);
            }
        }

        public void NotifyInternalGameEvent(AObject lObject, string details)
        {
            if (this.InternalGameEvent != null)
            {
                this.InternalGameEvent(this, lObject, details);
            }
        }
    }
}
