using System;
using System.Collections.Generic;
namespace Homework_8;
public enum ProjectStatus
{
    Проект,
    Исполнение,
    Закрыт
}

public enum TaskStatus
{
    Назначена,
    ВРаботе,
    НаПроверке,
    Выполнена
}

public class Task
{
    public string Description { get; set; }
    public string Assignee { get; set; }
    public TaskStatus Status { get; set; }

    public Task(string description, string assignee)
    {
        Description = description;
        Assignee = assignee;
        Status = TaskStatus.Назначена;
    }

    public void ChangeStatus(TaskStatus status)
    {
        Status = status;
    }

    public void DisplayTaskInfo()
    {
        Console.WriteLine($"Задача: {Description}, Статус: {Status}, Назначена: {Assignee}");
    }
}

public class Project
{
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public string Initiator { get; set; }
    public string TeamLead { get; set; }
    public List<Task> Tasks { get; set; }
    public ProjectStatus Status { get; set; }

    public Project(string description, DateTime deadline, string initiator, string teamLead)
    {
        Description = description;
        Deadline = deadline;
        Initiator = initiator;
        TeamLead = teamLead;
        Tasks = new List<Task>();
        Status = ProjectStatus.Проект;
    }

    public void AddTask(Task task)
    {
        Tasks.Add(task);
    }

    public void ChangeProjectStatus(ProjectStatus status)
    {
        Status = status;
    }

    public void DisplayProjectInfo()
    {
        Console.WriteLine($"Проект: {Description}, Статус: {Status}, Заказчик: {Initiator}, Тимлид: {TeamLead}, Срок: {Deadline.ToShortDateString()}");
        Console.WriteLine("Задачи по проекту:");
        foreach (var task in Tasks)
        {
            task.DisplayTaskInfo();
        }
    }
}

public class TaskManager
{
    private List<Project> _projects;

    public TaskManager()
    {
        _projects = new List<Project>();
    }

    public void AddProject(Project project)
    {
        _projects.Add(project);
    }

    public void DisplayProjects()
    {
        Console.WriteLine("Список проектов:");
        foreach (var project in _projects)
        {
            project.DisplayProjectInfo();
        }
    }

    public void ChangeProjectStatus(int projectIndex, ProjectStatus status)
    {
        if (projectIndex >= 0 && projectIndex < _projects.Count)
        {
            _projects[projectIndex].ChangeProjectStatus(status);
            Console.WriteLine($"Статус проекта изменен на {status}");
        }
        else
        {
            Console.WriteLine("Проект не найден.");
        }
    }

    public void ChangeTaskStatus(int projectIndex, int taskIndex, TaskStatus status)
    {
        if (projectIndex >= 0 && projectIndex < _projects.Count)
        {
            var project = _projects[projectIndex];
            if (taskIndex >= 0 && taskIndex < project.Tasks.Count)
            {
                project.Tasks[taskIndex].ChangeStatus(status);
                Console.WriteLine($"Статус задачи изменен на {status}");
            }
            else
            {
                Console.WriteLine("Задача не найдена.");
            }
        }
        else
        {
            Console.WriteLine("Проект не найден.");
        }
    }
}

class Program
{
    static void Main()
    {
        TaskManager taskManager = new TaskManager();

        Project project1 = new Project("Разработка нового сайта", DateTime.Parse("2024-12-31"), "Компания X", "Иван Иванов");
        Task task1 = new Task("Разработать дизайн", "Иван Иванов");
        Task task2 = new Task("Сделать верстку", "Петр Петров");

        project1.AddTask(task1);
        project1.AddTask(task2);

        taskManager.AddProject(project1);

        taskManager.DisplayProjects();

        Console.WriteLine("\nИзменение статуса задачи...");
        taskManager.ChangeTaskStatus(0, 1, TaskStatus.ВРаботе);

        Console.WriteLine("\nИзменение статуса проекта...");
        taskManager.ChangeProjectStatus(0, ProjectStatus.Исполнение);

        taskManager.DisplayProjects();

        Console.WriteLine("\nИзменение статуса задачи...");
        taskManager.ChangeTaskStatus(0, 0, TaskStatus.Выполнена);

        Console.WriteLine("\nИзменение статуса проекта...");
        taskManager.ChangeProjectStatus(0, ProjectStatus.Закрыт);

        taskManager.DisplayProjects();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
