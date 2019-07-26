using MediasAsyncDemo.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediasAsyncDemo.Context
{
    public class MediasContext : DbContext
    {
        public DbSet<Media> Medias { get; set; }

        public MediasContext(DbContextOptions<MediasContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //fill DB with mock data
            modelBuilder.Entity<Channel>().HasData(
                new Channel()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                    Name = "Shield Hero",
                    Type = "Series"
                },
                new Channel()
                {
                    Id = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                    Name = "Your Name",
                    Type = "Movie"
                },
                new Channel()
                {
                    Id = Guid.Parse("24810dfc-2d94-4cc7-aab5-cdf98b83f0c9"),
                    Name = "Goblin Slayer",
                    Type = "Series"
                },
                new Channel()
                {
                    Id = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"),
                    Name = "Fairy Tail",
                    Type = "Series"
                }
                );

            modelBuilder.Entity<Media>().HasData(
                new Media()
                {
                    Id = Guid.Parse("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
                    ChannelId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                    Title = "EP 1 The Shield Hero",
                    Description = "While in the library, college student Naofumi Iwatani finds a fantasy book about \"Four Heroes\"; The Spear, Sword, Bow, and Shield."
                },
                new Media()
                {
                    Id = Guid.Parse("d8663e5e-7494-4f81-8739-6e0de1bea7ee"),
                    ChannelId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                    Title = "Your Name",
                    Description = "A teenage boy and girl embark on a quest to meet each other for the first time after they magically swap bodies."
                },
                new Media()
                {
                    Id = Guid.Parse("d173e20d-159e-4127-9ce9-b0ac2564ad97"),
                    ChannelId = Guid.Parse("24810dfc-2d94-4cc7-aab5-cdf98b83f0c9"),
                    Title = "EP 1 The Fate of Particular Adventurers",
                    Description = "On Priestes first official adventure, she and her party of novices fall victim to murderous goblins."
                },
                new Media()
                {
                    Id = Guid.Parse("493c3228-3444-4a49-9cc0-e8532edc59b2"),
                    ChannelId = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"),
                    Title = "EP 1 The Fairy Tail",
                    Description = "When a phony wizard lures Lucy onto his ship with the promise of getting into the guild, her new friends must bail her out."
                }
                );
        }
    }
}
