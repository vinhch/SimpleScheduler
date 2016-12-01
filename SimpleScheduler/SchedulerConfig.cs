using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleScheduler
{
    public class SchedulerConfigSection : ConfigurationSection
    {
        private const string ELEMENT_NAME_JOB = "job";

        private readonly XmlSerializer _objectSerializer = new XmlSerializer(typeof(SchedulerConfigJob));

        public IList<SchedulerConfigJob> Jobs { get; private set; } = new List<SchedulerConfigJob>();

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (!elementName.Equals(ELEMENT_NAME_JOB, StringComparison.Ordinal))
                return base.OnDeserializeUnrecognizedElement(elementName, reader);

            Jobs.Add(_objectSerializer.Deserialize(reader) as SchedulerConfigJob);
            return true;
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("schedulerConfig", Namespace = "", IsNullable = false)]
    public class SchedulerConfig
    {

        private SchedulerConfigJob[] jobsField;

        /// <remarks/>
        [XmlArray("jobs")]
        [XmlArrayItem("job", IsNullable = false)]
        public SchedulerConfigJob[] Jobs
        {
            get
            {
                return jobsField;
            }
            set
            {
                jobsField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("job", Namespace = "", IsNullable = false)]
    public class SchedulerConfigJob
    {

        private string nameField;

        private string typeField;

        private bool enabledField;

        private bool stopOnErrorField;

        private int secondsField;

        /// <remarks/>
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("type")]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("enabled")]
        public bool Enabled
        {
            get
            {
                return enabledField;
            }
            set
            {
                enabledField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("stopOnError")]
        public bool StopOnError
        {
            get
            {
                return stopOnErrorField;
            }
            set
            {
                stopOnErrorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("seconds")]
        public int Seconds
        {
            get
            {
                return secondsField;
            }
            set
            {
                secondsField = value;
            }
        }
    }
}
