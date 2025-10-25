using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UMS.Colleges;
using UMS.Departments;
using UMS.Students;
using UMS.Subjects;

namespace UMS.Universities
{
    public class BaseClass
    {
        public string Name { get; set; }

        [XmlAttribute]
        public int Id { get; set; }
    }

    [XmlRoot("University")]
    public class University : BaseClass
    {
        public string Address { get; set; }

        [XmlArray("CollegeIds")]
        [XmlArrayItem("CollegeId")]
        public List<int> CollegeIds { get; set; }

        [XmlIgnore]
        public List<College> UniColleges { get; set; }

        [XmlArray()]
        public List<Department> UniDeps { get; set; }

        [XmlArray()]
        public List<Subject> UniSubs { get; set; }

        [XmlArray()]
        public List<Student> UniStds { get; set; }

        public University()
        {
            UniColleges = new List<College>();
            UniDeps = new List<Department>();
            UniSubs = new List<Subject>();
            UniStds = new List<Student>();
        }


    }
}
