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
        protected string text;
        protected string previousDisplayedText;
        protected string displayedText;

        private int indexText;
        private float speedText;
        private float speedFactor;
        private float nbCharacterToAdd;

        private bool isTextLaunched;

        private AToken previousToken;
        private AToken nextToken;

        public AToken(AToken previousToken, string id, string text) : base(id)
        {
            this.previousToken = previousToken;
            this.nextToken = null;

            this.text = text;
            this.previousDisplayedText = string.Empty;
            this.displayedText = string.Empty;

            this.indexText = 0;
            this.nbCharacterToAdd = 0;
            this.speedText = 8;
            this.speedFactor = 1;

            this.isTextLaunched = false;
        }

        public AToken NextToken
        {
            get
            {
                return this.nextToken;
            }
            set
            {
                this.nextToken = value;
            }
        }

        public Vector2f InitialPosition
        {
            get;
            set;
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

        public float SpeedFactor
        {
            get
            {
                return this.speedFactor;
            }
            set
            {
                this.speedFactor = value;
            }
        }


        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public string DisplayText
        {
            get
            {
                return this.displayedText;
            }
            set
            {
                this.displayedText = value;
            }
        }

        public void LaunchText(float speedFactor)
        {
            this.indexText = 0;

            this.speedFactor = speedFactor;

            this.isTextLaunched = true;
        }

        public bool IsTextFull
        {
            get
            {
                return this.indexText >= this.text.Length;
            }
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {
            if (this.previousPosition.X != this.position.X
                || this.previousPosition.Y != this.position.Y)
            {
                if (this.isFocused == false)
                {
                    officeWorld.NotifyTextPositionChanged(this, this.previousToken, this.position);
                }
                else
                {
                    officeWorld.NotifyObjectPositionChanged(this, this.position);
                }
                this.previousPosition = new Vector2f(this.position.X, this.position.Y);
            }

            if (this.isFocused != this.previousIsFocused)
            {
                officeWorld.NotifyObjectFocusChanged(this, this.isFocused);

                this.previousIsFocused = this.isFocused;
            }

            if(this.displayedText.Equals(this.previousDisplayedText) == false){
                officeWorld.NotifyObjectTextChanged(this, this.displayedText);

                this.previousDisplayedText = this.displayedText;

                AToken nextToken = this.NextToken;
                if (nextToken != null && nextToken.IsTextFull)
                {
                    while (nextToken != null)
                    {
                        nextToken.SetKinematicParameters(nextToken.InitialPosition, new SFML.System.Vector2f(0, 0));
                        nextToken = nextToken.NextToken;
                    }
                }
            }

            if (this.isTextLaunched && this.speedText > 0 && this.indexText < this.text.Length)
            {
                this.nbCharacterToAdd += this.speedText * timeElapsed.AsSeconds() * this.speedFactor;

                int nbCharacterToAddInteger = (int)this.nbCharacterToAdd;

                this.indexText += nbCharacterToAddInteger;

                this.indexText = Math.Min(this.text.Length, this.indexText);

                this.nbCharacterToAdd -= nbCharacterToAddInteger;

                this.displayedText = this.text.Substring(0, this.indexText);
            }

            if (this.IsTextFull && this.isTextLaunched)
            {
                this.isTextLaunched = false;
            }
        }

    }
}
