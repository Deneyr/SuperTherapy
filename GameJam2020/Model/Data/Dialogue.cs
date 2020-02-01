using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameJam2020.Model.Data
{
    public class Dialogue
    {
        [XmlArray("DialogueTokens")]
        [XmlArrayItem("Token")]
        public List<DialogueToken> DialoguesToken
        {
            get;
            set;
        }

        [XmlIgnore]
        private int currentIndexToken;

        public Dialogue()
        {
            this.DialoguesToken = new List<DialogueToken>();
        }

        public void AddToken(DialogueToken token)
        {
            this.DialoguesToken.Add(token);
        }
    }
}
