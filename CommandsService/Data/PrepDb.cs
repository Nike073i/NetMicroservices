using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
            var platforms = grpcClient.ReturnAllPlatforms();
            SeedData(serviceScope.ServiceProvider.GetRequiredService<ICommandRepo>(), platforms);
        }

        private static void SeedData(ICommandRepo repository, IEnumerable<Platform> platforms)
        {
            System.Console.WriteLine("--> Seeding new platforms");
            foreach (var platform in platforms)
            {
                if (!repository.ExternalPlatformExists(platform.ExternalID))
                {
                    repository.CreatePlatform(platform);
                }
            }
            repository.SaveChanges();
        }
    }
}