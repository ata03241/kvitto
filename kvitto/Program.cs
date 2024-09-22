using System;

namespace ProjectPartA_A1
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
    const decimal _vat = 0.25M;

    static Article[] articles = new Article[_maxNrArticles];
    static int nrArticles;

    static void Main(string[] args)
    {
      ReadArticles();
      PrintReciept();
    }

    private static void ReadArticles()
    {
      Console.Write("Let`s print a receipt (y/n)");
      string ans = Console.ReadLine().ToLower().Trim();

      if (ans == "y")
      {
        Console.WriteLine("How many articles do you want (between 1 and 10)");
        while (!int.TryParse(Console.ReadLine(), out nrArticles) || nrArticles < 1 || nrArticles > _maxNrArticles)
        {
          Console.WriteLine("wrong input, pls try again");
        }


        for (int i = 0; i < nrArticles; i++)
        {
          while (true)
          {
            Console.WriteLine($"Please enter name and price for article #{i} in the format name; price (Example Beer; 2,25)");
            string userinput = Console.ReadLine();
            

            if (string.IsNullOrWhiteSpace(userinput))
            {
              System.Console.WriteLine("Format error");
              continue;
            }
            string[] input = userinput.Split(';');

            if(input.Length != 2){
              System.Console.WriteLine("Format error");
              continue;
            }
            
            string name = input[0].Trim();
            string pricetag = input[1].Trim();

            if (string.IsNullOrEmpty(name))
            {
              System.Console.WriteLine("Name error");
              continue;
            }

            if(string.IsNullOrWhiteSpace(pricetag)){
              System.Console.WriteLine("Price Error! Please type the amount");
              continue;
            }

            if(userinput.Length > _maxArticleNameLength) {
              System.Console.WriteLine("Name error! Max Article Name Length should be less than 20");
              continue;
            }


            if (!decimal.TryParse(pricetag, out decimal price))
            {
              System.Console.WriteLine("Price error");
              continue;
            }

            articles[i].Name = name;
            articles[i].Price = price;
            break;
          }

        }
      }
      else 
      {
        System.Console.WriteLine("Program ends");
        
      }


    }
    private static void PrintReciept()
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


      System.Console.WriteLine($"\nTotal Purchased: {totalprice,13} KR");
      System.Console.WriteLine($"Includes VAT (25%):       {Math.Round(vat, 2)} KR");

    }
  }
}