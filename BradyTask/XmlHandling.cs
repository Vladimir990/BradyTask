using BradyTask.Models.Input;
using BradyTask.Models.References;
using System.Xml.Serialization;

namespace BradyTask
{
    public static class XmlHandling
    {
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
    }
}
