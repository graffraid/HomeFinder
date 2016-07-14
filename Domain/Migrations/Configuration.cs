namespace Domain.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<Domain.FinderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Domain.FinderDbContext context)
        {
            if (!context.Buildings.Any())
            {
                context.Buildings.AddRange(new List<Building>
                                               {
                                                   new Building { Street = "Невского", No = "48" },
                                                   new Building { Street = "Невского", No = "48В" },
                                                   new Building { Street = "Невского", No = "48Г" },
                                                   new Building { Street = "Невского", No = "31А" },
                                                   new Building { Street = "Невского", No = "31Б" },
                                                   new Building { Street = "Невского", No = "35" },
                                                   new Building { Street = "Невского", No = "33" },
                                                   new Building { Street = "Невского", No = "33А" },
                                                   new Building { Street = "Невского", No = "35А" },
                                                   new Building { Street = "Невского", No = "39Б" },
                                                   new Building { Street = "Невского", No = "39Д" },
                                                   new Building { Street = "Московский", No = "123" },
                                                   new Building { Street = "Московский", No = "117А" },
                                                   new Building { Street = "Московский", No = "117Б" },
                                                   new Building { Street = "Московский", No = "117В" }
                                               });
                context.SaveChanges();
            }
        }
    }
}
