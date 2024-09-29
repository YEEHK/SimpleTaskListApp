using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskListApp
{
    using System;
    class Task
    {
        static string filePath = "tasksfile.txt";
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        
        public Task(string description, DateTime dueDate)
        {
            Description = description;
            IsCompleted = false;            
            DueDate = dueDate;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }
    
        public override string ToString()
        {
            //return result either mark or not mark
            string status = IsCompleted ? "[x]" : "[ ]";            
            return $"{status} {Description} (Due on: {DueDate.ToShortDateString()})";
        }
    }

    class Program
    {
        static List<Task> tasks = new List<Task>();
        static List<string> tasksline = new List<string>();
        static bool IsCheckTaskMark = false;
        static bool IsDeleteTask = false;

        static string filePath = "tasksfile.txt";
        // Load tasks from file on startup
        static void LoadTasksFromFile()
        {
            if (File.Exists(filePath))
            {
                tasksline = new List<string>(File.ReadAllLines(filePath));
               
                for (int i=0; i<tasksline.Count; i++)
                {
                    string[] wordsArray = tasksline[i].Split(';');
               
                    tasks.Add(new Task(wordsArray[1], DateTime.Parse(wordsArray[2])));
                    if (wordsArray[0] == "[X]")
                        tasks[i].MarkAsCompleted();
                }                
                Console.WriteLine("Tasks loaded from file.");
            }
            else
            {
                Console.WriteLine("No previous tasks found.");
            }
        }

        // Save tasks to file whenever tasks are added or removed
        static void SaveTasksToFile(String tasksline)
        {
            File.AppendAllText(filePath, tasksline);
            //Console.WriteLine("Tasks saved to file.");
        }
        static void Main(string[] args)
        {
            bool exit = false;
            LoadTasksFromFile();
            while (!exit)
            {
                Console.Clear();
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        ViewTasks();
                        break;
                    case "3":
                        MarkTaskAsDone();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "5":
                        ExitTask();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("Task List Application");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View Tasks");
            Console.WriteLine("3. Mark Task as Done");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
        }

        static void AddTask()
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();
            DateTime dueDate;
                      
            while (true)
            {
                Console.Write("Enter task due date (dd/MM/yyyy): ");
                if (DateTime.TryParse(Console.ReadLine(), out dueDate))     //check for valid date input
                {                    
                    break;
                }               
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }
            tasks.Add(new Task(description, dueDate));           
                    
            Console.WriteLine("Task added. Press any key to continue...");            
            Console.ReadKey();            
        }

        static void ViewTasks()
        {
            Console.WriteLine("Your Tasks:");            
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                return;
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)                
                {
                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                }
            }
            
            if(!IsCheckTaskMark && !IsDeleteTask)       // avoid the message display when perform Task mark and delete task
                Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        static void MarkTaskAsDone()
        {
            IsCheckTaskMark = true;
            ViewTasks();
            if (tasks.Count == 0)       //No task found, return to main menu
                return;

            Console.Write("Enter the task number to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks[taskNumber - 1].MarkAsCompleted();
                Console.WriteLine("Task marked as completed.");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void DeleteTask()
        {
            IsCheckTaskMark = false;
            IsDeleteTask = true;
            ViewTasks();

            if (tasks.Count == 0)
                return;

            Console.Write("Enter the task number to delete: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks.RemoveAt(taskNumber - 1);
                Console.WriteLine("Task deleted.");                
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }

        static void ExitTask() 
        {
            File.Delete(filePath);  //delete the old data file, so that only lastest data store into file when exit.

            // Store to file when exit the application
            for (int i = 0; i < tasks.Count; i++)
            {                
                if (tasks[i].IsCompleted)
                    SaveTasksToFile($"[X];{tasks[i].Description};{tasks[i].DueDate}\n"); 
                else
                    SaveTasksToFile($"[ ];{tasks[i].Description};{tasks[i].DueDate}\n");              
            }    
        }
    }
}
