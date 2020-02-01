using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameJam2020.Model.Data
{
    [XmlRoot]
    public class LevelData
    {
        [XmlElement(ElementName = "PatientDialogue")]
        public Dialogue PatientDialogue
        {
            get;
            set;
        }

        [XmlElement(ElementName = "ToubibDialogue")]
        public Dialogue ToubibDialogue
        {
            get;
            set;
        }

        [XmlElement(ElementName = "AnswerTokens")]
        public Dialogue AnswerTokens
        {
            get;
            set;
        }

        [XmlElement(ElementName = "PatientSuccessAnswer")]
        public string PatientSuccessAnswer
        {
            get;
            set;
        }

        [XmlElement(ElementName = "PatientFailAnswer")]
        public string PatientFailAnswer
        {
            get;
            set;
        }

        public LevelData()
        {
            this.PatientDialogue = new Dialogue();

            this.ToubibDialogue = new Dialogue();

            this.AnswerTokens = new Dialogue();
        }

    }
}
