
using Almawave.xmpp.XMPP;
using System;

namespace Almawave.xmpp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var client = new Almawave.xmpp.XMPP.xmpp();
            client.cmdStart_Click();

      
        }
    }
}
