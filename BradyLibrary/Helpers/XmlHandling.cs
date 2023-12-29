using BradyTask.BusinessLogic.Models.Input;
using BradyTask.BusinessLogic.Models.References;
using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Helpers
{
    public static class XmlHandling
    {
        #region Public methods
        public static GenerationReport DeserializeInput(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GenerationReport));

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return (GenerationReport)serializer.Deserialize(stream);
            }
        }

        public static ReferenceData DeserializeReferenceData(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ReferenceData));

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return (ReferenceData)serializer.Deserialize(stream);
            }
        }
        #endregion
    }
}
