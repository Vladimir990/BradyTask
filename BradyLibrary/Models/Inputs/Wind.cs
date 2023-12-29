﻿using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Models.Input
{
    public class Wind
    {
        [XmlElement("WindGenerator")]
        public List<WindGenerator> WindGenerators { get; set; }
    }
}
