using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UMS.Universities;
using UMS.Colleges;
using UMS.Departments;
using UMS.Subjects;
using UMS.Students;

namespace UMS
{
    [XmlRoot("Data")]
    public class Data
    {
        [XmlArray("Universities")]
        [XmlArrayItem("University")]
        public static List<University> Unis { get; set; }

        [XmlArray("Colleges")]
        [XmlArrayItem("College")]
        public static List<College> Colleges { get; set; }

        [XmlArray("Departments")]
        [XmlArrayItem("Department")]
        public static List<Department> Departments { get; set; }

        [XmlArray("Subjects")]
        [XmlArrayItem("Subject")]
        public static List<Subject> Subjects { get; set; }

        [XmlArray("Students")]
        [XmlArrayItem("Student")]
        public static List<Student> Students { get; set; }

        public static void Initialize()
        {
            Unis = new List<University>();
            Colleges = new List<College>();
            Departments = new List<Department>();
            Subjects = new List<Subject>();
            Students = new List<Student>();
        }

        public static void ConvertAssignedObjectsToIds()
        {
            foreach (var college in Colleges)
                college.CollUniId = college.CollUni?.Id ?? 0;

            foreach (var dep in Departments)
                dep.DepCollegeId = dep.DepCollege?.Id ?? 0;

            foreach (var sub in Subjects)
                sub.SubjectDepId = sub.SubjectDep?.Id ?? 0;

            foreach (var std in Students)
                std.StdDepId = std.StdDep?.Id ?? 0;

            foreach (var s in Students)
                s.PrepareForSerialization();
        }

        public static void ConvertIdsToAssignedObjects()
        {
            foreach (var college in Colleges)
                college.CollUni = Unis.FirstOrDefault(u => u.Id == college.CollUniId);

            foreach (var dep in Departments)
                dep.DepCollege = Colleges.FirstOrDefault(c => c.Id == dep.DepCollegeId);

            foreach (var sub in Subjects)
                sub.SubjectDep = Departments.FirstOrDefault(d => d.Id == sub.SubjectDepId);

            foreach (var std in Students)
                std.StdDep = Departments.FirstOrDefault(d => d.Id == std.StdDepId);

            foreach (var s in Students)
            {
                s.StdMarks = new Dictionary<Subject, double>();
                foreach (var mark in s.SerializedMarks)
                {
                    Subject subj = Subjects.FirstOrDefault(sub => sub.Id == mark.SubjectId);
                    if (subj != null)
                        s.StdMarks[subj] = mark.Score;
                }
            }
        }

        public static void RebuildRelations()
        {
            foreach (var uni in Unis)
                uni.UniColleges.Clear();

            foreach (var coll in Colleges)
                coll.CollDeps.Clear();

            foreach (var dep in Departments)
            {
                dep.DepSubjects.Clear();
                dep.DepStds.Clear();
            }

            foreach (var coll in Colleges)
            {
                if (coll.CollUni != null && !coll.CollUni.UniColleges.Contains(coll))
                    coll.CollUni.UniColleges.Add(coll);
            }

            foreach (var dep in Departments)
            {
                if (dep.DepCollege != null && !dep.DepCollege.CollDeps.Contains(dep))
                    dep.DepCollege.CollDeps.Add(dep);
            }

            foreach (var sub in Subjects)
            {
                if (sub.SubjectDep != null && !sub.SubjectDep.DepSubjects.Contains(sub))
                    sub.SubjectDep.DepSubjects.Add(sub);
            }

            foreach (var std in Students)
            {
                if (std.StdDep != null && !std.StdDep.DepStds.Contains(std))
                    std.StdDep.DepStds.Add(std);
            }
        }
    }
}
