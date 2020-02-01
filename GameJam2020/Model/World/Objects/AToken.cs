using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public abstract class AToken : AObject
    {
        private static int nbInstances; 

        protected string text;

        private int indexText;
        private float speedText;
        private float nbCharacterToAdd;

        private bool isTextLaunched;

        private float policeWidth;

        private int idInstance;

        private AToken previousToken;

        private float widthDialogue;

        static AToken()
        {
            AToken.nbInstances = 0;
        }

        public AToken(AToken previousToken, string id, string text) : base(id)
        {
            this.idInstance = AToken.nbInstances++;

            this.previousToken = previousToken;

            this.text = text;

            this.indexText = 0;
            this.nbCharacterToAdd = 0;
            this.speedText = 6;

            this.isTextLaunched = false;

            this.policeWidth = 0.025f;
        }

        public AToken PreviousToken
        {
            get
            {
                return this.previousToken;
            }
            set
            {
                this.previousToken = value;
            }
        }


        public string Text
        {
            get {
                return this.text;
            }
        }

        public void LaunchText()
        {
            this.indexText = 0;

            this.isTextLaunched = true;
        }

        public override string Alias
        {
            get
            {
                return this.Id + " " + this.idInstance;
            }
        }

        public bool IsTextFull
        {
            get
            {
                return this.indexText >= this.text.Length;
            }
        }

        public float LengthFullText
        {
            get
            {
                return this.text.Length * this.policeWidth;
            }
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {
            if (this.previousPosition.X != this.position.X
                || this.previousPosition.Y != this.position.Y)
            {
                officeWorld.NotifyTextPositionChanged(this, this.previousToken, this.position);

                this.previousPosition = new Vector2f(this.position.X, this.position.Y);
            }

            if (this.isTextLaunched && this.speedText > 0 && this.indexText < this.text.Length)
            {
                this.nbCharacterToAdd += this.speedText * timeElapsed.AsSeconds();

                int nbCharacterToAddInteger = (int)this.nbCharacterToAdd;

                this.indexText += nbCharacterToAddInteger;

                this.indexText = Math.Min(this.text.Length, this.indexText);

                this.nbCharacterToAdd -= nbCharacterToAddInteger;

                officeWorld.NotifyObjectTextChanged(this, this.text.Substring(0, this.indexText));
            }

            if (this.IsTextFull && this.isTextLaunched)
            {
                this.isTextLaunched = false;
            }
        }

    }
}
