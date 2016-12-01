using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("schedulerConfig", Namespace = "", IsNullable = false)]
    public partial class SchedulerConfig
    {

        private SchedulerConfigJob[] jobsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("jobs")]
        [System.Xml.Serialization.XmlArrayItemAttribute("job", IsNullable = false)]
        public SchedulerConfigJob[] Jobs
        {
            get
            {
                return this.jobsField;
            }
            set
            {
                this.jobsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulerConfigJob
    {

        private string nameField;

        private string typeField;

        private bool enabledField;

        private bool stopOnErrorField;

        private int secondsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("type")]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("enabled")]
        public bool Enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("stopOnError")]
        public bool StopOnError
        {
            get
            {
                return this.stopOnErrorField;
            }
            set
            {
                this.stopOnErrorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("seconds")]
        public int Seconds
        {
            get
            {
                return this.secondsField;
            }
            set
            {
                this.secondsField = value;
            }
        }
    }
}
