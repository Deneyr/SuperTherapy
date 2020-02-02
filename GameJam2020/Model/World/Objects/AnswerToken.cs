using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.Model.World.Objects
{
    public class AnswerToken : AToken
    {
        public AnswerToken(AToken previousToken, string text) : base(previousToken, "answerToken", text)
        {

        }
    }
}
