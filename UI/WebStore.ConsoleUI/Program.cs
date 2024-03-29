﻿using Microsoft.Extensions.Configuration;
using System;
using WebStore.Clients.Products;

namespace WebStore.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var product_client = new ProductsClient(configuration);

            Console.WriteLine("После запуска Hosting приложения нажмите любую клавишу");
            Console.ReadKey();
            foreach (var product in product_client.GetProducts().Products)
            {
                Console.WriteLine($"{product.Id} - {product.Name} - {product.Price}");
            }
            Console.ReadLine();
        }
    }
}
