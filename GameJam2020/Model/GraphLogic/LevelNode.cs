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

        private bool isSuccess;

        public LevelNode()
        {
            this.pathLevel = string.Empty;

            this.phaseNodes = new List<APhaseNode>();
            this.currentPhaseNode = null;

            this.isSuccess = false;
        }

        public LevelNode(string pathLevel)
        {
            this.pathLevel = pathLevel;

            this.phaseNodes = new List<APhaseNode>();

            APhaseNode prePhase = new PrePhase();
            APhaseNode startPhase = new StartPhase();
            APhaseNode explainPhase = new ExplainPhase();
            APhaseNode thinkPhase = new ThinkPhase();
            APhaseNode exposePhase = new ExposePhase();
            APhaseNode resolvePhase = new ResolvePhase();

            prePhase.NextNode = startPhase;
            startPhase.NextNode = explainPhase;
            explainPhase.NextNode = thinkPhase;
            thinkPhase.NextNode = exposePhase;
            exposePhase.NextNode = resolvePhase;

            this.phaseNodes.Add(prePhase);
            this.phaseNodes.Add(startPhase);
            this.phaseNodes.Add(explainPhase);
            this.phaseNodes.Add(exposePhase);
            this.phaseNodes.Add(resolvePhase);
            this.currentPhaseNode = null;

            this.isSuccess = false;
        }

        public bool IsSuccess
        {
            get
            {
                return this.isSuccess;
            }
        }

        public virtual string LevelName
        {
            get
            {
                return "level";
            }
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
            NotebookObject notebook = new NotebookObject();
            notebook.Alias = "main";
            BubbleObject bubble = new BubbleObject();
            bubble.Alias = "main";
            TimerObject timer = new TimerObject();
            timer.Alias = "main";

            QueueTalkObject queueTalk = new QueueTalkObject();
            queueTalk.Alias = "main";
            QueueDreamObject queueDream = new QueueDreamObject();
            queueDream.Alias = "main";

            //TestObject test = new TestObject();

            DialogueObject dialoguePatient = DialogueFactory.CreateDialogueFactory(60, this.levelData.PatientDialogue);
            dialoguePatient.Alias = "patient";

            DialogueObject dialogueToubib = DialogueFactory.CreateDialogueFactory(60, this.levelData.ToubibDialogue);
            dialogueToubib.Alias = "toubib";

            DialogueObject dialogueAnswer = DialogueFactory.CreateDialogueFactory(30, this.levelData.AnswerTokens);
            dialogueAnswer.Alias = "answer";

            DialogueObject dialogueSuccessAnswer = DialogueFactory.CreateDialogueFactory(60, this.levelData.PatientSuccessAnswer, TokenType.NORMAL);
            dialogueSuccessAnswer.Alias = "successAnswer";

            DialogueObject dialogueFailAnswer = DialogueFactory.CreateDialogueFactory(60, this.levelData.PatientFailAnswer, TokenType.NORMAL);
            dialogueFailAnswer.Alias = "failAnswer";

            DialogueObject dialogueComing = DialogueFactory.CreateDialogueFactory(30, "Hum, Entrez ...", TokenType.NORMAL);
            dialogueComing.Alias = "coming";

            AToken timerToken = DialogueFactory.CreateToken(string.Empty, TokenType.TIMER);
            timerToken.Alias = "main";

            // Create layers
            Layer background = new Layer();
            Layer middleground = new Layer();
            Layer foreground = new Layer();
            Layer textLayer = new Layer();
            Layer answerLayer = new Layer();

            // Add Resources
            List<string> resourcesToLoad = new List<string>();
            resourcesToLoad.Add(this.LevelName);

            resourcesToLoad.Add(office.Id);

            resourcesToLoad.Add(toubib.Id);
            resourcesToLoad.Add(patient.Id);

            resourcesToLoad.Add(notebook.Id);
            resourcesToLoad.Add(bubble.Id);

            resourcesToLoad.Add(queueTalk.Id);
            resourcesToLoad.Add(queueDream.Id);

            resourcesToLoad.Add(timer.Id);

            resourcesToLoad.Add("normalToken");
            resourcesToLoad.Add("sanctuaryToken");
            resourcesToLoad.Add("answerToken");

            resourcesToLoad.Add("lampClipped");
            resourcesToLoad.Add("wordPicked");
            resourcesToLoad.Add("bubbleClosed");
            resourcesToLoad.Add("bubbleOpened");
            resourcesToLoad.Add("wordInserted");
            resourcesToLoad.Add("wordDroped");

            resourcesToLoad.Add("dialoguePatient");
            resourcesToLoad.Add("dialogueToubib");
            resourcesToLoad.Add("dialogueReflexion");
            resourcesToLoad.Add("dialoguePatientSuccess");
            resourcesToLoad.Add("dialoguePatientFail");
            resourcesToLoad.Add("doorKnock");
            resourcesToLoad.Add("endTimer");

            resourcesToLoad.Add("validationSuccess");
            resourcesToLoad.Add("validationFail");
            /*resourcesToLoad.Add(patient.Id);
            resourcesToLoad.Add(toubib.Id);*/
            world.NotifyResourcesToLoad(resourcesToLoad);

            // Add Layers
            world.AddLayer(background);
            world.AddLayer(middleground);
            world.AddLayer(foreground);
            world.AddLayer(textLayer);
            world.AddLayer(answerLayer);

            // Add Objects
            /*world.AddObject(test, 0);
            world.AddObject(patient, 0);
            world.AddObject(toubib, 0);*/
            world.AddObject(office, 0);

            world.AddObject(toubib, 1);
            world.AddObject(patient, 1);

            world.AddObject(queueTalk, 2);
            world.AddObject(queueDream, 2);

            world.AddObject(timer, 2);
            world.AddObject(timerToken, 3);

            world.AddObject(notebook, 2);
            world.AddObject(bubble, 2);

            world.AddObject(dialoguePatient, 3);
            world.AddObject(dialogueToubib, 3);
            world.AddObject(dialogueAnswer, 4);
            world.AddObject(dialogueFailAnswer, 3);
            world.AddObject(dialogueSuccessAnswer, 3);
            world.AddObject(dialogueComing, 3);

            // Set Object Position.
            office.SetKinematicParameters(new Vector2f(0, 0), new Vector2f(0, 0));

            toubib.SetKinematicParameters(new Vector2f(150, 160), new Vector2f(0, 0));
            patient.SetKinematicParameters(new Vector2f(-550, 140), new Vector2f(0, 0));

            notebook.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));
            bubble.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));

            queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));
            queueDream.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));

            timer.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));
            timerToken.SetKinematicParameters(new Vector2f(400, 260), new Vector2f(0f, 0f));

            queueTalk.SetAnimationIndex(1);
            queueDream.SetAnimationIndex(1);
        }

        public override void VisitEnd(OfficeWorld world)
        {
            world.NbPatient++;
            if (this.isSuccess)
            {
                world.NbHappyPatient++;
            }

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
                    if(this.currentPhaseNode is ResolvePhase)
                    {
                        this.isSuccess = (this.currentPhaseNode as ResolvePhase).IsSuccess;
                    }

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
