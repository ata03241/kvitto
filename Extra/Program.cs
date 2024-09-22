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
            int menuSel = 9;
            do
            {
                menuSel = MenuSelection();
                MenuExecution(menuSel);

            } while (menuSel != 9);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine("\nThank you for your time");
            Console.ForegroundColor = ConsoleColor.White;
        }



        private static int MenuSelection()
        {
            int menuSel = 9;

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
                        System.Console.WriteLine("3 - Print receipt sorted by name");
                        System.Console.WriteLine("4 - Print receipt sorted by price");
                        System.Console.WriteLine("5- Search an Article");
                        System.Console.WriteLine("6- Clear all the artciles");
                        System.Console.WriteLine("7- Export to file");
                        System.Console.WriteLine("8_ Update the name and price of the article");
                        System.Console.WriteLine("9 - Quit");
                        menuSel = int.Parse(Console.ReadLine());

                        if (menuSel >= 1 && menuSel <= 9)
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
                    SortArticlesPrice(true);
                    break;
                case 5:
                    SearchArticle();
                    break;
                case 6:
                    ClearArticle();
                    break;
                case 7:
                    ExportFile();
                    break;
                case 8:
                    UpdateRecipt();
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
                    if (input.Length != 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Format error");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }

                    articles[nrArticles] = new Article { Name = name, Price = price };
                    nrArticles++;
                    break;
                }

            }
        }

        private static void UpdateRecipt()
        {
            if (nrArticles == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("No articles to search");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            System.Console.WriteLine("Type the name of the article that your want to update: ");
            string searchName = Console.ReadLine().Trim().ToLower();

            int articleindex = -1;
            for (int i = 0; i < nrArticles; i++)
            {
                if (articles[i].Name == searchName)
                {
                    articleindex = i;
                    break;
                }
            }

            if (articleindex == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Article not found.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            System.Console.WriteLine("Enter new name (or press Enter to keep the same name: )");
            string Newname = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(Newname))
            {
                articles[articleindex].Name = Newname;
            }

            System.Console.WriteLine("enter new price ( (or press Enter to keep the same name:)");
            string newprice = Console.ReadLine().Trim();
            if (decimal.TryParse(newprice, out decimal price))
            {
                articles[articleindex].Price = price;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Article updated successfully.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SearchArticle()
        {
            if (nrArticles == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("No articles to search");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            System.Console.WriteLine("Type the name of the article that your are looking for: ");
            string searchName = Console.ReadLine().Trim().ToLower();

            bool ArtcileName = false;
            for (int i = 0; i < nrArticles; i++)
            {
                if (articles[i].Name == searchName)
                {
                    System.Console.WriteLine($"\nArtcile found: {articles[i].Name,-23} {articles[i].Price}");
                    ArtcileName = true;
                }
            }

            if (!ArtcileName)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("No articles found by that name");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void ExportFile()
        {
            if (nrArticles == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("No articles to export");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            string folder = @"C:\Users\pakhr\OneDrive\Desktop\kvitto\Extra";
            System.IO.Directory.CreateDirectory(folder);
            string fileName = System.IO.Path.Combine(folder, "receipt.txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
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
            System.Console.WriteLine("The articles has been export");
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




    }
}
