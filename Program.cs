using System;
using System.Collections.Generic;

namespace VarmDrinkStation
{
    //Ett gränssnitt som representerar en varm dryck
    public interface IWarmDrink
    {
        void Consume();
    }

    //Implementerar gränssnittet IWarmDrink
    internal class Water : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm water is served.");
        }
    }

    //Implementerar gränssnittet IWarmDrink
    internal class Coffee : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Coffee is served.");
        }
    }

    //Implementerar gränssnittet IWarmDrink
    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Cappuccino is served.");
        }
    }

    //Implementerar gränssnittet IWarmDrink
    internal class HotChocolate : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot chocolate is served.");
        }
    }

    //Gränssnitt för fabriker som förbereder varma drycker
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total);
    }

    //Är en fabrik för att förbereda varmt vatten och implementerar IWarmDrinkFactory
    internal class HotWaterFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot water in your cup");
            return new Water();
        }
    }

    //Är en fabrik för att förbereda kaffe och implementerar IWarmDrinkFactory
    internal class CoffeeFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Brew {total} ml of coffee");
            return new Coffee();
        }
    }

    //Som ovan
    internal class CappuccinoFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Brew {total} ml of espresso and milk");
            return new Cappuccino();
        }
    }

    //Som ovan
    internal class HotChocolateFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Mix {total} grams of hot chocolate powder with hot milk");
            return new HotChocolate();
        }
    }

    //Klassen WarmDrinkMachine hanterar maskinen för varma drycker
    public class WarmDrinkMachine
    {
        // Enums som representerar tillgängliga drycker
        public enum AvailableDrink
        {
            Water,
            Coffee,
            Cappuccino,
            HotChocolate,
        }

        private Dictionary<AvailableDrink, IWarmDrinkFactory> factories =
            new Dictionary<AvailableDrink, IWarmDrinkFactory>(); //Dictionary för att lagra fabrikerna för dryckerna

        private List<Tuple<string, IWarmDrinkFactory>> namedFactories =
            new List<Tuple<string, IWarmDrinkFactory>>(); //Lista för att lagra namn och fabriker för dryckerna


        public WarmDrinkMachine()  //Konstruktor som instansierar varmdrycksmaskinen och registrerar fabrikerna för varje tillgänglig dryck
        {
            RegisterFactory(AvailableDrink.Water, new HotWaterFactory());
            RegisterFactory(AvailableDrink.Coffee, new CoffeeFactory());
            RegisterFactory(AvailableDrink.Cappuccino, new CappuccinoFactory());
            RegisterFactory(AvailableDrink.HotChocolate, new HotChocolateFactory());
        }

        public void RegisterFactory(AvailableDrink drink, IWarmDrinkFactory factory)  //Metod för att registrera en fabrik för en specifik dryck
        {
            factories.Add(drink, factory);
            namedFactories.Add(Tuple.Create(drink.ToString(), factory));
        }

        public IWarmDrink MakeDrink()
        {
            Console.WriteLine("This is what we serve today:");
            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index}: {tuple.Item1}");
            }

            Console.WriteLine("Select a number to continue:");
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i) // c# 7
                    && i >= 0
                    && i < namedFactories.Count)
                {
                    Console.Write("How much: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        return namedFactories[i].Item2.Prepare(total);
                    }
                }
                Console.WriteLine("Something went wrong with your input, try again.");
            }
        }
    }

    class Program
    {
        //Huvudmetod som skapar en varmdrycksmaskin, låter användaren välja en dryck och konsumera den
        static void Main(string[] args)
        {
            var machine = new WarmDrinkMachine(); 
            IWarmDrink drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}