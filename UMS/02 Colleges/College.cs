using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UMS.Departments;
using UMS.Universities;

namespace UMS.Colleges
{
    [XmlRoot("College")]
    public class College : BaseClass
    {
        public int CollUniId { get; set; }

        [XmlIgnore]
        public University CollUni { get; set; }

        [XmlArray("Departments")]
        [XmlArrayItem("Department")]
        public List<Department> CollDeps { get; set; }

        public College()
        {
            CollDeps = new List<Department>();
        }
    }
}
