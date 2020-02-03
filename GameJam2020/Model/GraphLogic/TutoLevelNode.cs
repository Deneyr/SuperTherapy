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
    public class TutoLevelNode : LevelNode
    {
        public TutoLevelNode()
        {
            this.phaseNodes.Add(new TutoPhase());
        }

        public override void VisitStart(OfficeWorld world)
        {
            this.NodeState = NodeState.ACTIVE;

            world.InternalGameEvent += this.OnInternalGameEvent;

            // Create Objects
            DarkOfficeObject office = new DarkOfficeObject();

            ArrowObject arrow = new ArrowObject();
            BubbleHeaderObject bubbleHeader = new BubbleHeaderObject();
            BubbleTutoObject bubbleTuto = new BubbleTutoObject();

            DialogueObject header = DialogueFactory.CreateDialogueFactory("Docteur IPSO", TokenType.HEADER);
            header.Alias = "header";
            DialogueObject answer = DialogueFactory.CreateDialogueFactory("Déplacer", TokenType.ANSWER);
            answer.Alias = "answer";
            DialogueObject question = DialogueFactory.CreateDialogueFactory("Déplacer", TokenType.FIELD);
            question.Alias = "question";
            DialogueObject text = DialogueFactory.CreateDialogueFactory("  pour commencer !", TokenType.NORMAL);
            text.Alias = "text";

            // Create layers
            Layer background = new Layer();
            Layer middleground = new Layer();
            Layer foreground = new Layer();
            Layer textLayer = new Layer();
            Layer answerLayer = new Layer();

            // Add Resources
            List<string> resourcesToLoad = new List<string>();
            resourcesToLoad.Add(office.Id);

            resourcesToLoad.Add(arrow.Id);
            resourcesToLoad.Add(bubbleHeader.Id);
            resourcesToLoad.Add(bubbleTuto.Id);

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

            world.AddObject(header, 3);
            world.AddObject(question, 3);
            world.AddObject(answer, 4);
            world.AddObject(text, 3);

            // Set Object Position.
            office.SetKinematicParameters(new Vector2f(0, 0), new Vector2f(0, 0));

            arrow.SetKinematicParameters(new Vector2f(-200, 50), new Vector2f(0, 0));

            header.SetKinematicParameters(new Vector2f(-160, -260), new Vector2f(0, 0));
            header.LaunchDialogue(2);

            answer.SetKinematicParameters(new Vector2f(-250, -30), new Vector2f(0, 0));
            answer.LaunchDialogue(2);

            question.SetKinematicParameters(new Vector2f(120, -30), new Vector2f(0, 0));
            question.LaunchDialogue(2);

            text.SetKinematicParameters(new Vector2f(220, -30), new Vector2f(0, 0));
            text.LaunchDialogue(2);

            bubbleHeader.SetKinematicParameters(new Vector2f(-240, -320), new Vector2f(0, 0));
            bubbleTuto.SetKinematicParameters(new Vector2f(80, -70), new Vector2f(0, 0));
            //arrow.IsFocused = true;
            bubbleHeader.SetAnimationIndex(1);
        }

        public override void VisitEnd(OfficeWorld world)
        {
            base.VisitEnd(world);
        }
    }
}
