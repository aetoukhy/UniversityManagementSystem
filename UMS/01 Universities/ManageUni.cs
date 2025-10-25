using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Colleges;
using UMS.Departments;
using UMS.Students;
using UMS.Subjects;

namespace UMS.Universities
{
    static class ManageUni
    {
        private static int _navigator = 1;

        public static int idCounter = 0;

        public static void CreateUni()
        {
            idCounter++;
            int id = idCounter;

            Console.WriteLine("Enter University Name: ");
            string name = Console.ReadLine();

            if (Data.Unis.Any(uni => uni.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("A University with that name already exists.");
                if (Helper.Footer(_navigator) == 1) CreateUni();
                return;
            }

            Console.WriteLine("Enter University Address: ");
            string address = Console.ReadLine();

            University university = new University() { Id = id, Name = name, Address = address };
            Console.WriteLine($"\n{university.Name} University created successfully with ID: {university.Id}.");
            Data.Unis.Add(university);

            if (Helper.Footer(_navigator) == 1) CreateUni();
        }

        public static University RetrieveUni(int uniId)
        {
            return Data.Unis.FirstOrDefault(uni => uni.Id == uniId);
        }

        internal static void PrintUni(University uni)
        {
            if (uni != null)
            {
                Console.WriteLine($"ID: {uni.Id}");
                Console.WriteLine($"Name: {uni.Name}");
                Console.WriteLine($"Address: {uni.Address}");
                Console.WriteLine($"Colleges:");
                foreach (var college in uni.UniColleges)
                {
                    Console.WriteLine($"- {college.Name}");
                }
            }
            Console.WriteLine("-----------------------------");
        }

        public static void PrintAllUnisData()
        {
            foreach (var uni in Data.Unis)
            {
                PrintUni(uni);
            }

            if (Helper.Footer(_navigator) == 1) PrintAllUnisData();
        }

        public static void UpdateUni()
        {
            Helper.DisplayExisting(Data.Unis, "Universities");
            Console.WriteLine("Enter University ID to update:");
            int uniId = Helper.IdInput(Console.ReadLine());
            University uni = RetrieveUni(uniId);

            if (uni != null)
            {
                Console.WriteLine("Select action number from the list below:");
                Console.WriteLine("1. Update University name");
                Console.WriteLine("2. Update University address");
                Console.WriteLine("3. Update University Colleges");


                int[] options1 = { 1, 2, 3 };
                int i = Helper.SelectFrom(Console.ReadLine(), options1);

                switch (i)
                {
                    case 1:
                        Console.WriteLine("Enter new name: ");

                        string new_name = Console.ReadLine();
                        Data.Unis.FirstOrDefault(_uni => _uni.Id == uniId).Name = new_name;
                        break;

                    case 2:
                        Console.WriteLine("Enter new address: ");
                        string new_address = Console.ReadLine();
                        Data.Unis.FirstOrDefault(_uni => _uni.Id == uniId).Address = new_address;
                        break;

                    case 3:
                        Helper.DisplayExisting(Data.Colleges, "Colleges");
                        Console.WriteLine("Select action number from the list below: ");
                        Console.WriteLine("1. Add a College");
                        Console.WriteLine("2. Delete a College");

                        int[] options2 = { 1, 2 };
                        int j = Helper.SelectFrom(Console.ReadLine(), options2);

                        Console.WriteLine("Enter College ID: ");
                        int collegeID = Helper.IdInput(Console.ReadLine());
                        College college = ManageCollege.RetrieveCollege(collegeID);

                        if (college != null)
                        {
                            if (j == 1)
                            {
                                if (!uni.UniColleges.Contains(college))
                                {
                                    uni.UniColleges.Add(college);
                                    if (college.CollUni == null)
                                    {
                                        Console.WriteLine($"\n{college.Name} College is now assigned to {uni.Name} University.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\n{college.Name} College moved from {college.CollUni.Name} University to {uni.Name} University.");
                                    }
                                    college.CollUni = uni;
                                }
                                else
                                {
                                    Console.WriteLine($"\n{college.Name} College already exists in {uni.Name} University.");
                                }

                            }
                            else if (j == 2)
                            {
                                if (uni.UniColleges.Contains(college))
                                {
                                    uni.UniColleges.Remove(college);
                                    college.CollUni = null;
                                    Console.WriteLine($"\n{college.Name} College is now removed from {uni.Name} University and not assigned to any University.");
                                }
                                else
                                {
                                    Console.WriteLine($"\n{college.Name} was not assigned to {uni.Name} University. Deletion Skipped.");
                                }
                            }
                        }
                        else Console.WriteLine("\nCollege not found");
                        break;
                }

                Console.WriteLine($"\nUniversity updated successfully.");
            }
            else Console.WriteLine("\nUniversity not found");

            if (Helper.Footer(_navigator) == 1) UpdateUni();
        }

        public static void DeleteUni()
        {
            Helper.DisplayExisting(Data.Unis, "Universities");
            Console.WriteLine("Enter University ID to delete: ");
            int uniId = Helper.IdInput(Console.ReadLine());
            University uni = RetrieveUni(uniId);

            if (uni != null)
            {
                Console.WriteLine($"Are you sure you want to delete {uni.Name} University?");
                Console.WriteLine($"1. Delete Permenantly");
                Console.WriteLine($"2. Cancel");

                int[] options = { 1, 2 };
                int i = Helper.SelectFrom(Console.ReadLine(), options);

                if (i == 1)
                {
                    foreach (var college in Data.Colleges)
                    {
                        if (college.CollUni == uni)
                        {
                            college.CollUni = null;
                        }
                        Console.WriteLine($"{college.Name} is not assigned to a University now.");
                    }
                    Data.Unis.Remove(uni);
                    Console.WriteLine("\nUniversity deleted successfully.");
                }
                else if (i == 2)
                {
                    Console.WriteLine("\nDeletion process cancelled.");
                }
            }
            else Console.WriteLine("\nUniversity not found.");

            if (Helper.Footer(_navigator) == 1) DeleteUni();
        }

        public static void AssignCollegesToUni(University uni)
        {
            Console.Write("Specify number of Colleges to be assigned: ");
            int numOfColleges = Helper.IntInput(Console.ReadLine());
            if (numOfColleges > ManageCollege.idCounter)
            {
                Console.WriteLine($"Number exceeds existing Colleges.\nLimit is set to: {ManageCollege.idCounter}");
                numOfColleges = ManageCollege.idCounter;
                if (numOfColleges == 0)
                {
                    Console.Write("Operation skipped.");
                    if (Helper.Footer(1) == 1) AssignCollegesToUni(uni);
                    return;
                }
            }

            Helper.DisplayExisting(Data.Colleges, "Colleges");
            Console.WriteLine("Enter College IDs:");

            for (int i = 0; i < numOfColleges; i++)
            {
                Console.Write($"College no.{i + 1}: ");
                int collegeId = Helper.IdInput(Console.ReadLine());
                College college = ManageCollege.RetrieveCollege(collegeId);

                if (college == null)
                {
                    Console.WriteLine($"College with ID {collegeId} not found.");
                    continue;
                }
                
                if (college.CollUni != null)
                {
                    college.CollUni.UniColleges.Remove(college);
                    Console.WriteLine($"{college.Name} is moved from {college.CollUni.Name} to {uni.Name}.");
                }

                college.CollUni = uni;
                if (!uni.UniColleges.Contains(college))
                {
                    uni.UniColleges.Add(college);
                }

                Console.WriteLine($"\n{college.Name} College is now assigned to {uni.Name} University.");
            }

            if (Helper.Footer(1) == 1) AssignCollegesToUni(uni);
        }

        public static void AssignCollegesToAnyUni()
        {
            Helper.DisplayExisting(Data.Unis, "Universities");
            Console.WriteLine("Enter University ID to assign colleges: ");

            int uniId = Helper.IdInput(Console.ReadLine());
            University uni = RetrieveUni(uniId);

            if (uni != null)
            {
                AssignCollegesToUni(uni);
            }
            else
            {
                Console.WriteLine($"University with ID {uniId} not found.");
            }

            if (Helper.Footer(1) == 1) AssignCollegesToAnyUni();
        }

        public static void EvaluateUniversity()
        {
            Helper.DisplayExisting(Data.Unis, "Universities");
            Console.WriteLine("Enter University ID to evaluate: ");
            int uniId = Helper.IdInput(Console.ReadLine());
            University uni = RetrieveUni(uniId);
            if (uni != null)
            {
                var stds = uni.UniColleges
                .SelectMany(coll => coll.CollDeps)
                .SelectMany(dep => dep.DepStds)
                .ToList();

                if (stds.Count == 0)
                {
                    Console.WriteLine($"No students found in {uni.Name} University.");
                    return;
                }

                double avgMark = stds.Average(std => ManageStudent.AvgMark(std.Id));
                int passed = stds.Count(std => ManageStudent.StdPassed(std.Id));
                double rate = (double)passed / stds.Count * 100;
                string classification = Helper.GetClassification(rate);

                Console.WriteLine($"Classification System: A: ≥90%, B: ≥80%, C: ≥70%, D: ≥60%, E: <60%");
                Console.WriteLine($"University: {uni.Name}");
                Console.WriteLine($"Total Students: {stds.Count}");
                Console.WriteLine($"Average Student Mark: {avgMark:F2}");
                Console.WriteLine($"Passed: {passed}");
                Console.WriteLine($"Failed: {stds.Count - passed}");
                Console.WriteLine($"Success Rate: {rate:F2}%");
                Console.WriteLine($"Classification: {classification}");
                Console.WriteLine("-----------------------------------");
            }
            else
            {
                Console.WriteLine($"Univeristy with ID {uniId} not found");
            }

            if (Helper.Footer(_navigator) == 1) EvaluateUniversity();

        }

    }
}