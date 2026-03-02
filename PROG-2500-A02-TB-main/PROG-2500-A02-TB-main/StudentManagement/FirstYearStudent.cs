using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace StudentManagement
{
    // Assuming the base class Student and interface IStudentData are already defined elsewhere
    // public abstract class Student { ... }
    // public interface IStudentData { ... }  (not shown in your snippet)

    public class FirstYearStudent : Student, IStudentData
    {
        private List<FirstYearStudent> fstudents;

        // Discussion about the properties:
        // These are classic auto-implemented properties (C# 3.0+).
        // The compiler automatically creates a private backing field for us.
        // Advantages: concise, clean, suitable when no extra logic/validation is needed.
        // We could later change them to full properties with logic without breaking the public contract.
        public int yearOfStudy { get; set; }
        public string workTermStatus { get; set; }

        // How to use the parent constructor to initialize (explanation of :base(...) syntax)
        // The colon (:) in C# has multiple uses. Here it is used for constructor chaining / base class initialization.
        // :base(...) means: "call the constructor of the immediate base class (Student) and pass these arguments to it".
        // This is the correct and recommended way to initialize inherited fields/properties when the base class has no parameterless constructor.
        // Placing initialization of derived-class-specific fields after the base call is good style.
        public FirstYearStudent(string fname, string lname, int age, string program, string workTerm)
            : base(fname, lname, age, program)
        {
            yearOfStudy = 1;               // First-year students always start at year 1
            workTermStatus = workTerm;     // Store the additional first-year-specific information
        }

        // Why 'override' here?
        // The base class Student declares public abstract void DisplayStudentDetails();
        // Because it is abstract, every concrete derived class MUST provide an implementation.
        // The 'override' keyword is REQUIRED in C# when you are providing that implementation.
        // Without 'override', you would get a compiler error (CS0534 or similar).
        // Using 'override' also enables polymorphic behavior: when you call student.DisplayStudentDetails()
        // through a Student reference, the correct derived-class version runs.
        public override void DisplayStudentDetails()
        {
            MessageBox.Show(
                $"Student Name: {Fname} {LName}\n" +
                $"Age: {Age}\n" +
                $"Program: {sProgram}");
        }

        /// <summary>
        /// Loads student records from StudentMaster.txt and returns them as a List<FirstYearStudent>.
        /// Uses very basic CSV parsing — suitable only for simple academic/demo projects.
        /// </summary>
        public List<FirstYearStudent> Load()
        {
            MessageBox.Show("Loading student data...");

            fstudents = new List<FirstYearStudent>();

            try
            {
                string file = "StudentMaster.txt";

                using (StreamReader reader = new StreamReader(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 6) // very strict format check
                        {
                            string firstName = parts[0].Trim();
                            string lastName = parts[1].Trim();
                            int age = int.Parse(parts[2].Trim());
                            string program = parts[3].Trim();
                            // parts[4] is yearOfStudy — ignored during load (recalculated as 1)
                            string workStatus = parts[5].Trim();

                            fstudents.Add(new FirstYearStudent(firstName, lastName, age, program, workStatus));
                        }
                    }
                }

                MessageBox.Show("Student data loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}");
            }

            return fstudents;
        }

        /// <summary>
        /// Saves the provided list of FirstYearStudent objects back to StudentMaster.txt in CSV format.
        /// Overwrites the entire file — no append or merge logic.
        /// </summary>
        public void Save(List<FirstYearStudent> students)
        {
            string StudentMasterFile = "StudentMaster.txt";
            fstudents = students; // Updates the internal list reference (not strictly necessary)

            try
            {
                using (StreamWriter writer = new StreamWriter(StudentMasterFile))
                {
                    foreach (var student in fstudents)
                    {
                        // Safe cast — since we only ever add FirstYearStudent objects, this is safe
                        FirstYearStudent st1 = (FirstYearStudent)student;

                        writer.WriteLine(
                            $"{st1.Fname},{st1.LName},{st1.Age},{st1.sProgram}," +
                            $"{st1.yearOfStudy},{st1.workTermStatus}");
                    }
                }

                MessageBox.Show("Student data saved to file successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}");
            }
        }
    }
}

/*
References (APA 7th ed.) - Educational resources used as reference for this student management example:

Microsoft Learn. (n.d.). Abstract and sealed classes and class members (C# programming guide). 
    https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members

Microsoft Learn. (n.d.). Inheritance - derive types to create more specialized behavior. 
    https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/inheritance

Microsoft Learn. (n.d.). Automatically implemented properties. 
    https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties

Microsoft Learn. (n.d.). Properties (C# programming guide). 
    https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties

Microsoft Learn. (n.d.). override (C# reference). 
    https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override

Microsoft Learn. (n.d.). Knowing when to use override and new keywords (C# programming guide). 
    https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/knowing-when-to-use-override-and-new-keywords

Microsoft Learn. (n.d.). Create a Windows Forms app with C#. 
    https://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio

Microsoft Learn. (n.d.). Windows Forms overview. 
    https://learn.microsoft.com/en-us/dotnet/desktop/winforms/overview

These links point to the most current official Microsoft documentation (retrieved February 2026). 
They explain core OOP features (abstract classes, inheritance, override), property patterns, 
and basic WinForms structure used in this project.
*/