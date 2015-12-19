using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace JohnJesus.Calculator
{
    // Define a service contract.
    [ServiceContract(Namespace = "http://JohnJesus.Calculator")]
    public interface ICalc
    {
        [OperationContract]
        double Add(double n1, double n2);

        [OperationContract]
        double Subtract(double n1, double n2);

        //Multiply involves some file I/O so we'll make it Async.
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginMultiply(double n1, double n2, AsyncCallback callback, object state);
        double EndMultiply(IAsyncResult ar);

        //Divide involves some file I/O so we'll make it Async.
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginDivide(double n1, double n2, AsyncCallback callback, object state);
        double EndDivide(IAsyncResult ar);
    }
}
