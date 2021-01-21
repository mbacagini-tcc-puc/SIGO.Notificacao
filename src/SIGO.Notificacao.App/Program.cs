using System;
using System.Threading;
using System.Threading.Tasks;

namespace SIGO.Notificacao.App
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                SmsQueeConsumer.ConsumeQueue();
            }).Start();

            Console.WriteLine("Recebendo mensagens...");
            Console.ReadLine();
        }
    }
}
