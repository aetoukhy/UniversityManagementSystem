using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UMS.Colleges;
using UMS.Departments;
using UMS.Students;
using UMS.Universities;

namespace UMS.Subjects
{
    [XmlRoot]
    public class Subject : BaseClass
    {
        public int SubjectDepId { get; set; }

        [XmlIgnore]
        public Department SubjectDep { get; set; }

        public double FullMark { get; set; }

    }

}
