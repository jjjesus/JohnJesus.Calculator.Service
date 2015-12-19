using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace JohnJesus.Calculator.Service
{
    public class MathExpression
    {
        private double n1;
        private double n2;
        private string operation;

        public MathExpression(double n1, double n2, string operation)
        {
            this.n1 = n1;
            this.n2 = n2;
            this.operation = operation;
        }

        public double N1
        {
            get { return n1; }
        }

        public double N2
        {
            get { return n2; }
        }

        public string Operation
        {
            get { return operation; }
        }

        public double Result
        {
            get
            {
                switch (Operation)
                {
                    case "+":
                        return N1 + N2;
                    case "-":
                        return N1 - N2;
                    case "*":
                        return N1 * N2;
                    case "/":
                        return N1 / N2;
                    default:
                        throw new InvalidOperationException("could not handle " + Operation + " operation.");
                }
            }
        }

        public byte[] ToBytes()
        {
            return Encoding.Unicode.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} = {3}", N1, Operation, N2, Result));
        }
    }
}
