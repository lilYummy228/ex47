using System;
using System.Collections.Generic;

namespace ex47
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Shop shop = new Shop();

            Console.WriteLine("Супермаркет");
            shop.CreateClientQueue();
            shop.ServeClients();
        }
    }

    class Shop
    {
        private Queue<Client> _clients = new Queue<Client>();
        private List<Product> _products;
        private Random _random = new Random();

        public Shop()
        {
            _products = new List<Product>
            {
                new Product("Банан", 30),
                new Product("Авакадо", 40),
                new Product("Яблоко", 15),
                new Product("Груша", 20),
                new Product("Помело", 60),
                new Product("Апельсин", 30),
            };
        }

        public void CreateClientQueue()
        {
            int clientCount = 10;

            for (int i = 0; i < clientCount; i++)
            {
                _clients.Enqueue(CreateClient());
            }
        }

        public void ServeClients()
        {
            if (_clients.Count > 0)
            {
                while (_clients.Count > 0)
                {
                    _clients.Dequeue().BuyProducts();
                }
            }
        }

        private Client CreateClient()
        {
            List<Product> cart = new List<Product>();

            int minProductCount = 2;
            int maxProductCount = 8;
            int minMoney = 50;
            int maxMoney = 101;
            int productCount = _random.Next(minProductCount, maxProductCount);
            int money = _random.Next(minMoney, maxMoney);

            for (int i = 0; i < productCount; i++)
            {
                cart.Add(_products[_random.Next(0, _products.Count - 1)]);
            }

            return new Client(money, cart);
        }
    }

    class Client
    {
        private int _money;
        private List<Product> _cart;

        public Client(int money, List<Product> cart)
        {
            _money = money;
            _cart = cart;
        }

        public void BuyProducts()
        {
            Console.Clear();
            ShowCart();

            if (_money >= GetPurchaseAmount())
            {
                WriteSuccessMessage();
            }
            else
            {
                RemoveUnpayedProducts();
                WriteSuccessMessage();
            }
        }

        private void ShowCart()
        {
            Console.WriteLine("Список продуктов: \n");

            foreach (Product product in _cart)
            {
                product.ShowInfo();
            }

            DecorateMessage(ConsoleColor.DarkYellow, $"\nКлиент набрал на сумму: {GetPurchaseAmount()} рублей, у клиента есть {_money} рублей\n");
        }

        private int GetPurchaseAmount()
        {
            int purchaseAmount = 0;

            foreach (Product product in _cart)
            {
                purchaseAmount += product.Cost;
            }

            return purchaseAmount;
        }

        private void RemoveUnpayedProducts()
        {
            while (GetPurchaseAmount() >= _money)
            {                
                RemoveProduct();
            }
        }

        private void RemoveProduct()
        {
            Random random = new Random();
            int index = random.Next(0, _cart.Count);
            Product product = _cart[index];
            _cart.Remove(product);
            DecorateMessage(ConsoleColor.DarkRed, $"Клиент убрал продукт {product.Name}, ценой в {product.Cost} рублей.");
            Console.ReadKey();
        }

        private void WriteSuccessMessage()
        {
            DecorateMessage(ConsoleColor.Green, $"Клиент оплатил покупку на сумму {GetPurchaseAmount()} рублей.");
            Console.ReadKey();
            Console.Clear();
        }

        private void DecorateMessage(ConsoleColor color, string message)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColor;
        }
    }

    class Product
    {
        public Product(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public string Name { get; private set; }
        public int Cost { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Товар: {Name}, стоимость: {Cost}");
        }
    }
}

