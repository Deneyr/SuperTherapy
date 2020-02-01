using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using GameJam2020.Model.Data;

namespace GameJam2020
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Dialogue dialogue = new Dialogue();

            DialogueToken token = new DialogueToken("test ", TokenType.NORMAL);
            DialogueToken token2 = new DialogueToken("tëst, ..Nouveau ", TokenType.FIELD);
            dialogue.AddToken(token);
            dialogue.AddToken(token2);
            dialogue.AddToken(token);
            dialogue.AddToken(token2);
            dialogue.AddToken(token);

            LevelData levelData = new LevelData();
            levelData.PatientDialogue = dialogue;
            levelData.ToubibDialogue = dialogue;

            levelData = Serializer.Deserialize("test.xml");

            Serializer.Serialize(levelData, "test2.xml");

            Console.WriteLine(dialogue.DialoguesToken[0].Token);*/

            MainWindow window = new MainWindow();

            window.Run();
        }
    }
}
