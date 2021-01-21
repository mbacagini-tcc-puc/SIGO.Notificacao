using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SIGO.Notificacao.App.Models;
using System;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SIGO.Notificacao.App
{
    public static class SmsQueeConsumer
    {
        public static void ConsumeQueue()
        {
            var endpoint = Environment.GetEnvironmentVariable("SIGORabbitMq__endpoint");
            var username = Environment.GetEnvironmentVariable("SIGORabbitMq__username");
            var password = Environment.GetEnvironmentVariable("SIGORabbitMq__password");
            
            var factory = new ConnectionFactory { Uri = new Uri(endpoint), UserName = username, Password = password };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "sms",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                EnviarSms(message);
            };

            channel.BasicConsume(queue: "sms",
                                 autoAck: true,
                                 consumer: consumer);

        }

        public static void EnviarSms(string json)
        {
            var payload = JsonConvert.DeserializeObject<SmsQueuePayload>(json);
            var accountSid = Environment.GetEnvironmentVariable("SIGOTwilio__accountSid");
            var authToken = Environment.GetEnvironmentVariable("SIGOTwilio__authToken");
            var twilioNumber = Environment.GetEnvironmentVariable("SIGOTwilio__number");

            TwilioClient.Init(accountSid, authToken);

            MessageResource.Create(
                body: payload.Mensagem,
                from: new Twilio.Types.PhoneNumber(twilioNumber),
                to: new Twilio.Types.PhoneNumber(payload.NumeroCelular)
            );
        }
    }
}
