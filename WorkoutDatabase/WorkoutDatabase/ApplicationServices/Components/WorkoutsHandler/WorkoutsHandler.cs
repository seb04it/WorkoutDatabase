using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDatabase.ApplicationServices.Repositories;
using WorkoutDatabase.DataAccess.Data.Entities;

namespace WorkoutDatabase.ApplicationServices.Components.CategoryHandler
{
    public class WorkoutsHandler : IWorkoutsHandler
    {
        private readonly IRepository<WorkoutCategoryEntity> _workoutCategoryRepository;

        public WorkoutsHandler(IRepository<WorkoutCategoryEntity> workoutCategoryRepository)
        {
            _workoutCategoryRepository = workoutCategoryRepository;
        }

        public string CategoryChoice()
        {
            Console.Clear();
            Console.Write("\nWybierz numer kategorii z poniższych:\n" +
                "1. Rozgrzewka\n" +
                "2. Pośladki\n" +
                "3. Martwy ciąg\n" +
                "4. Triceps\n" +
                "5. Biceps\n" +
                "6. Nogi\n" +
                "7. Ramiona\n" +
                "8. Brzuch\n" +
                "9. Rozciąganie\n" +
                "========================================" +
                "\n10. Podaj własną nazwę kategorii\n\n" +
                "Wybór: ");

            string workoutCategory = null;

            switch (Console.ReadLine())
            {
                case "1":
                    workoutCategory = "Rozgrzewka";
                    break;
                case "2":
                    workoutCategory = "Pośladki";
                    break;
                case "3":
                    workoutCategory = "Martwy ciąg";
                    break;
                case "4":
                    workoutCategory = "Triceps";
                    break;
                case "5":
                    workoutCategory = "Biceps";
                    break;
                case "6":
                    workoutCategory = "Nogi";
                    break;
                case "7":
                    workoutCategory = "Ramiona";
                    break;
                case "8":
                    workoutCategory = "Brzuch";
                    break;
                case "9":
                    workoutCategory = "Rozciąganie";
                    break;
                case "10":
                    workoutCategory = FurtherCategoryChoice();
                    break;
                default:
                    throw new Exception("Invalid choice");
            }
            Console.Clear();
            Console.WriteLine($"\nPodaj kategorię ćwiczenia: {workoutCategory}");
            return workoutCategory;
        }

        public DateTime DateTimeUpdate()
        {
            Console.Write("\nWybierz sposób dodania daty ostatniego użycia:\n" +
                        "\n1. Data dzisiejsza\n" +
                        "2. Samodzielne wpisanie\n" +
                        "\nWybór: ");
            var choice = Console.ReadLine().ToLower();
            switch (choice)
            {
                case "1":
                    try
                    {
                        var today = DateTime.Now.Date;
                        return today;
                    }
                    catch (Exception exception)
                    {
                        Console.Clear();
                        throw new Exception("An error occurred while retrieving the current date", exception);
                    }
                case "2":
                    Console.Write("Podaj datę (dd.MM.yyyy lub dd:MM:yyyy): ");
                    string inputDate = Console.ReadLine().Replace(':', '.');
                    if (DateTime.TryParseExact(inputDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
                    {
                        return date;
                    }
                    else
                    {
                        throw new Exception("Form of date is invalid. Try (day.month.year)");
                    }
                default:
                    throw new Exception("Invalid choice (Choose 1 or 2)");
            }
        }

        public string FurtherCategoryChoice()
        {
            Console.Write("\nJak chcesz to zrobić?\n" +
                "\n1. Z listy wcześniej dodanych kategorii\n" +
                "2. Wpisując nową\n\n" +
                "Wybór: ");

            string workoutCategory = null;

            switch (Console.ReadLine())
            {

                case "1":
                    var categories = _workoutCategoryRepository.GetAll().ToList();
                    Console.WriteLine("\nLista własnych kategorii:");
                    foreach (var category in categories)
                    {
                        Console.WriteLine(category.CategoryName);
                    }
                    Console.Write("\nWybierz ID kategorii ćwiczenia z listy: ");
                    if (int.TryParse(Console.ReadLine(), out int workoutId))
                    {
                        var workoutCategoryChoice = categories.FirstOrDefault(category => category.Id == workoutId);
                        if (workoutCategoryChoice != null)
                        {
                            workoutCategory = workoutCategoryChoice.CategoryName;
                        }
                        else
                        {
                            throw new Exception("Chosen ID doesn't exist");
                        }
                    }
                    else
                    {
                        throw new Exception("ID fortmat was invalid");
                    }

                    break;
                case "2":
                    Console.Write("\nWpisz kategorię: ");

                    var existingCategory = _workoutCategoryRepository.GetAll().FirstOrDefault(c => c.CategoryName.Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase));
                    if (existingCategory != null)
                    {
                        workoutCategory = existingCategory.CategoryName;
                    }
                    else
                    {
                        _workoutCategoryRepository.AddItem(new WorkoutCategoryEntity
                        {
                            CategoryName = workoutCategory
                        });
                    }
                    break;
                default:
                    throw new Exception("Invalid choice");
            }
            return workoutCategory;
        }
    }
}
