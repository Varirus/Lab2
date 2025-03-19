using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

[assembly: InternalsVisibleTo("Lab2_Forms")]
namespace Lab2
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            API api = new API();
            api.download().Wait();
            if (api.games == null)
            {
                Console.WriteLine("Failed to download games or no games available.");
                return;
            }
            List<Game> games = api.games;
            Console.WriteLine(games.Count);
            foreach (var game in games)
            {
                Console.WriteLine(game);
                Thread.Sleep(10);

            }
        }
    }
}