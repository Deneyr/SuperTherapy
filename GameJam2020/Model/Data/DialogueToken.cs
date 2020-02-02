using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameJam2020.Model.Data
{
    public class DialogueToken
    {
        [XmlElement(ElementName = "Token")]
        public string Token
        {
            get;
            set;
        }

        [XmlElement(ElementName = "Type")]
        public TokenType Type
        {
            get;
            set;
        }

        public DialogueToken()
        {
            this.Token = string.Empty;

            this.Type = TokenType.NORMAL;
        }

        public DialogueToken(string str, TokenType type)
        {
            this.Token = str;

            this.Type = type;
        }
    }

    public enum TokenType
    {
        NORMAL,
        SANCTUARY,
        ANSWER,
        HEADER,
        FIELD
    }
}
