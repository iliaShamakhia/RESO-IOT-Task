using API.Data.DTOs;
using Microsoft.Extensions.Configuration;
using Simulator.Sensors;

namespace Simulator
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var illumSensor = new IlluminanceSensor();
            illumSensor.Start();

            Console.WriteLine("\nPress the Enter key to exit the application...\n");

            Console.ReadLine();
            illumSensor.Stop();

            Console.WriteLine(await illumSensor.GetDataFromApi());
            Console.ReadLine();

        }

    }
}