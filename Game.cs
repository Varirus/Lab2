using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab2
{
    internal class Game
    {
        public int id { get; set; }
        public required string title { get; set; }
        public string? short_description { get; set; }
        public string? game_url { get; set; }
        public required string genre { get; set; }
        public required string platform { get; set; }
        public string? publisher { get; set; }
        public string? developer { get; set; }
        public string? release_date { get; set; }

        public override string ToString()
        {
            return $"id: {id},\t" +
                $"title: {title},\t" +
                $"short_description: {short_description},\t" +
                $"game_url: {game_url},\t" +
                $"genre: {genre},\t" +
                $"platform: {platform},\t" +
                $"publisher: {publisher},\t" +
                $"developer: {developer},\t" +
                $"release_date: {release_date},\t";

        }
    }
}
