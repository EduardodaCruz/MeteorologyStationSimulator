using MeteorologyStationSimulator.Models;
using NetMQ;
using NetMQ.Sockets;
using System.Collections.Generic;
using System.Text.Json;

namespace MeteorologyStationSimulator
{
    public class WeatherReportPublisher
    {
        public static void Main(string[] args)
        {
            using var subscriber = new SubscriberSocket();
            subscriber.Connect("tcp://localhost:5556");
            subscriber.Subscribe("WeatherReport");
            
            using var publisher = new PublisherSocket();
            publisher.Bind("tcp://*:5557");

            while (true)
            {
                var message = subscriber.ReceiveFrameString();

                var averages = JsonSerializer.Deserialize<List<WeatherAverage>>(message);

                foreach(var average in averages)
                {
                    publisher.SendFrame(JsonSerializer.Serialize(average));
                }
            }
        }
    }
}
