namespace Domain.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<FinderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FinderDbContext context)
        {
            if (!context.Buildings.Any())
            {
                context.Buildings.AddRange(new List<Building>
                                               {
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "48" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "48�" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "48�" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "31�" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "31�" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "35" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "33" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "33�" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "35�" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "39�" },
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "39�" },
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "123" },
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "117�" },
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "117�" },
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "117�" }
                                               });
                context.SaveChanges();
            }
        }
    }
}
