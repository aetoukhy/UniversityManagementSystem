using System;
using System.Collections.Generic;
using System.Linq;
using UMS.Departments;
using UMS.Students;
using UMS.Subjects;
using UMS.Universities;

namespace UMS.Colleges
{
    public static class ManageCollege
    {
        private static int _navigator = 2;
        public static int idCounter = 0;

        public static void CreateCollege()
        {
            idCounter++;
            int id = idCounter;

            Console.WriteLine("Enter College Name: ");
            string name = Console.ReadLine();

            College college = new College { Id = id, Name = name };
            Data.Colleges.Add(college);
            Console.WriteLine($"\n\"{college.Name}\" College created successfully with ID: {college.Id}.");

            Console.WriteLine($"{college.Name} College is not assigned to a University yet.");
            Console.WriteLine("1. Assign University now");
            Console.WriteLine("2. Assign later");
            int[] options = { 1, 2 };
            int choice = Helper.SelectFrom(Console.ReadLine(), options);

            if (choice == 1)
                AssignUni(college);

            if (Helper.Footer(_navigator) == 1) CreateCollege();
        }

        public static College RetrieveCollege(int collegeId)
        {
            return Data.Colleges.FirstOrDefault(coll => coll.Id == collegeId);
        }

        public static void PrintCollege(College college)
        {
            if (college == null)
            {
                Console.WriteLine("College not found.");
                return;
            }

            Console.WriteLine($"ID: {college.Id}");
            Console.WriteLine($"Name: {college.Name}");
            Console.WriteLine($"University: {(college.CollUni != null ? college.CollUni.Name : "Unassigned")}");
            Console.WriteLine($"Departments:");
            foreach (var dep in college.CollDeps)
                Console.WriteLine($"- {dep.Name}");

            Console.WriteLine("-----------------------------");
        }

        public static void PrintAllCollsData()
        {
            foreach (var c in Data.Colleges)
                PrintCollege(c);

            if (Helper.Footer(_navigator) == 1) PrintAllCollsData();
        }

        public static void UpdateCollege()
        {
            Helper.DisplayExisting(Data.Colleges, "Colleges");
            Console.WriteLine("Enter College ID to update:");
            int collegeId = Helper.IdInput(Console.ReadLine());
            College college = RetrieveCollege(collegeId);

            if (college == null)
            {
                Console.WriteLine("College not found.");
                if (Helper.Footer(_navigator) == 1) UpdateCollege();
                return;
            }

            Console.WriteLine("Select action number from the list below:");
            Console.WriteLine("1. Update College name");
            Console.WriteLine("2. Update University assignment");
            Console.WriteLine("3. Update Departments");
            int[] options1 = { 1, 2, 3 };
            int action = Helper.SelectFrom(Console.ReadLine(), options1);

            switch (action)
            {
                case 1:
                    Console.WriteLine("Enter new name: ");
                    college.Name = Console.ReadLine();
                    Console.WriteLine("College name updated successfully.");
                    break;

                case 2:
                    AssignUni(college);
                    break;

                case 3:
                    Console.WriteLine("1. Add Departments");
                    Console.WriteLine("2. Delete Departments");
                    int[] options3 = { 1, 2 };
                    int depChoice = Helper.SelectFrom(Console.ReadLine(), options3);

                    if (depChoice == 1) AssignDeps(college);
                    else if (depChoice == 2)
                    {
                        Helper.DisplayExisting(Data.Departments, "Departments");
                        Console.Write("Enter Department ID to delete: ");
                        int depId = Helper.IdInput(Console.ReadLine());
                        Department dep = ManageDep.RetrieveDep(depId);

                        if (dep != null && college.CollDeps.Contains(dep))
                        {
                            college.CollDeps.Remove(dep);
                            dep.DepCollege = null;
                            Console.WriteLine($"Department '{dep.Name}' removed.");
                        }
                        else Console.WriteLine("Department not found or not assigned.");
                    }
                    break;
            }

            if (Helper.Footer(_navigator) == 1) UpdateCollege();
        }

        public static void DeleteCollege()
        {
            Helper.DisplayExisting(Data.Colleges, "Colleges");
            Console.WriteLine("Enter College ID to delete:");
            int collegeId = Helper.IdInput(Console.ReadLine());
            College college = RetrieveCollege(collegeId);

            if (college == null)
            {
                Console.WriteLine("College not found.");
                if (Helper.Footer(_navigator) == 1) DeleteCollege();
                return;
            }

            Console.WriteLine($"Are you sure to delete \"{college.Name}\" College?");
            Console.WriteLine("1. Delete Permanently");
            Console.WriteLine("2. Cancel");
            int[] options = { 1, 2 };
            int choice = Helper.SelectFrom(Console.ReadLine(), options);

            if (choice == 1)
            {
                if (college.CollUni != null)
                    college.CollUni.UniColleges.Remove(college);

                foreach (var dep in Data.Departments)
                {
                    if (dep.DepCollege == college)
                        dep.DepCollege = null;
                }

                Data.Colleges.Remove(college);
                Console.WriteLine("College deleted successfully.");
            }
            else Console.WriteLine("Deletion cancelled.");

            if (Helper.Footer(_navigator) == 1) DeleteCollege();
        }

        public static void AssignUni(College college)
        {
            Helper.DisplayExisting(Data.Unis, "Universities");
            Console.WriteLine("Enter University ID to assign:");
            int uniId = Helper.IdInput(Console.ReadLine());
            University uni = ManageUni.RetrieveUni(uniId);

            if (uni == null)
            {
                Console.WriteLine($"University with ID {uniId} not found.");
                return;
            }

            if (college.CollUni != null)
            {
                if (college.CollUni == uni)
                {
                    Console.WriteLine($"{college.Name} is already assigned to {uni.Name}.");
                    return;
                }

                college.CollUni.UniColleges.Remove(college);
                Console.WriteLine($"{college.Name} moved from {college.CollUni.Name} to {uni.Name}.");
            }

            college.CollUni = uni;
            if (!uni.UniColleges.Contains(college))
                uni.UniColleges.Add(college);

            Console.WriteLine($"{college.Name} is now assigned to {uni.Name} University.");
        }

        public static void AssignUniToAnyCollege()
        {
            Helper.DisplayExisting(Data.Colleges, "Colleges");
            Console.WriteLine("Enter College ID to assign a University:");
            int collegeId = Helper.IdInput(Console.ReadLine());
            College college = RetrieveCollege(collegeId);

            if (college != null)
                AssignUni(college);
            else
                Console.WriteLine($"College with ID {collegeId} not found.");

            if (Helper.Footer(_navigator) == 1) AssignUniToAnyCollege();
        }

        public static void AssignDeps(College college)
        {
            Console.Write("Specify number of Departments to be assigned: ");
            int numOfDeps = Helper.IntInput(Console.ReadLine());
            if (numOfDeps > ManageDep.idCounter)
            {
                Console.WriteLine($"Number exceeds existing Departments.\nLimit is set to: {ManageDep.idCounter}");
                numOfDeps = ManageDep.idCounter;
                if (numOfDeps == 0)
                {
                    Console.Write("Operation skipped.");
                    if (Helper.Footer(1) == 1) AssignDeps(college);
                    return;
                }

            }

            Helper.DisplayExisting(Data.Departments, "Departments");
            Console.WriteLine("Enter Department IDs:");
            for (int i = 0; i < numOfDeps; i++)
            {
                Console.Write($"Department no.{i + 1}: ");
                int depId = Helper.IdInput(Console.ReadLine());
                Department dep = ManageDep.RetrieveDep(depId);

                if (dep == null)
                {
                    Console.WriteLine($"Department with ID {depId} not found.");
                    continue;
                }

                if (dep.DepCollege != null && dep.DepCollege != college)
                {
                    Console.WriteLine($"{dep.Name} moved from {dep.DepCollege.Name} to {college.Name}.");
                    dep.DepCollege.CollDeps.Remove(dep);
                }

                dep.DepCollege = college;
                if (!college.CollDeps.Contains(dep))
                    college.CollDeps.Add(dep);
            }
        }

        public static void AssignDepsToAnyCollege()
        {
            Helper.DisplayExisting(Data.Colleges, "Colleges");
            Console.WriteLine("Enter College ID to assign departments:");
            int collegeId = Helper.IdInput(Console.ReadLine());
            College college = RetrieveCollege(collegeId);

            if (college != null)
                AssignDeps(college);
            else
                Console.WriteLine($"College with ID {collegeId} not found.");

            if (Helper.Footer(_navigator) == 1) AssignDepsToAnyCollege();
        }

        public static void EvaluateCollege()
        {
            Helper.DisplayExisting(Data.Colleges, "Colleges");
            Console.WriteLine("Enter College ID to evaluate:");
            int collId = Helper.IdInput(Console.ReadLine());
            College college = RetrieveCollege(collId);

            if (college == null)
            {
                Console.WriteLine($"College with ID {collId} not found.");
                if (Helper.Footer(_navigator) == 1) EvaluateCollege();
                return;
            }

            var stds = college.CollDeps.SelectMany(dep => dep.DepStds).ToList();
            if (stds.Count == 0)
            {
                Console.WriteLine($"No students found in {college.Name} College.");
                return;
            }

            double avgMark = stds.Average(std => ManageStudent.AvgMark(std.Id));
            int passed = stds.Count(std => ManageStudent.StdPassed(std.Id));
            double rate = (double)passed / stds.Count * 100;
            string classification = Helper.GetClassification(rate);

            Console.WriteLine($"Classification System: A: ≥90%, B: ≥80%, C: ≥70%, D: ≥60%, E: <60%");
            Console.WriteLine($"College: {college.Name}");
            Console.WriteLine($"University: {(college.CollUni != null ? college.CollUni.Name : "Unassigned")}");
            Console.WriteLine($"Total Students: {stds.Count}");
            Console.WriteLine($"Average Student Mark: {avgMark:F2}");
            Console.WriteLine($"Passed: {passed}");
            Console.WriteLine($"Failed: {stds.Count - passed}");
            Console.WriteLine($"Success Rate: {rate:F2}%");
            Console.WriteLine($"Classification: {classification}");

            if (Helper.Footer(_navigator) == 1) EvaluateCollege();
        }
    }
}
