using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class DialogueObject : AObject
    {
        private List<List<AToken>> tokensList;
        private int currentTokenRowIndex;
        private int currentTokenIndex;

        private int dialogueLength;
        private float rowHeight;

        private bool isDialogueLaunched;

        public DialogueObject() : base("dialogue")
        {
            this.tokensList = new List<List<AToken>>();
            this.tokensList.Add(new List<AToken>());

            this.currentTokenIndex = -1;
            this.currentTokenRowIndex = 0;

            this.isDialogueLaunched = false;

            this.dialogueLength = 30;
            this.rowHeight = 50f;
            this.position = new Vector2f(0.1f, 0.1f);
        }

        public float WidthDialogue
        {
            get
            {
                return this.dialogueLength;
            }
        }

        public void AddToken(AToken token)
        {
            List<AToken> currentList = this.tokensList.Last();

            int nbLetter = 0;
            foreach(AToken tokenInList in currentList)
            {
                nbLetter += tokenInList.Text.Length;
            }

            if (nbLetter > this.WidthDialogue){
                currentList = new List<AToken>();
                this.tokensList.Add(currentList);
                token.PreviousToken = null;
            }

            currentList.Add(token);
        }

        public void AddTokenToWorld(OfficeWorld world, int indexLayer)
        {
            foreach(List<AToken> tokens in this.tokensList)
            {
                foreach (AToken token in tokens)
                {
                    world.AddObject(token, indexLayer);

                    token.SetKinematicParameters(new Vector2f(-10, -10), new Vector2f(0, 0));
                }
            }
        }

        public bool IsDialogueFull
        {
            get
            {
                return this.currentTokenRowIndex >= this.tokensList.Count;
            }
        }

        public void LaunchDialogue()
        {
            this.currentTokenIndex = -1;
            this.currentTokenRowIndex = 0;

            this.isDialogueLaunched = true;
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time TimeElapsed)
        {
            if (this.isDialogueLaunched && this.currentTokenRowIndex < this.tokensList.Count)
            {
                if (this.currentTokenIndex < 0 || this.tokensList[this.currentTokenRowIndex][this.currentTokenIndex].IsTextFull)
                {
                    this.currentTokenIndex++;

                    if (this.currentTokenIndex >= this.tokensList[this.currentTokenRowIndex].Count)
                    {
                        this.currentTokenRowIndex++;
                        this.currentTokenIndex = 0;
                    }

                    if (this.currentTokenRowIndex < this.tokensList.Count && this.currentTokenIndex < this.tokensList[this.currentTokenRowIndex].Count)
                    {
                        Vector2f newPosition = new Vector2f(this.position.X, this.position.Y + this.currentTokenRowIndex * this.rowHeight);

                        this.tokensList[this.currentTokenRowIndex][this.currentTokenIndex].SetKinematicParameters(newPosition, new Vector2f(0, 0));
                        this.tokensList[this.currentTokenRowIndex][this.currentTokenIndex].LaunchText();
                    }
                }
            }

            if(this.IsDialogueFull && this.isDialogueLaunched)
            {
                this.isDialogueLaunched = false;
            }
        }
    }
}
