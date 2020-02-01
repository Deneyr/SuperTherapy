using GameJam2020.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public static class DialogueFactory
    {
        public static DialogueObject CreateDialogueFactory(Dialogue data)
        {
            DialogueObject dialogueObject = new DialogueObject();

            AToken previousToken = null;
            foreach(DialogueToken token in data.DialoguesToken)
            {
                string[] words = token.Token.Split(' ');

                foreach (string word in words) {

                    AToken tokenObject = null;
                    switch (token.Type)
                    {
                        case TokenType.NORMAL:
                            tokenObject = new NormalToken(previousToken, word + " ");
                            break;
                        case TokenType.SANCTUARY:
                            tokenObject = new SanctuaryToken(previousToken, word + " ");
                            break;
                        case TokenType.FIELD:

                            break;
                    }

                    dialogueObject.AddToken(tokenObject);

                    previousToken = tokenObject;
                }
            }

            return dialogueObject;
        }
    }
}
