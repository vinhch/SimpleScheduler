using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SimpleScheduler
{
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
    [XmlRoot("jobs", Namespace = "", IsNullable = false)]
    public partial class SchedulerConfigJobs
    {

        private SchedulerConfigJob[] jobField;

        /// <remarks/>
        [XmlElement("job")]
        public SchedulerConfigJob[] Job
        {
            get
            {
                return this.jobField;
            }
            set
            {
                this.jobField = value;
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
