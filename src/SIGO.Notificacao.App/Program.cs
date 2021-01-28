using System;
using System.Threading;

namespace SIGO.Notificacao.App
{
    class Program
    {
        static void Main(string[] args)
        {
            SmsQueeConsumer.ConsumeQueue();

            Console.WriteLine("Recebendo mensagens...");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
