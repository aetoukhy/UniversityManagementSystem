# University Management System (UMS)

### Overview
This project is a C# console-based application developed as part of the KAITECH BIM Application Development course.  
It provides a structured and modular approach to managing university data, including universities, colleges, departments, subjects, and students.  
The system demonstrates key programming concepts such as object-oriented design, data serialization, and persistent storage using XML.

---

### Features
- **Full CRUD Operations:** Create, Read, Update, and Delete for all entities.
- **Entity Relationships:**  
  - Assign Colleges to Universities  
  - Assign Departments to Colleges  
  - Assign Students and Subjects to Departments  
- **Evaluation Module:**  
  Automatically evaluates and classifies each College or University based on student performance.
- **XML Data Persistence:**  
  Save and load all entered data to and from XML files for long-term storage.
- **Console-Based Navigation:**  
  A simple and intuitive text-based menu for managing data.

---

### Technologies Used
- **Language:** C# (.NET Framework)
- **Development Environment:** Visual Studio
- **Serialization Format:** XML
- **Programming Concepts:**  
  - Object-Oriented Programming (OOP)  
  - Data Serialization and Deserialization  
  - File Handling  
  - Data Relationships and Referencing

---

### Project Structure
UMS/
│
├── 01 Universities/
│ └── University.cs
│
├── 02 Colleges/
│ └── College.cs
│
├── 03 Departments/
│ └── Department.cs
│
├── 04 Subjects/
│ └── Subject.cs
│
├── 05 Students/
│ └── Student.cs
│
├── Helpers/
│ └── XmlManager.cs
│
├── Data.cs
├── Navigation.cs
├── Program.cs
├── App.config
└── .gitignore

---

### How It Works
1. The user interacts with a console menu to manage data entities.  
2. All relationships (e.g., student-to-department, department-to-college) are dynamically built through object references.  
3. The `XmlManager` class handles saving and loading data through XML serialization.  
4. On load, all object relationships are automatically rebuilt using ID mapping and reference restoration logic.  
5. Evaluation modules process student grades to classify academic entities accordingly.

---

### Key Learning Outcomes
This project strengthened understanding of:
- Object-oriented design and data modeling
- Managing interdependent data structures
- Persistent data management through XML serialization
- Building modular and maintainable C# applications

---

### Author
**Ahmed ElToukhy**  
BIM Application Development – KAITECH  
2025
