using System;
using System.Runtime.Intrinsics.Arm;

namespace ProjectPartA_A2
{
    class Program
    {
        struct Article
        {
            public string Name;
            public decimal Price;
        }

        const int _maxNrArticles = 10;
        const int _maxArticleNameLength = 20;
        const decimal _vat = 0.20M;

        static Article[] articles = new Article[_maxNrArticles];

        static int nrArticles = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Project Part A\n");
            while (true)
            {
                System.Console.WriteLine("Enter your name: ");
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    System.Console.WriteLine($"Hello {input}! Let`s choose the articles");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Please enter your name!");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
            }
            int menuSel = 6;
            do
            {
                menuSel = MenuSelection();
                MenuExecution(menuSel);

            } while (menuSel != 6);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine("\nThank you for your time");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static int MenuSelection()
        {
            int menuSel = 6;

            while (true)
            {
                try
                {
                    for (int i = 0; i < _maxNrArticles; i++)
                    {

                        System.Console.WriteLine($"\n{nrArticles} articles entered.");
                        System.Console.WriteLine("Menu: ");
                        System.Console.WriteLine("1 - Enter an article");
                        System.Console.WriteLine("2 - Remove an article");
                        System.Console.WriteLine("3 - Print receipt by Name");
                        System.Console.WriteLine("4 - Clear receipt");
                        System.Console.WriteLine("5 - Print receipt sorted by price");
                        System.Console.WriteLine("6 - Quit");
                        menuSel = int.Parse(Console.ReadLine());

                        if (menuSel >= 1 && menuSel <= 6)
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("Invalid selection");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Wrong selection, pls try again");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }


                return menuSel;
            }
        }
        private static void MenuExecution(int menuSel)
        {

            switch (menuSel)
            {
                case 1:
                    ReadAnArticle();
                    break;
                case 2:
                    RemoveAnArticle();
                    break;
                case 3:
                    SortArticles(true);
                    break;
                case 4:
                    ClearArticle();
                    break;
                case 5:
                    SortArticlesPrice(true);
                    break;
            }


        }

        private static void ReadAnArticle()
        {
            int newarticle;
            Console.WriteLine("How many articles do you want (between 1 and 10)");
            while (!int.TryParse(Console.ReadLine(), out newarticle) || newarticle < 1 || newarticle > _maxNrArticles)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("wrong input, pls try again!! choose number between 1 and 10");
                Console.ForegroundColor = ConsoleColor.White;
            }


            for (int i = 0; i < newarticle; i++)
            {
                while (true)
                {
                    Console.WriteLine($"Please enter name and price for article #{i} in the format name; price (Example Beer; 2,25)");
                    string userinput = Console.ReadLine();


                    if (string.IsNullOrWhiteSpace(userinput))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Format error");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    string[] input = userinput.Split(';');

                    if (input.Length != 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Format error");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }


                    string name = input[0].Trim();
                    string pricetag = input[1].Trim();

                    if (string.IsNullOrEmpty(name))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Name error");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }

                    if (!decimal.TryParse(pricetag, out decimal price))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Price error");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    if (userinput.Length > _maxArticleNameLength)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Name error! Max Article Name Length should be less than 20");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }

                    articles[nrArticles] = new Article { Name = name, Price = price };
                    nrArticles++;
                    break;
                }

            }

        }
        private static void RemoveAnArticle()
        {
            Console.WriteLine("Please enter the name of the article to remove (example: Beer): ");
            string removeInput = Console.ReadLine().Trim().ToLower();

            int indexToRemove = -1;
            for (int i = 0; i < nrArticles; i++)
            {

                if (articles[i].Name.ToLower() == removeInput)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Article '{removeInput}' not found.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            for (int i = indexToRemove; i < nrArticles - 1; i++)
            {
                articles[i] = articles[i + 1];
            }


            nrArticles--;


            articles[nrArticles] = default(Article);

            Console.WriteLine($"Article '{removeInput}' has been removed.");
        }


        private static void PrintReciept(string title)
        {

            if (nrArticles == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("No articles to display");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            DateTime now = DateTime.Now;
            System.Console.WriteLine("Reciept Purchased Articles");
            System.Console.WriteLine("Purchased date: " + now.ToString("yyyy-MM-dd HH:mm:ss"));

            System.Console.WriteLine($"Number of items purchesed: {nrArticles}");

            decimal totalprice = 0;
            System.Console.WriteLine("\n#  Name                     Price");
            for (int i = 0; i < nrArticles; i++)
            {
                System.Console.WriteLine($"{i}  {articles[i].Name,-23}{articles[i].Price} KR");
                totalprice += articles[i].Price;
            }

            decimal vat = totalprice * _vat;

            System.Console.WriteLine("---------------------------------");
            System.Console.WriteLine($"Total Purchased: {totalprice,13} KR");
            System.Console.WriteLine($"Includes VAT (25%):       {Math.Round(vat, 2)} KR");
        }

        private static void ClearArticle()
        {
            System.Console.WriteLine("Do you want to clear all the Articles? (y/n): ");
            string clearArticel = Console.ReadLine().Trim().ToLower();

            if (clearArticel == "y")
            {
                articles = new Article[_maxNrArticles];
                nrArticles = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("All the articles has been removed");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Opeartion failed");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        //Challange, not included in VG critera
        private static void SortArticles(bool sortByName = false)
        {
            if (sortByName)
            {                
                for (int i = 0; i < nrArticles - 1; i++)
                {
                    for (int j = 0; j < nrArticles - (1 + i); j++)
                    {
                       
                        if (string.Compare(articles[j].Name, articles[j + 1].Name) > 0)
                        {
                            
                            var tempArticle = articles[j + 1];
                            articles[j + 1] = articles[j];
                            articles[j] = tempArticle;
                        }
                    }
                }
                PrintReciept(null);
            }
        }


        private static void SortArticlesPrice(bool sortByPrice = false)
        {
            //Your code to Sort. Either BubbleSort or SelectionSort
            if (sortByPrice)
            {
                for (int i = 0; i < nrArticles - 1; i++)
                {
                    for (int j = 0; j < nrArticles - (1 + i); j++)
                    {
                        if (articles[j].Price > articles[j + 1].Price)
                        {
                            var pricetemp = articles[j + 1];
                            articles[j + 1] = articles[j];
                            articles[j] = pricetemp;
                        }
                    }
                }
                PrintReciept(null);
            }
        }

        //På extra har jag gjort flera roliga saker som du kan kika på
    }
}
