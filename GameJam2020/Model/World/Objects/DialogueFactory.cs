﻿using GameJam2020.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public static class DialogueFactory
    {
        public static DialogueObject CreateDialogueFactory(int dialogueLength, Dialogue data)
        {
            DialogueObject dialogueObject = new DialogueObject(dialogueLength);

            AToken previousToken = null;
            DialogueToken previousDataToken = null;
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
                            if(previousDataToken != null && (previousDataToken.Type == TokenType.FIELD || previousDataToken.Type == TokenType.ANSWER))
                            {
                                tokenObject = new NormalToken(previousToken, " " + word + " ");
                            }
                            else
                            {
                                tokenObject = new NormalToken(previousToken, word + " ");
                            }
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
                    previousDataToken = token;
                }
            }

            return dialogueObject;
        }

        public static DialogueObject CreateDialogueFactory(int dialogueLength, string data, TokenType type)
        {
            DialogueObject dialogueObject = new DialogueObject(dialogueLength);
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

        public static AToken CreateToken(string data, TokenType type)
        {
            AToken tokenObject = null;
            switch (type)
            {
                case TokenType.NORMAL:
                    tokenObject = new NormalToken(null, data);
                    break;
                case TokenType.SANCTUARY:
                    tokenObject = new SanctuaryToken(null, data);
                    break;
                case TokenType.ANSWER:
                    tokenObject = new AnswerToken(null, data);
                    break;
                case TokenType.HEADER:
                    tokenObject = new HeaderToken(null, data);
                    break;
                case TokenType.FIELD:
                    tokenObject = new FieldToken(null, data);
                    break;
                case TokenType.TIMER:
                    tokenObject = new TimerTokenObject(null, data);
                    break;
            }

            return tokenObject;
        }
    }
}
