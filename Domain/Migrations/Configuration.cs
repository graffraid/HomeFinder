namespace Domain.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text.RegularExpressions;
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

            if (!context.AlternativeBuildingNumbers.Any())
            {
                var allBuildings = context.Buildings.ToList();
                var buildings = allBuildings.Where(x => !Regex.IsMatch(x.No, "^[0-9]*$")).ToList();
                foreach (var building in buildings)
                {
                    var let = new Regex(@"[\d-]").Replace(building.No, "");
                    var dig = building.No.Replace(let, "");
                    building.AlternativeBuildingNumbers = new List<AlternativeBuildingNumber>
                                                              {
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}{1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}{1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} {1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} {1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}-{1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}-{1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} -{1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} -{1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}- {1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}- {1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} - {1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} - {1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}/{1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}/{1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} /{1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} /{1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}/ {1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}/ {1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} / {1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} / {1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}\\{1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}\\{1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} \\{1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} \\{1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}\\ {1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0}\\ {1}", dig, let.ToUpper())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} \\ {1}", dig, let.ToLower())},
                                                                 new AlternativeBuildingNumber{No = string.Format("{0} \\ {1}", dig, let.ToUpper())}
                                                              };
                }
                context.SaveChanges();
            }
        }
    }
}
