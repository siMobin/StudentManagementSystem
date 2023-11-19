using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Dynamic;
using Microsoft.VisualBasic;
// unused imports, but can be needed in other machines

using Microsoft.Data.SqlClient;

internal class Program
{
    // Database connection string
    private static readonly string connectionString = "Data Source=localhost,51609;Initial Catalog=StudentManagementSystem;Integrated Security=True;Encrypt=False";

    // Main function that serves as the entry point of the program.
    // It displays a menu of options to the user and performs the corresponding actions based on the user's choice.
    private static void Main()
    {

        char choice;
        do
        {
            int Equal = 15;
            Console.Clear();
            Console.WriteLine("Student Management System");
            Console.WriteLine("\n" + new string('=', Equal) + "MAIN MENU" + new string('=', Equal) + "\n");

            Console.WriteLine("1. Create Student");
            Console.WriteLine("2. Show Students");
            Console.WriteLine("3. Search Student");
            Console.WriteLine("4. Update Student");
            Console.WriteLine("5. Delete Student");
            Console.WriteLine("6. Assign course to student");
            Console.WriteLine("7. Insert Marks and Calculate GPA");
            Console.WriteLine("8. View courses");
            Console.WriteLine("9. View departments");
            Console.WriteLine("0. Exit");
            Console.WriteLine(new string('=', Equal * 3));
            Console.Write("Enter your choice: ");
            choice = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (choice)
            {
                case '1':
                    CreateStudent();
                    break;
                case '2':
                    ShowStudents();
                    break;
                case '3':
                    SearchStudent();
                    break;
                case '4':
                    UpdateStudent();
                    break;
                case '5':
                    DeleteStudent();
                    break;
                case '6':
                    AddCourseToStudent();
                    break;
                case '7':
                    InsertMarksAndCalculateGPA();
                    break;
                case '8':
                    ShowAvailableCoursesAndInsertNewCourses();
                    break;
                case '9':
                    ShowAvailableDepartmentsAndInsertNewDepartments();
                    break;
                case '0':
                    Console.WriteLine("Exiting. Thank you!");
                    break;
                default:                                         // recheck
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        } while (choice != '0');
    }


    // Creates a new student record in the database.
    // Prompts the user to enter the student's ID, first name, last name, date of birth, and department ID.
    // Inserts the student record into the 'Student' table in the database.
    // Returns void.
    private static void CreateStudent()
    {
        Console.WriteLine("\nCreating a new student...");
        Console.Write("Enter ID: ");
        int StudentID = int.Parse(Console.ReadLine());
        Console.Write("Enter First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter Date of Birth (yyyy-MM-dd): ");
        string dateOfBirth = Console.ReadLine();
        Console.Write("Enter Department ID: ");
        int departmentID = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();

            using SqlCommand cmd = new("INSERT INTO Student (StudentID, FirstName, LastName, DateOfBirth, DepartmentID) VALUES (@StudentID ,@FirstName, @LastName, @DateOfBirth, @DepartmentID)", connection);
            cmd.Parameters.AddWithValue("@StudentID", StudentID);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Student created successfully.");
            }
            else
            {
                Console.WriteLine("Failed to create the student.");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // Shows the list of students by retrieving the student data from the database.
    private static void ShowStudents()
    {
        Console.WriteLine("\nReading students...");
        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("SELECT StudentID, FirstName, LastName, DateOfBirth, DepartmentID FROM Student", connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int studentID = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                DateTime dateOfBirth = reader.GetDateTime(3);
                int departmentID = reader.GetInt32(4);

                Console.WriteLine($"Student ID: {studentID}");
                Console.WriteLine($"Name: {firstName} {lastName}");
                Console.WriteLine($"Date of Birth: {dateOfBirth:dd-MM-yyyy}"); // Formatting the date
                Console.WriteLine($"Department ID: {departmentID}");
                Console.WriteLine("----------------------------");
            }
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // Updates a student's information in the database.
    // 
    // Parameters:
    //   None
    //
    // Returns:
    //   None
    private static void UpdateStudent()
    {
        Console.WriteLine("\nUpdating a student...");
        Console.Write("Enter Student ID to update: ");
        int studentID = int.Parse(Console.ReadLine());

        Console.Write("Enter new First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter new Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter new Date of Birth (yyyy-MM-dd): ");
        string dateOfBirth = Console.ReadLine();
        Console.Write("Enter new Department ID: ");
        int departmentID = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, DepartmentID = @DepartmentID WHERE StudentID = @StudentID", connection);
            cmd.Parameters.AddWithValue("@StudentID", studentID);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Student updated successfully.");
            }
            else
            {
                Console.WriteLine("No student found with the provided ID.");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // Deletes a student from the database based on the provided Student ID.
    private static void DeleteStudent()
    {
        Console.WriteLine("\nDeleting a student...");
        Console.Write("Enter Student ID to delete: ");
        int studentID = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("DELETE FROM Student WHERE StudentID = @StudentID", connection);
            cmd.Parameters.AddWithValue("@StudentID", studentID);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Student deleted successfully.");
            }
            else
            {
                Console.WriteLine("No student found with the provided ID.");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // Adds a course to a student.
    private static void AddCourseToStudent()
    {
        Console.WriteLine("\nAdding a course to a student...");
        Console.Write("Enter Student ID: ");
        int studentID = int.Parse(Console.ReadLine());

        // Display available courses
        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("SELECT CourseID, CourseName FROM Course", connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("Available Courses:");
            while (reader.Read())
            {
                int availableCourseID = reader.GetInt32(0);
                string courseName = reader.GetString(1);
                Console.WriteLine($"{availableCourseID}: {courseName}");
            }
        }

        Console.Write("Enter Course ID to add to the student: ");
        int selectedCourseID = int.Parse(Console.ReadLine());

        // Check if the student and course exist
        bool studentExists = CheckIfStudentExists(studentID);
        bool courseExists = CheckIfCourseExists(selectedCourseID);

        if (studentExists && courseExists)
        {
            // Insert the enrollment record, omitting the EnrollmentID
            using SqlConnection connection = new(connectionString);
            connection.Open();
            using SqlCommand cmd = new("INSERT INTO Enrollment (StudentID, CourseID) VALUES (@StudentID, @CourseID)", connection);
            cmd.Parameters.AddWithValue("@StudentID", studentID);
            cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Course added to the student successfully.");
            }
            else
            {
                Console.WriteLine("Failed to add the course to the student.");
            }
        }
        else
        {
            Console.WriteLine("Student or course not found. Please check the IDs.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // Checks if a student with the given student ID exists in the database.
    // 
    // Parameters:
    //   studentID: The ID of the student to check.
    //
    // Returns:
    //   A boolean value indicating whether the student exists or not.
    private static bool CheckIfStudentExists(int studentID)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        using SqlCommand cmd = new("SELECT COUNT(*) FROM Student WHERE StudentID = @StudentID", connection);
        cmd.Parameters.AddWithValue("@StudentID", studentID);

        int count = (int)cmd.ExecuteScalar();
        return count > 0;
    }

    // Checks if a course with the given courseID exists in the database.
    private static bool CheckIfCourseExists(int courseID)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        using SqlCommand cmd = new("SELECT COUNT(*) FROM Course WHERE CourseID = @CourseID", connection);
        cmd.Parameters.AddWithValue("@CourseID", courseID);

        int count = (int)cmd.ExecuteScalar();
        return count > 0;
    }



    // Inserts marks for a student in a course and calculates the GPA.
    private static void InsertMarksAndCalculateGPA()
    {
        Console.WriteLine("\nInserting Marks and Calculating GPA...");
        Console.Write("Enter Student ID: ");
        int studentID = int.Parse(Console.ReadLine());
        Console.Write("Enter Course ID: ");
        int courseID = int.Parse(Console.ReadLine());
        Console.Write("Enter Marks: ");
        decimal marks = decimal.Parse(Console.ReadLine());

        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("UPDATE Enrollment SET Grade = @Marks WHERE StudentID = @StudentID AND CourseID = @CourseID", connection);
            cmd.Parameters.AddWithValue("@StudentID", studentID);
            cmd.Parameters.AddWithValue("@CourseID", courseID);
            cmd.Parameters.AddWithValue("@Marks", marks);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                CalculateAndSetGPA(connection, studentID, courseID); // Pass both StudentID and CourseID
                Console.WriteLine("Marks inserted and GPA calculated successfully.");
            }
            else
            {
                Console.WriteLine("No matching student or course found.");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // CalculateAndSetGPA calculates the GPA for a student in a specific course and updates the Grade field in the Enrollment table accordingly.
    //
    // Parameters:
    //   connection: The SqlConnection object representing the connection to the database.
    //   studentID: The ID of the student whose GPA is to be calculated.
    //   courseID: The ID of the course for which the GPA is to be calculated.
    private static void CalculateAndSetGPA(SqlConnection connection, int studentID, int courseID)
    {
        using SqlCommand cmd = new("UPDATE Enrollment SET Grade = " +
            "CASE " +
            "WHEN Grade >= 80 THEN 4.0 " +
            "WHEN Grade >= 70 THEN 3.5 " +
            "WHEN Grade >= 60 THEN 3.0 " +
            "WHEN Grade >= 50 THEN 2.5 " +
            "WHEN Grade >= 40 THEN 2.0 " +
            "ELSE 0.0 " +
            "END " +
            "WHERE StudentID = @StudentID AND CourseID = @CourseID", connection);
        cmd.Parameters.AddWithValue("@StudentID", studentID);
        cmd.Parameters.AddWithValue("@CourseID", courseID); // Add the CourseID parameter
        cmd.ExecuteNonQuery();
    }


    // write a function to show student information including course and GPA
    // take input of student id to search a singe student in this function
    // Searches for a student based on their ID and displays their information.
    private static void SearchStudent()
    {
        Console.WriteLine("Enter Student ID to search for: ");
        int studentID = int.Parse(Console.ReadLine());

        using SqlConnection connection = new(connectionString);
        connection.Open();
        using (SqlCommand cmd = new(
            "SELECT Student.StudentID, Student.FirstName, Student.LastName, " +
            "Enrollment.CourseID, Course.CourseName, Enrollment.Grade " +
            "FROM Student " +
            "JOIN Enrollment ON Student.StudentID = Enrollment.StudentID " +
            "JOIN Course ON Enrollment.CourseID = Course.CourseID " +
            "WHERE Student.StudentID = @StudentID", connection))
        {
            cmd.Parameters.AddWithValue("@StudentID", studentID);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int studentIdResult = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                int courseId = reader.GetInt32(3);
                string courseName = reader.GetString(4);
                decimal grade = reader.GetDecimal(5);

                Console.WriteLine("Student Information:");
                Console.WriteLine($"Student ID: {studentIdResult}");
                Console.WriteLine($"Name: {firstName} {lastName}");
                Console.WriteLine($"Course ID: {courseId}");
                Console.WriteLine($"Course Name: {courseName}");
                Console.WriteLine($"Grade: {grade}");
                Console.WriteLine("----------------------------");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // write a function to to show available courses and insert new courses
    private static void ShowAvailableCoursesAndInsertNewCourses()
    {
        Console.WriteLine("Available Courses:");
        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("SELECT CourseID, CourseName FROM Course", connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int availableCourseID = reader.GetInt32(0);
                string courseName = reader.GetString(1);
                Console.WriteLine($"{availableCourseID}: {courseName}");
            }
        }

        Console.WriteLine("\nOptions:");
        Console.WriteLine("1. Insert New Course");
        Console.WriteLine("0. Return to Main Menu");

        Console.Write("Enter your choice: ");
        char choice = Console.ReadKey().KeyChar;

        switch (choice)
        {
            case '1':
                InsertNewCourse();
                break;
            case '0':
                // Return to the main menu
                break;
            default:
                Console.WriteLine("\nInvalid choice. Press any key to continue...");
                Console.ReadLine();
                break;
        }
    }

    // InsertNewCourse is a private static void function that inserts a new course into the database.
    // It prompts the user to enter the course ID, course name, and department ID.
    // Then, it establishes a connection to the database using the provided connection string.
    // It creates a new SQL command to insert the course into the "Course" table, with the provided parameters.
    // It executes the command and checks the number of rows affected.
    // If the insertion is successful, it prints a success message.
    // Otherwise, it prints a failure message.
    // Finally, it prompts the user to press any key to continue.
    private static void InsertNewCourse()
    {
        Console.WriteLine("\nInserting a New Course...");
        Console.Write("Enter Course ID: ");
        int courseID = int.Parse(Console.ReadLine());
        Console.Write("Enter Course Name: ");
        string courseName = Console.ReadLine();
        Console.Write("Enter Department ID: ");
        int departmentID = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("INSERT INTO Course (CourseID, CourseName, DepartmentID) VALUES (@CourseID, @CourseName, @DepartmentID)", connection);
            cmd.Parameters.AddWithValue("@CourseID", courseID);
            cmd.Parameters.AddWithValue("@CourseName", courseName);
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("New course inserted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to insert the course.");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // Show the available departments and insert new departments.
    private static void ShowAvailableDepartmentsAndInsertNewDepartments()
    {
        Console.WriteLine("Available Departments:");
        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("SELECT DepartmentID, DepartmentName FROM Department", connection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int availableDepartmentID = reader.GetInt32(0);
                string departmentName = reader.GetString(1);
                Console.WriteLine($"{availableDepartmentID}: {departmentName}");
            }
        }

        Console.WriteLine("\nOptions:");
        Console.WriteLine("1. Insert New Department");
        Console.WriteLine("0. Return to Main Menu");
        Console.WriteLine("-------------------------------");
        Console.Write("Enter your choice: ");
        char choice = Console.ReadKey().KeyChar;

        switch (choice)
        {
            case '1':
                InsertNewDepartment();
                break;
            case '0':
                // Return to the main menu
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }



    // Inserts a new department into the database.
    private static void InsertNewDepartment()
    {
        Console.WriteLine("\nInserting a New Department...");
        Console.Write("Enter Department ID: ");
        int departmentID = int.Parse(Console.ReadLine());
        Console.Write("Enter Department Name: ");
        string departmentName = Console.ReadLine();

        using (SqlConnection connection = new(connectionString))
        {
            connection.Open();
            using SqlCommand cmd = new("INSERT INTO Department (DepartmentID, DepartmentName) VALUES (@DepartmentID, @DepartmentName)", connection);
            cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
            cmd.Parameters.AddWithValue("@DepartmentName", departmentName);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("New department inserted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to insert the department.");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // // //
}// End of Class "Program"
 // // //