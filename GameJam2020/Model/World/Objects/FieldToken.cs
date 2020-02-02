using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class FieldToken : AToken
    {
        private string trueText;

        private AnswerToken associatedToken;
        private AnswerToken previousAssociatedToken;

        public FieldToken(AToken previousToken, string text) : base(previousToken, "fieldToken", fillUserText(text))
        {
            this.trueText = text;

            this.associatedToken = null;
            this.previousAssociatedToken = null;
        }

        public AnswerToken AssociatedToken
        {
            get
            {
                return this.associatedToken;
            }
            set
            {
                this.associatedToken = value;
            }
        }

        private static string fillUserText(string text)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                builder.Append("_");
            }

            return builder.ToString();
        }

        public void ChangeDisplayText(string newString)
        {
            this.displayedText = newString;
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time timeElapsed)
        {
            if(this.associatedToken != this.previousAssociatedToken)
            {
                if (this.associatedToken != null)
                {
                    AToken nextToken = this.NextToken;

                    int i = 0;
                    while (nextToken != null)
                    {
                        if(i == 0)
                        {
                            officeWorld.NotifyTextUpdated(nextToken, nextToken.PreviousToken, this.associatedToken, this.InitialPosition);
                        }
                        else
                        {
                            officeWorld.NotifyTextUpdated(nextToken, nextToken.PreviousToken, null, this.InitialPosition);
                        }

                        i++;
                        nextToken = nextToken.NextToken;
                    }

                    officeWorld.NotifyInternalGameEvent(this, "association");
                }

                this.associatedToken = this.previousAssociatedToken;
            }

            base.UpdateLogic(officeWorld, timeElapsed);
        }
    }
}
