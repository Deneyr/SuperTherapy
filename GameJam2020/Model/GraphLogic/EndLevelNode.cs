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
    public class EndLevelNode: LevelNode
    {
        public EndLevelNode()
        {
            this.phaseNodes.Add(new EndPhase());
        }

        public override string LevelName
        {
            get
            {
                return "endLevel";
            }
        }

        public override void VisitStart(OfficeWorld world)
        {
            this.NodeState = NodeState.ACTIVE;

            world.InternalGameEvent += this.OnInternalGameEvent;

            int nbHappyPatient = world.NbHappyPatient;

            // Create Objects
            DarkOfficeObject office = new DarkOfficeObject();

            ArrowObject arrow = new ArrowObject();
            BubbleHeaderObject bubbleHeader = new BubbleHeaderObject();
            BubbleTutoObject bubbleTuto = new BubbleTutoObject();
 
            ResultObject[] resultObjects = new ResultObject[nbHappyPatient];
            for(int i=0; i < nbHappyPatient; i++)
            {
                resultObjects[i] = new ResultObject();
            }

            //BubbleTutoObject bubbleResult = new BubbleTutoObject();

            DialogueObject header = DialogueFactory.CreateDialogueFactory(30, "Docteur IPSO", TokenType.HEADER);
            header.Alias = "header";
            DialogueObject answer = DialogueFactory.CreateDialogueFactory(30, "Déplacer", TokenType.ANSWER);
            answer.Alias = "answer";
            DialogueObject question = DialogueFactory.CreateDialogueFactory(30, "Déplacer", TokenType.FIELD);
            question.Alias = "question";
            DialogueObject text = DialogueFactory.CreateDialogueFactory(30, "  pour recommencer !", TokenType.NORMAL);
            text.Alias = "text";

            DialogueObject result;
            if(world.NbHappyPatient > 0)
            {
                result = DialogueFactory.CreateDialogueFactory(60, "Vous avez rendu " + world.NbHappyPatient + " patients heureux sur " + world.NbPatient + " !", TokenType.NORMAL);
            }
            else
            {
                result = DialogueFactory.CreateDialogueFactory(60, "Vous n'avez rendu aucun patient heureux !", TokenType.NORMAL);
            }

            result.Alias = "result";

            DialogueObject credits = DialogueFactory.CreateDialogueFactory(60, "Pierre Duchateau   Clément Romagny   François Massy", TokenType.NORMAL);
            text.Alias = "credits";

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

            resourcesToLoad.Add(arrow.Id);
            resourcesToLoad.Add(bubbleHeader.Id);
            resourcesToLoad.Add(bubbleTuto.Id);
            if (resultObjects.Count() > 0)
            {
                resourcesToLoad.Add(resultObjects[0].Id);
            }

            resourcesToLoad.Add("normalToken");
            resourcesToLoad.Add("sanctuaryToken");
            resourcesToLoad.Add("answerToken");
            resourcesToLoad.Add("wordInserted");
            resourcesToLoad.Add("wordDroped");
            resourcesToLoad.Add("wordInserted");
            resourcesToLoad.Add("wordPicked");

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

            world.AddObject(arrow, 2);
            world.AddObject(bubbleHeader, 2);
            world.AddObject(bubbleTuto, 2);
            //world.AddObject(bubbleResult, 2);
            for (int i = 0; i < nbHappyPatient; i++)
            {
                world.AddObject(resultObjects[i], 2);
            }

            world.AddObject(header, 3);
            world.AddObject(question, 3);
            world.AddObject(answer, 4);
            world.AddObject(text, 3);
            world.AddObject(credits, 3);
            world.AddObject(result, 3);

            // Set Object Position.
            office.SetKinematicParameters(new Vector2f(0, 0), new Vector2f(0, 0));

            arrow.SetKinematicParameters(new Vector2f(-220, 230), new Vector2f(0, 0));

            header.SetKinematicParameters(new Vector2f(-220, -300), new Vector2f(0, 0));
            header.LaunchDialogue(2);

            answer.SetKinematicParameters(new Vector2f(-270, 150), new Vector2f(0, 0));
            answer.LaunchDialogue(2);

            question.SetKinematicParameters(new Vector2f(100, 150), new Vector2f(0, 0));
            question.LaunchDialogue(2);

            text.SetKinematicParameters(new Vector2f(200, 150), new Vector2f(0, 0));
            text.LaunchDialogue(2);

            result.SetKinematicParameters(new Vector2f(-240, -80), new Vector2f(0, 0));
            result.LaunchDialogue(2);

            credits.SetKinematicParameters(new Vector2f(-350, 300), new Vector2f(0, 0));
            credits.LaunchDialogue(2);

            float startPositionX = -((nbHappyPatient / 2f) * 140);
            for(int i=0; i < nbHappyPatient; i++)
            {
                resultObjects[i].SetKinematicParameters(new Vector2f(startPositionX + i * 140, -20), new Vector2f(0, 0));
            }

            bubbleHeader.SetKinematicParameters(new Vector2f(-340, -360), new Vector2f(0, 0));
            bubbleTuto.SetKinematicParameters(new Vector2f(60, 110), new Vector2f(0, 0));
            //arrow.IsFocused = true;
            bubbleHeader.SetAnimationIndex(2);

            //bubbleResult.SetKinematicParameters(new Vector2f(-200, 0), new Vector2f(0, 0));
        }

        public override void VisitEnd(OfficeWorld world)
        {
            base.VisitEnd(world);

            LevelNode firstLevelNode = world.CreateGame("Levels", 5);

            // Level links
            this.NextNode = firstLevelNode;
        }
    }
}
