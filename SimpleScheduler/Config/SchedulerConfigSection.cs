using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleScheduler
{
    public class SchedulerConfigSection : ConfigurationSection
    {
        private const string ELEMENT_NAME_JOBS = "jobs";

        private readonly XmlSerializer _objectSerializer = new XmlSerializer(typeof(SchedulerConfigJobs));

        public IList<SchedulerConfigJob> Jobs { get; private set; } = new List<SchedulerConfigJob>();

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (!elementName.Equals(ELEMENT_NAME_JOBS, StringComparison.Ordinal))
                return base.OnDeserializeUnrecognizedElement(elementName, reader);

            var jobs = _objectSerializer.Deserialize(reader) as SchedulerConfigJobs;
            if (jobs != null) Jobs = jobs.Job;

            return true;
        }
    }
}
