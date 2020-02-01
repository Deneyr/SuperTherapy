using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameJam2020.Model.Data
{
    public static class Serializer
    {
        public static void Serialize(LevelData dialogue, string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LevelData));

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                xmlSerializer.Serialize(streamWriter, dialogue);
            }
        }

        public static LevelData Deserialize(string path)
        {
            LevelData levelData = null;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LevelData));
            using (StreamReader streamReader = new StreamReader(path))
            {
                levelData = xmlSerializer.Deserialize(streamReader) as LevelData;
            }

            return levelData;
        }

    }
}
