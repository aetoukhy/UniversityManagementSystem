using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UMS.Universities;
using UMS.Colleges;
using UMS.Departments;
using UMS.Subjects;
using UMS.Students;

namespace UMS
{
    public static class XmlManager
    {
        private static readonly string FolderPath = "XMLData";

        static XmlManager()
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
        }

        public static void Save<T>(T data, string fileName)
        {
            try
            {
                string fullPath = Path.Combine(FolderPath, fileName);
                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(fs, data);
                }
                Console.WriteLine($"{fileName} saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving {fileName}: {ex.Message}");
            }
        }

        public static T Load<T>(string fileName)
        {
            try
            {
                string fullPath = Path.Combine(FolderPath, fileName);
                if (!File.Exists(fullPath)) return default(T);

                using (FileStream fs = new FileStream(fullPath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {fileName}: {ex.Message}");
                return default(T);
            }
        }

        public static void SaveAll()
        {
            Data.ConvertAssignedObjectsToIds();

            Save(Data.Unis, "Universities.xml");
            Save(Data.Colleges, "Colleges.xml");
            Save(Data.Departments, "Departments.xml");
            Save(Data.Subjects, "Subjects.xml");
            Save(Data.Students, "Students.xml");

            Console.WriteLine("\nAll data saved successfully.\n");
        }

        public static void LoadAll()
        {
            Data.Unis = Load<List<University>>("Universities.xml") ?? new List<University>();
            Data.Colleges = Load<List<College>>("Colleges.xml") ?? new List<College>();
            Data.Departments = Load<List<Department>>("Departments.xml") ?? new List<Department>();
            Data.Subjects = Load<List<Subject>>("Subjects.xml") ?? new List<Subject>();
            Data.Students = Load<List<Student>>("Students.xml") ?? new List<Student>();

            Data.ConvertIdsToAssignedObjects();

            Data.RebuildRelations();

            SyncIdCounters();

            Console.WriteLine("\nAll data loaded, linked, and relationships rebuilt successfully.\n");
        }

        public static void SyncIdCounters()
        {
            ManageUni.idCounter = Data.Unis.Any() ? Data.Unis.Max(u => u.Id) : 0;
            ManageCollege.idCounter = Data.Colleges.Any() ? Data.Colleges.Max(c => c.Id) : 0;
            ManageDep.idCounter = Data.Departments.Any() ? Data.Departments.Max(d => d.Id) : 0;
            ManageSubject.idCounter = Data.Subjects.Any() ? Data.Subjects.Max(s => s.Id) : 0;
            ManageStudent.idCounter = Data.Students.Any() ? Data.Students.Max(s => s.Id) : 0;
        }
    }
}
