﻿using GameJam2020.Model.Data;
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

            APhaseNode startPhase = new StartPhase();
            APhaseNode explainPhase = new ExplainPhase();
            APhaseNode thinkPhase = new ThinkPhase();
            APhaseNode exposePhase = new ExposePhase();
            APhaseNode resolvePhase = new ResolvePhase();

            startPhase.NextNode = explainPhase;
            explainPhase.NextNode = thinkPhase;
            thinkPhase.NextNode = exposePhase;
            exposePhase.NextNode = resolvePhase;

            this.phaseNodes.Add(startPhase);
            this.phaseNodes.Add(explainPhase);
            this.phaseNodes.Add(exposePhase);
            this.phaseNodes.Add(resolvePhase);
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
            NotebookObject notebook = new NotebookObject();
            notebook.Alias = "main";
            BubbleObject bubble = new BubbleObject();
            bubble.Alias = "main";

            QueueTalkObject queueTalk = new QueueTalkObject();
            queueTalk.Alias = "main";
            QueueDreamObject queueDream = new QueueDreamObject();
            queueDream.Alias = "main";

            //TestObject test = new TestObject();

            DialogueObject dialoguePatient = DialogueFactory.CreateDialogueFactory(this.levelData.PatientDialogue);
            dialoguePatient.Alias = "patient";

            DialogueObject dialogueToubib = DialogueFactory.CreateDialogueFactory(this.levelData.ToubibDialogue);
            dialogueToubib.Alias = "toubib";

            DialogueObject dialogueAnswer = DialogueFactory.CreateDialogueFactory(this.levelData.AnswerTokens);
            dialogueAnswer.Alias = "answer";

            DialogueObject dialogueSuccessAnswer = DialogueFactory.CreateDialogueFactory(this.levelData.PatientSuccessAnswer, TokenType.NORMAL);
            dialogueSuccessAnswer.Alias = "successAnswer";

            DialogueObject dialogueFailAnswer = DialogueFactory.CreateDialogueFactory(this.levelData.PatientFailAnswer, TokenType.NORMAL);
            dialogueFailAnswer.Alias = "failAnswer";

            // Create layers
            Layer background = new Layer();
            Layer middleground = new Layer();
            Layer foreground = new Layer();
            Layer textLayer = new Layer();
            Layer answerLayer = new Layer();

            // Add Resources
            List<string> resourcesToLoad = new List<string>();
            resourcesToLoad.Add(office.Id);

            resourcesToLoad.Add(toubib.Id);
            resourcesToLoad.Add(patient.Id);

            resourcesToLoad.Add(notebook.Id);
            resourcesToLoad.Add(bubble.Id);

            resourcesToLoad.Add(queueTalk.Id);
            resourcesToLoad.Add(queueDream.Id);

            resourcesToLoad.Add("normalToken");
            resourcesToLoad.Add("sanctuaryToken");
            resourcesToLoad.Add("answerToken");

            resourcesToLoad.Add("level");
            resourcesToLoad.Add("lampClipped");
            resourcesToLoad.Add("moved");
            resourcesToLoad.Add("bubbleClosed");
            resourcesToLoad.Add("bubbleOpened");
            resourcesToLoad.Add("wordPlaced");
            resourcesToLoad.Add("wordDroped");
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

            world.AddObject(notebook, 2);
            world.AddObject(bubble, 2);

            world.AddObject(dialoguePatient, 3);
            world.AddObject(dialogueToubib, 3);
            world.AddObject(dialogueAnswer, 4);
            world.AddObject(dialogueFailAnswer, 3);
            world.AddObject(dialogueSuccessAnswer, 3);

            // Set Object Position.
            office.SetKinematicParameters(new Vector2f(0, 0), new Vector2f(0, 0));

            toubib.SetKinematicParameters(new Vector2f(150, 160), new Vector2f(0, 0));
            patient.SetKinematicParameters(new Vector2f(-550, 140), new Vector2f(0, 0));

            notebook.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));
            bubble.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));

            queueTalk.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));
            queueDream.SetKinematicParameters(new Vector2f(10000, 10000), new Vector2f(0, 0));

            queueTalk.SetAnimationIndex(1);
            queueDream.SetAnimationIndex(1);
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
