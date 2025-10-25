using System;
using UMS.Universities;
using UMS.Colleges;
using UMS.Departments;
using UMS.Subjects;
using UMS.Students;

namespace UMS
{
    internal class Navigation
    {
        internal static void HomePage()
        {
            Console.WriteLine(" --------- ");
            Console.WriteLine("| Home Page |");
            Console.WriteLine(" --------- ");
            Console.WriteLine("Select a Category number:\n");
            Console.WriteLine("1. Universities Management");
            Console.WriteLine("2. Colleges Management");
            Console.WriteLine("3. Departments Management");
            Console.WriteLine("4. Subjects Management");
            Console.WriteLine("5. Students Management");
            Console.WriteLine("6. Data (Save/Load XML)");
            Console.WriteLine("7. Exit\n");

            int[] options = { 1, 2, 3, 4, 5, 6, 7 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 1: UniversitiesManagement(); break;
                case 2: CollegesManagement(); break;
                case 3: DepartmentsManagement(); break;
                case 4: SubjectsManagement(); break;
                case 5: StudentsManagement(); break;
                case 6: DataManagement(); break;
                case 7:
                    Console.WriteLine("\nSave data?");
                    Console.WriteLine("1. Save");
                    Console.WriteLine("2. Don't Save");

                    int[] options2 = { 1, 2 };
                    int j = Helper.SelectFrom(Console.ReadLine(), options2);

                    if (j == 1)
                    {
                        Console.WriteLine("\nSaving data before exit...");
                        XmlManager.SaveAll();
                        Console.WriteLine("Data saved successfully. Exiting program...");
                        Environment.Exit(0);
                    }
                    else Environment.Exit(0);
                    break;
            }
        }

        internal static void UniversitiesManagement()
        {
            Console.WriteLine("\n ----------------------- ");
            Console.WriteLine("| Universities Management |");
            Console.WriteLine(" ----------------------- ");
            Console.WriteLine("Select Action number: (or Press 0 for HomePage)\n");
            Console.WriteLine("1. Create University");
            Console.WriteLine("2. Update University");
            Console.WriteLine("3. Assign Colleges to University");
            Console.WriteLine("4. Delete University");
            Console.WriteLine("5. Print Universities Data");
            Console.WriteLine("6. Evaluate Universities\n");

            int[] options = { 0, 1, 2, 3, 4, 5, 6 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 0: HomePage(); break;
                case 1: ManageUni.CreateUni(); break;
                case 2: ManageUni.UpdateUni(); break;
                case 3: ManageUni.AssignCollegesToAnyUni(); break;
                case 4: ManageUni.DeleteUni(); break;
                case 5: ManageUni.PrintAllUnisData(); break;
                case 6: ManageUni.EvaluateUniversity(); break;
            }
        }

        internal static void CollegesManagement()
        {
            Console.WriteLine("\n ------------------- ");
            Console.WriteLine("| Colleges Management |");
            Console.WriteLine(" ------------------- ");
            Console.WriteLine("Select Action number: (or Press 0 for HomePage)\n");
            Console.WriteLine("1. Create College");
            Console.WriteLine("2. Update College");
            Console.WriteLine("3. Assign Universities to College");
            Console.WriteLine("4. Assign Departments to College");
            Console.WriteLine("5. Delete College");
            Console.WriteLine("6. Print Colleges Data");
            Console.WriteLine("7. Evaluate College\n");

            int[] options = { 0, 1, 2, 3, 4, 5, 6, 7 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 0: HomePage(); break;
                case 1: ManageCollege.CreateCollege(); break;
                case 2: ManageCollege.UpdateCollege(); break;
                case 3: ManageCollege.AssignUniToAnyCollege(); break;
                case 4: ManageCollege.AssignDepsToAnyCollege(); break;
                case 5: ManageCollege.DeleteCollege(); break;
                case 6: ManageCollege.PrintAllCollsData(); break;
                case 7: ManageCollege.EvaluateCollege(); break;
            }
        }

        internal static void DepartmentsManagement()
        {
            Console.WriteLine("\n ----------------------- ");
            Console.WriteLine("| Departments Management |");
            Console.WriteLine(" ----------------------- ");
            Console.WriteLine("Select Action number: (or Press 0 for HomePage)\n");
            Console.WriteLine("1. Create Department");
            Console.WriteLine("2. Update Department");
            Console.WriteLine("3. Assign College to Department");
            Console.WriteLine("4. Assign Subjects to Department");
            Console.WriteLine("5. Delete Department");
            Console.WriteLine("6. Print Departments Data\n");

            int[] options = { 0, 1, 2, 3, 4, 5, 6 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 0: HomePage(); break;
                case 1: ManageDep.CreateDepartment(); break;
                case 2: ManageDep.UpdateDep(); break;
                case 3: ManageDep.AssignCollegeToDep(); break;
                case 4: ManageDep.AssignSubjectsToDep(); break;
                case 5: ManageDep.DeleteDep(); break;
                case 6: ManageDep.PrintAllDepsData(); break;
            }
        }

        internal static void SubjectsManagement()
        {
            Console.WriteLine("\n ------------------- ");
            Console.WriteLine("| Subjects Management |");
            Console.WriteLine(" ------------------- ");
            Console.WriteLine("Select Action number: (or Press 0 for HomePage)\n");
            Console.WriteLine("1. Create Subject");
            Console.WriteLine("2. Update Subject");
            Console.WriteLine("3. Assign Department to Subject");
            Console.WriteLine("4. Delete Subject");
            Console.WriteLine("5. Print Subjects Data\n");

            int[] options = { 0, 1, 2, 3, 4, 5 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 0: HomePage(); break;
                case 1: ManageSubject.CreateSubject(); break;
                case 2: ManageSubject.UpdateSub(); break;
                case 3: ManageSubject.AssignDepartmentToSubject(); break;
                case 4: ManageSubject.DeleteSubject(); break;
                case 5: ManageSubject.PrintAllSubjects(); break;
            }
        }

        internal static void StudentsManagement()
        {
            Console.WriteLine("\n ------------------- ");
            Console.WriteLine("| Students Management |");
            Console.WriteLine(" ------------------- ");
            Console.WriteLine("Select Action number: (or Press 0 for HomePage)\n");
            Console.WriteLine("1. Create Student");
            Console.WriteLine("2. Assign Department to Student");
            Console.WriteLine("3. Assign Subjects to Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Print Students Data\n");

            int[] options = { 0, 1, 2, 3, 4, 5 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 0: HomePage(); break;
                case 1: ManageStudent.CreateStudent(); break;
                case 2: ManageStudent.AssignDepartmentToStudent(); break;
                case 3: ManageStudent.AssignSubjectsToStudent(); break;
                case 4: ManageStudent.DeleteStudent(); break;
                case 5: ManageStudent.PrintAllStdsData(); break;
            }
        }

        internal static void DataManagement()
        {
            Console.WriteLine("\n ---------------- ");
            Console.WriteLine("| Data (XML) Menu |");
            Console.WriteLine(" ---------------- ");
            Console.WriteLine("Select Action number: (or Press 0 for HomePage)\n");
            Console.WriteLine("1. Save All Data to XML");
            Console.WriteLine("2. Load All Data from XML\n");

            int[] options = { 0, 1, 2 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 0: HomePage(); break;
                case 1:
                    XmlManager.SaveAll();
                    Helper.Footer(0);
                    break;
                case 2:
                    XmlManager.LoadAll();
                    Helper.Footer(0);
                    break;
            }
        }

        internal static int NextAction(int navigator)
        {
            int[] options = { 0, 1, 2, 3 };
            int i = Helper.SelectFrom(Console.ReadLine(), options);

            switch (i)
            {
                case 0: HomePage(); break;
                case 1: return 1;
                case 2:
                    switch (navigator)
                    {
                        case 1: UniversitiesManagement(); break;
                        case 2: CollegesManagement(); break;
                        case 3: DepartmentsManagement(); break;
                        case 4: SubjectsManagement(); break;
                        case 5: StudentsManagement(); break;
                    }
                    break;
                case 3:
                    Console.WriteLine("\nSave data?");
                    Console.WriteLine("1. Save");
                    Console.WriteLine("2. Don't Save");

                    int[] options2 = { 1, 2 };
                    int j = Helper.SelectFrom(Console.ReadLine(), options2);

                    if (j == 1)
                    {
                        Console.WriteLine("\nSaving data before exit...");
                        XmlManager.SaveAll();
                        Console.WriteLine("Data saved successfully. Exiting program...");
                        Environment.Exit(0);
                    }
                    else Environment.Exit(0);
                    break;
            }
            return 0;
        }
    }
}
