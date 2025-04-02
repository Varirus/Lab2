using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Lab2
{
    internal class Game
    {
        public required int id { get; set; }
        public required string title { get; set; }
        public string? short_description { get; set; }
        public required string genre { get; set; }
        public required string platform { get; set; }
        public string? developer { get; set; }
        public string? release_date { get; set; }

        [SetsRequiredMembers]
        public Game(int id,
                    string title,
                    string? short_description,
                    string genre,
                    string platform,
                    string? developer,
                    string? release_date
                    )
        {
            this.id = id;
            this.title = title;
            this.short_description = short_description;
            this.genre = genre;
            this.platform = platform;
            this.developer = developer;
            this.release_date = release_date;
        }
        public override string ToString()
        {
            return $"id: {id},\t" +
                $"title: {title},\t" +
                $"short_description: {short_description},\t" +
                $"genre: {genre},\t" +
                $"platform: {platform},\t" +
                $"developer: {developer},\t" +
                $"release_date: {release_date},\t";

        }
    }
}
