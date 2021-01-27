using System;

namespace SIGO.Notificacao.App
{
    class Program
    {
        static void Main(string[] args)
        {
            SmsQueeConsumer.ConsumeQueue();

            Console.WriteLine("Recebendo mensagens...");
            Console.ReadLine();
        }
    }
}
