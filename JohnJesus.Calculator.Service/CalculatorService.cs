using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.ServiceModel.Description;


namespace JohnJesus.Calculator.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CalculatorService : ICalc
    {
        public double Add(double n1, double n2)
        {
            Console.WriteLine("Received Add Synchronously on ThreadID {0}:  Sleeping for 3 seconds", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(3000);
            Console.WriteLine("Returning Add Result on ThreadID {0}", Thread.CurrentThread.ManagedThreadId);
            return n1 + n2;
        }

        public double Subtract(double n1, double n2)
        {
            Console.WriteLine("Received Subtract Synchronously on ThreadID {0}:  Sleeping for 3 seconds", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(3000);
            Console.WriteLine("Returning Subtract Result on ThreadID {0}", Thread.CurrentThread.ManagedThreadId);
            return n1 - n2;
        }

        public IAsyncResult BeginMultiply(double n1, double n2, AsyncCallback callback, object state)
        {
            Console.WriteLine("Asynchronous call: BeginMultiply on ThreadID {0}", Thread.CurrentThread.ManagedThreadId);
            //return an AsyncResult
            return new MathAsyncResult(new MathExpression(n1, n2, "*"), callback, state);
        }

        public double EndMultiply(IAsyncResult ar)
        {
            Console.WriteLine("EndMultiply called on ThreadID {0}", Thread.CurrentThread.ManagedThreadId);
            //use the AsyncResult to complete that async operation
            return MathAsyncResult.End(ar);
        }

        public IAsyncResult BeginDivide(double n1, double n2, AsyncCallback callback, object state)
        {
            Console.WriteLine("Asynchronous call: BeginDivide on ThreadID {0}", Thread.CurrentThread.ManagedThreadId);
            //return an AsyncResult
            return new MathAsyncResult(new MathExpression(n1, n2, "/"), callback, state);
        }

        public double EndDivide(IAsyncResult ar)
        {
            Console.WriteLine("EndDivide called on ThreadID {0}", Thread.CurrentThread.ManagedThreadId);
            //use the AsyncResult to complete that async operation
            return MathAsyncResult.End(ar);
        }

        // Host the service within this EXE console application.
        public static void Run()
        {
            // Create a ServiceHost for the CalculatorService type.
            string uri = "http://localhost:8000/JohnJesus.Calculator.Service/service";
            using (ServiceHost serviceHost = new ServiceHost(typeof(CalculatorService), new Uri(uri)))
            {
                // Check to see if the service host already has a ServiceMetadataBehavior
                ServiceMetadataBehavior smb = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                // If not, add one
                if (smb == null)
                    smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                serviceHost.Description.Behaviors.Add(smb);
                // Add MEX endpoint
                serviceHost.AddServiceEndpoint(
                  ServiceMetadataBehavior.MexContractName,
                  MetadataExchangeBindings.CreateMexHttpBinding(),
                  "mex"
                );
                // Add application endpoint
                serviceHost.AddServiceEndpoint(typeof(ICalc), new WSHttpBinding(SecurityMode.None), "");

                // Open the ServiceHost to create listeners and start listening for messages.
                serviceHost.Open();

                // The service can now be accessed.
                Console.WriteLine("The service is ready at {0}", uri);
                Console.WriteLine("Metadata ready at {0}", uri + "/mex");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }

}
