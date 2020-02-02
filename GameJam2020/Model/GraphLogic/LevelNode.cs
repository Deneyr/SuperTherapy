using GameJam2020.Model.Data;
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
    public class LevelNode: ANode
    {
        protected string pathLevel;

        protected List<APhaseNode> phaseNodes;
        protected APhaseNode currentPhaseNode;

        protected LevelData levelData;

        public LevelNode()
        {
            this.pathLevel = string.Empty;

            this.phaseNodes = new List<APhaseNode>();
            this.currentPhaseNode = null;
        }

        public LevelNode(string pathLevel)
        {
            this.pathLevel = pathLevel;

            this.phaseNodes = new List<APhaseNode>();

            this.phaseNodes.Add(new StartPhase());
            this.currentPhaseNode = null;
        }

        public LevelData Data
        {
            get
            {
                return this.levelData;
            }
        }

        public override void VisitStart(OfficeWorld world)
        {
            base.VisitStart(world);

            this.levelData = Serializer.Deserialize(this.pathLevel);

            // Create Objects
            OfficeObject office = new OfficeObject();

            PatientObject patient = new PatientObject();
            patient.Alias = "main";
            ToubibObject toubib = new ToubibObject();
            toubib.Alias = "main";

            //TestObject test = new TestObject();

            DialogueObject dialoguePatient = DialogueFactory.CreateDialogueFactory(this.levelData.PatientDialogue);
            dialoguePatient.Alias = "patient";

            // Create layers
            Layer background = new Layer();
            Layer middleground = new Layer();
            Layer foreground = new Layer();

            // Add Resources
            List<string> resourcesToLoad = new List<string>();
            resourcesToLoad.Add(office.Id);

            resourcesToLoad.Add(toubib.Id);
            resourcesToLoad.Add(patient.Id);

            resourcesToLoad.Add("normalToken");
            resourcesToLoad.Add("sanctuaryToken");
            resourcesToLoad.Add("answerToken");
            /*resourcesToLoad.Add(patient.Id);
            resourcesToLoad.Add(toubib.Id);*/
            world.NotifyResourcesToLoad(resourcesToLoad);

            // Add Layers
            world.AddLayer(background);
            world.AddLayer(middleground);
            world.AddLayer(foreground);

            // Add Objects
            /*world.AddObject(test, 0);
            world.AddObject(patient, 0);
            world.AddObject(toubib, 0);*/
            world.AddObject(office, 0);

            world.AddObject(toubib, 1);
            world.AddObject(patient, 1);

            world.AddObject(dialoguePatient, 2);

            // Set Object Position.
            office.SetKinematicParameters(new Vector2f(0, 0), new Vector2f(0, 0));

            toubib.SetKinematicParameters(new Vector2f(150, 160), new Vector2f(0, 0));
            patient.SetKinematicParameters(new Vector2f(-550, 140), new Vector2f(0, 0));
        }

        public override void VisitEnd(OfficeWorld world)
        {
            world.FlushLayers();

            base.VisitEnd(world);
        }

        public override void UpdateLogic(OfficeWorld world, Time timeElapsed)
        {
            if(this.NodeState == NodeState.ACTIVE)
            {
                if(this.currentPhaseNode == null)
                {
                    this.currentPhaseNode = this.phaseNodes[0];

                    this.currentPhaseNode.VisitStart(world);
                }
                else if(this.currentPhaseNode.NodeState == NodeState.NOT_ACTIVE)
                {
                    this.currentPhaseNode.VisitEnd(world);

                    if(this.currentPhaseNode.NextNode == null)
                    {
                        this.currentPhaseNode = null;
                    }
                    else
                    {
                        this.currentPhaseNode = this.currentPhaseNode.NextNode as APhaseNode;

                        this.currentPhaseNode.VisitStart(world);
                    }
                }

                if(this.currentPhaseNode != null)
                {
                    this.currentPhaseNode.UpdateLogic(world, timeElapsed);
                }
                else
                {
                    this.NodeState = NodeState.NOT_ACTIVE;
                }
            }
        }

    }
}
