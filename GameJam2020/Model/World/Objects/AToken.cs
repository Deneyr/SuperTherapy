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

        private int indexText;
        private float speedText;
        private float nbCharacterToAdd;

        public AToken(string id, string text) : base(id)
        {
            this.indexText = 0;
            this.nbCharacterToAdd = 0;
        }

        public void SetTextParameters(int index, float speed)
        {
            this.indexText = index;
            this.speedText = speed;
        }

        public override void UpdateLogic(OfficeWorld officeWorld, Time TimeElapsed)
        {
            base.UpdateLogic(officeWorld, TimeElapsed);

            if (this.speedText > 0 && this.indexText < this.text.Length)
            {
                this.nbCharacterToAdd += this.speedText * TimeElapsed.AsSeconds();

                int nbCharacterToAddInteger = (int)this.nbCharacterToAdd;

                this.indexText += nbCharacterToAddInteger;

                this.nbCharacterToAdd -= nbCharacterToAddInteger;

                officeWorld.NotifyObjectTextChanged(this, this.text.Substring(0, this.indexText));
            }
        }

    }
}
