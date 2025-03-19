using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab2
{
    class API
    {
        public List<Game>? games;
        public HttpClient? client;
        public async Task download()
        {
            client = new HttpClient();
            string call = "https://www.freetogame.com/api/games";
            string response = await client.GetStringAsync(call);
            games = JsonSerializer.Deserialize<List<Game>>(response);
        }
    }
}
