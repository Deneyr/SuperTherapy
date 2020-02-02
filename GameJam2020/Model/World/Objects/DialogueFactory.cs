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
            foreach (DialogueToken token in data.DialoguesToken)
            {
                string[] words;
                if (token.Type != TokenType.ANSWER 
                    && token.Type != TokenType.FIELD)
                {
                    words = token.Token.Split(' ');
                }
                else
                {
                    words = new string[] { token.Token };
                }

                foreach (string word in words)
                {

                    AToken tokenObject = null;
                    switch (token.Type)
                    {
                        case TokenType.NORMAL:
                            tokenObject = new NormalToken(previousToken, word + " ");
                            break;
                        case TokenType.SANCTUARY:
                            tokenObject = new SanctuaryToken(previousToken, word + " ");
                            break;
                        case TokenType.ANSWER:
                            tokenObject = new AnswerToken(previousToken, word);
                            break;
                        case TokenType.HEADER:
                            tokenObject = new HeaderToken(previousToken, word);
                            break;
                        case TokenType.FIELD:
                            tokenObject = new FieldToken(previousToken, word);
                            break;
                    }

                    if (previousToken != null)
                    {
                        previousToken.NextToken = tokenObject;
                    }

                    dialogueObject.AddToken(tokenObject);

                    previousToken = tokenObject;
                }
            }

            return dialogueObject;
        }

        public static DialogueObject CreateDialogueFactory(string data, TokenType type)
        {
            DialogueObject dialogueObject = new DialogueObject();
            AToken previousToken = null;

            string[] words;
            if (type != TokenType.ANSWER 
                && type != TokenType.FIELD)
            {
                words = data.Split(' ');
            }
            else
            {
                words = new string[] { data };
            }

            foreach (string word in words)
            {

                AToken tokenObject = null;
                switch (type)
                {
                    case TokenType.NORMAL:
                        tokenObject = new NormalToken(previousToken, word + " ");
                        break;
                    case TokenType.SANCTUARY:
                        tokenObject = new SanctuaryToken(previousToken, word + " ");
                        break;
                    case TokenType.ANSWER:
                        tokenObject = new AnswerToken(previousToken, word);
                        break;
                    case TokenType.HEADER:
                        tokenObject = new HeaderToken(previousToken, word + " ");
                        break;
                    case TokenType.FIELD:
                        tokenObject = new FieldToken(previousToken, word);
                        break;
                }

                if (previousToken != null)
                {
                    previousToken.NextToken = tokenObject;
                }

                dialogueObject.AddToken(tokenObject);          

                previousToken = tokenObject;
            }

            return dialogueObject;
        }
    }
}
