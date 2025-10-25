using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UMS.Departments;
using UMS.Subjects;
using UMS.Universities;


namespace UMS.Students
{
    [XmlRoot]
    public class Student : BaseClass
    {
        public int StdDepId { get; set; }

        [XmlIgnore]
        public Department StdDep { get; set; }

        [XmlIgnore]
        public Dictionary<Subject, double> StdMarks { get; set; }

        [XmlArray("Marks")]
        public List<SerializedMark> SerializedMarks { get; set; } = new List<SerializedMark>();

        public void PrepareForSerialization()
        {
            SerializedMarks.Clear();
            foreach (var pair in StdMarks)
                SerializedMarks.Add(new SerializedMark { SubjectId = pair.Key.Id, Score = pair.Value });
        }

        public class SerializedMark
        {
            [XmlAttribute]
            public int SubjectId { get; set; }

            [XmlAttribute]
            public double Score { get; set; }
        }

        public Student ()
        {
            StdMarks = new Dictionary<Subject, double>();
        }

    }
}
