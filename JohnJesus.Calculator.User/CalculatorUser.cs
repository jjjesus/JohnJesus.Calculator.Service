using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;


using JohnJesus.Calculator;

namespace JohnJesus.Calculator.User
{
    public class CalculatorUser
    {
        public static void Run()
        {
            Console.WriteLine("Press <ENTER> to terminate client once the output is displayed.");
            Console.WriteLine();


            var client = new CalcClient(
                new WSHttpBinding(SecurityMode.None),
                new EndpointAddress("http://localhost:8000/JohnJesus.Calculator.Service/service"));

            try
            {
                double value1 = 100.00D;
                double value2 = 15.99D;
                // Add
                //client.Add(value1, value2);
                //Console.WriteLine("Add({0},{1})", value1, value2);

                // AddAsync
                client.AddCompleted += new EventHandler<AddCompletedEventArgs>(AddCallback);
                client.AddAsync(value1, value2);
                Console.WriteLine("AddAsync({0},{1})", value1, value2);

                // Subtract
                //value1 = 145.00D;
                //value2 = 76.54D;
                //client.Subtract(value1, value2);
                //Console.WriteLine("Subtract({0},{1})", value1, value2);

                // SubtractAsync
                client.SubtractCompleted += new EventHandler<SubtractCompletedEventArgs>(SubtractCallback);
                client.SubtractAsync(value1, value2);
                Console.WriteLine("SubtractAsync({0},{1})", value1, value2);

                // Multiply
                value1 = 9.00D;
                value2 = 81.25D;
                client.MultiplyCompleted += new EventHandler<MultiplyCompletedEventArgs>(MultiplyCallback);
                client.MultiplyAsync(value1, value2);

                // Divide
                value1 = 22.00D;
                value2 = 7.00D;
                client.DivideCompleted += new EventHandler<DivideCompletedEventArgs>(DivideCallback);
                client.DivideAsync(value1, value2);

                //Closing the client gracefully closes the connection and cleans up resources
                ((ICommunicationObject)client).Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (client != null)
                {
                    ((ICommunicationObject)client).Abort();
                }
            }


            Console.ReadLine();

        }

        // Asynchronous callbacks for displaying results.
        static void MultiplyCallback(object sender, MultiplyCompletedEventArgs e)
        {
            Console.WriteLine("Multiply Result: {0}", e.Result);
        }

        static void DivideCallback(object sender, DivideCompletedEventArgs e)
        {
            Console.WriteLine("Divide Result: {0}", e.Result);
        }

        static void AddCallback(object sender, AddCompletedEventArgs e)
        {
            Console.WriteLine("Add Result: {0}", e.Result);
        }

        static void SubtractCallback(object sender, SubtractCompletedEventArgs e)
        {
            Console.WriteLine("Subtract Result: {0}", e.Result);
        }
    }
}
