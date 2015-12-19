using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace JohnJesus.Calculator.Service
{
    /// <summary>
    /// Implementation of async Math invocation 
    /// </summary>
    class MathAsyncResult : JohnJesus.AsyncLibrary.TypedAsyncResult<double>
    {
        FileStream fs;
        MathExpression expr;

        public MathAsyncResult(MathExpression mathExpression, AsyncCallback callback, object state)
            : base(callback, state)
        {
            expr = mathExpression;
            //Turn the expression into an array of bytes
            byte[] bytes = expr.ToBytes();

            //open a file for writing
            fs = File.OpenWrite(Path.GetRandomFileName() + ".txt");

            //begin writing asynchronously
            IAsyncResult result = fs.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(OnWrite), this);

            //if the write did not complete synchronously, we are done setting up this AsyncResult
            if (!result.CompletedSynchronously)
                return;

            //If the write did complete synchronously, then we'll complete the AsyncResult
            CompleteWrite(result, true);
        }

        //Completes asynchronous work.
        //cleans up any resources managed by this AsyncResult
        //Signals the base class that all work is finished
        void CompleteWrite(IAsyncResult result, bool synchronous)
        {
            try
            {
                //Complete the asynchronous file write
                fs.EndWrite(result);
            }
            finally
            {
                //Clean up the file resources
                fs.Close();
            }

            //Calling Complete on the base AsyncResult signals the WaitHandle
            //And makes the callback if necessary
            base.Complete(expr.Result, synchronous);
        }

        void OnWrite(IAsyncResult result)
        {
            //if we returned synchronously, then CompletWrite will be called directly
            if (result.CompletedSynchronously)
                return;

            Console.WriteLine("IO thread for {0} operation on ThreadID {1}", expr.Operation, Thread.CurrentThread.ManagedThreadId);

            try
            {
                //Call CompleteWrite to cleanup resources and complete the AsyncResult
                CompleteWrite(result, false);
            }
            catch (Exception e)
            {
                //if something bad happend, then call the exception overload
                //on the base class.  This will serve up the exception on the
                //AsyncResult
                base.Complete(false, e);
            }
        }
    }
}
