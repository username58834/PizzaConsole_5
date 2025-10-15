using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace PizzaConsole_5
{
    public class PizzaClass
    {
        public List<Ingredients> ingredients = new List<Ingredients>();
        private string name;
        private float price;
        private double weight;
        private DateTime bakingDate = DateTime.Now;
        private double bakingTime = (new Random().Next() % 10000) / 1000;//time in sec

        private DateTime lastTimeChecked = DateTime.Now;
        private States state { get; set; } = States.Baking;

        private static int counter = 1;
        public static int Counter { get; private set; }

        static double freshTime = 60;

        public static double FreshTime {
            get
            {
                return freshTime;
            }
            set
            {
                if (value > 0) freshTime = value;
                else throw new Exception("Enter a number greater than zero");
            }
        }
        public static string CodeCounter()
        {
            string text = "";
            int n = Counter;
            if (n == 0) text = "A";
            while (n > 0)
            {
                text = (char)('A' + (n % 26)) + text;
                n /= 26;
            }
            return text;
        }

        public static PizzaClass Parse(string text)
        {
            string[] arr = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length <= 3)
            {
                switch (arr.Length)
                {
                    case 0:
                        return new PizzaClass();
                    case 1:
                        return new PizzaClass(arr[0]);
                    case 2:
                        return new PizzaClass(arr[0], float.Parse(arr[1]));
                    case 3:
                        return new PizzaClass(arr[0], float.Parse(arr[1]), float.Parse(arr[2]));
                    default:
                        return new PizzaClass();
                }
            }
            else throw new Exception("The pizza cannot be created. Too much arguments.");  
        }

        public static bool TryParse(string text, out PizzaClass? pizza, out string message)
        {
            try
            {
                pizza = Parse(text);
                message = "OK";
            }
            catch (Exception ex)
            {
                pizza = null;
                message = ex.Message;
            }
            return (pizza != null);
        }

        public static bool TryParse(string text, out PizzaClass? pizza)
        {
            return TryParse(text, out pizza, out _);
        }
        public override string ToString()
        {
            return $"{Name} {Price.ToString("F2")} {Weight.ToString("F3")}";
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (!Regex.IsMatch(value, "^[A-Za-z ]*$") || value.Length < 3 || value.Length > 12)
                {
                    throw new ArgumentNullException(null, "Write from 3 to 12 characters. Only Latin letters are allowed");
                }
                name = value;
            }
        }

        public float Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value <= 0 || value >= 10000)
                {
                    throw new ArgumentNullException(null, "Only numbers are allowed. The price must be greater than 0 and less than 10000");
                }
                price = value;
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (value <= 0 || value >= 10)
                {
                    throw new ArgumentNullException(null, "Only numbers are allowed. The weight must be greater than 0 and less than 10");
                }
                else
                {
                    weight = value;
                }
            }
        }

        public States State
        {
            get
            {
                return state;
            }
            private set
            {
                state = value;
            }
        }

        public string Info
        {
            get { return $"Name: {Name}, price: {Price.ToString("F2")}$, weight: {Weight.ToString("F3")}kg"; }
        }
        public void ChangeState()
        {
            lastTimeChecked = DateTime.Now;
            double time = (lastTimeChecked - bakingDate).TotalSeconds;

            if (State == States.Baking && time >= bakingTime)
            {
                State = States.Ready;
            }
            if (State == States.Ready && time >= bakingTime + freshTime)
            {
                State = States.Spoiled;
            }
        }

        public bool ThrowAwayIfSpoiled()
        {
            if (State == States.Spoiled)
            {
                return true;
            }
            return false;
        }

        public PizzaClass() : this("Pizza" + CodeCounter(), 10, 0.5)
        {

        }
        public PizzaClass(string name) : this(name, 10, 0.5)
        {

        }

        public PizzaClass(string name, float price) : this(name, price, 0.5)
        {

        }
        public PizzaClass(string name, float price, double weight)
        {            
            Name = name;
            Price = price;
            Weight = weight;
            ingredients.Add(Ingredients.Dough);
            Counter++;
        }

        public void ChangeIngredients(Ingredients ingredient)
        {
            if (ingredients.Contains(ingredient))
            {
                ingredients.Remove(ingredient);
            }
            else
            {
                ingredients.Add(ingredient);
            }
        }

        public bool Sell(ref float money)
        {
            if (money >= Price)
            {
                money -= Price;
                return true;
            }
            return false;
        }

        public bool Sell(ref float money, ref string text)
        {
            double time = (lastTimeChecked - bakingDate).TotalSeconds;
            if (money >= Price)
            {
                money -= Price;
                
                if(State == States.Baking)
                {
                    text = $"The pizza can be sold, but you will have to wait for {(int)(bakingTime - time)} seconds";
                }
                else text = "The pizza can be sold";
                return true;
            }
            text = "Unfortunetly, the pizza cannot be sold";
            return false;
        }
        public string ShowIngredientsOnly()
        {
            string text = $" ---------------------------\n" +
                          $"| Ingredients:              |\n" +
                          $" ---------------------------\n";
            for (int i = 1; i < Enum.GetValues(typeof(Ingredients)).Length; i++)
            {
                text += $"| {i}";
                if (i < 10) text += " ";
                text += $" | {((Ingredients)(i)).ToString()} ";
                for (int j = 0; j < 12 - ((Ingredients)(i)).ToString().Length; j++) text += " ";
                text += $"| {ingredients.Contains((Ingredients)(i))} ";
                if (ingredients.Contains((Ingredients)(i))) text += " ";
                text += "|\n";

                text += $" ---------------------------\n";
            }

            return text;
        }

        private string AddSpaces(string text, int max_length)
        {
            while (text.Length < max_length)
            {
                text += " ";
            }
            return text;
        }
        public string Show()
        {
            ChangeState();
            double time = (lastTimeChecked - bakingDate).TotalSeconds;

            string text =
                $" -----------------------------------\n" +
                $"| Name:       | {AddSpaces(Name, 19)} |\n" +
                $"| Price:      | {AddSpaces(Price.ToString("F2") + "$", 19)} |\n" +
                $"| Weight:     | {AddSpaces(Weight.ToString("F3") + "kg", 19)} |\n" +
                $"| BakingDate: | {AddSpaces(bakingDate.ToString(), 19)} |\n" +
                $"| BakingTime: | {AddSpaces(bakingTime.ToString() + " time left: " + ((State == States.Baking) ? ((int)(bakingTime-time)).ToString() : "Done"), 19)} |\n" +
                $"| FreshTime:  | {AddSpaces(freshTime.ToString() + " time left: " + (State != States.Spoiled ? ((int)(bakingTime + freshTime - time)).ToString() : "-"), 19)} |\n" +
                $"| Status:     | {AddSpaces(State.ToString(), 19)} |\n" +
                $" -----------------------------------\n" +
                $"| Ingredients:                      |\n";

            for (int i = 1; i < Enum.GetValues(typeof(Ingredients)).Length; i++)
            {
                if (ingredients.Contains((Ingredients)(i)))
                {
                    text += $"| \t" + AddSpaces(((Ingredients)(i)).ToString(), 28 - "\t".Length) + " |\n";
                }
            }
            text += $" -----------------------------------\n";

            return text;
        }
    }
}