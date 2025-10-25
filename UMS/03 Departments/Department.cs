using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UMS.Colleges;
using UMS.Students;
using UMS.Subjects;
using UMS.Universities;

namespace UMS.Departments
{
    [XmlRoot]
    public class Department : BaseClass
    {

        public int DepCollegeId { get; set; }

        [XmlIgnore]
        public College DepCollege { get; set; }

        [XmlArray("Students")]
        public List<Student> DepStds { get; set; }

        [XmlArray("Subjects")]
        public List<Subject> DepSubjects { get; set; }

        public Department()
        {
            DepSubjects = new List<Subject>();
            DepStds = new List<Student>();
        }
    }
}
