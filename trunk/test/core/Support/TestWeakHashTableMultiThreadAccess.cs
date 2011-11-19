using System;
using System.Threading;
using NUnit.Framework;

namespace Lucene.Net.Support
{
    [TestFixture]
    public class TestWeakHashTableMultiThreadAccess
    {
        WeakHashTable wht = new WeakHashTable();
        Exception AnyException = null;
        bool EndOfTest = false;

        [Test]
        public void Test()
        {
            CreateThread(Add);
            CreateThread(Enum);
            
            int count = 200;
            while (count-- > 0)
            {
                Thread.Sleep(50);
                if (AnyException != null)
                {
                    EndOfTest = true;
                    Thread.Sleep(50);
                    Assert.Fail(AnyException.Message);
                }
            }
        }

        void CreateThread(ThreadStart fxn)
        {
            Thread t = new Thread(fxn);
            t.IsBackground = true;
            t.Start();
        }
        

        void Add()
        {
            try
            {
                long count = 0;
                while (EndOfTest==false)
                {
                    wht.Add(count.ToString(), count.ToString());
                    Thread.Sleep(1);

                    string toReplace = (count - 10).ToString();
                    if (wht.Contains(toReplace))
                    {
                        wht[toReplace] = "aa";
                    }

                    count++;
                }
            }
            catch (Exception ex)
            {
                AnyException = ex;
            }
        }

        void Enum()
        {
            try
            {
                while (EndOfTest==false)
                {
                    System.Collections.IEnumerator e = wht.Keys.GetEnumerator();
                    while (e.MoveNext())
                    {
                        string s = (string)e.Current;
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                AnyException = ex;
            }
        }
    }
}