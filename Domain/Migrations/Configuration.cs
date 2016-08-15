namespace Domain.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using Entities;
    using Infrastructure;

    internal sealed class Configuration : DbMigrationsConfiguration<FinderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FinderDbContext context)
        {
            // Seed Buildings
            if (!context.Buildings.Any())
            {
                context.Buildings.AddRange(new List<Building>
                                               {
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "48" },         //1
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "48�" },        //2
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "48�" },        //3
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "31�" },        //4
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "31�" },        //5
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "35" },         //6
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "33" },         //7
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "33�" },        //8
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "35�" },        //9
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "39�" },        //10
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "39�" },        //11
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "123" },     //12
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "117�" },    //13
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "117�" },    //14
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "117�" },    //15
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "131�" },    //16
                                                   new Building { Street = "���������� ��������", ShortStreet = "����������", No = "131�" },    //17
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "46/2" },       //18
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "46/3" },       //19
                                                   new Building { Street = "��������� ��������", ShortStreet = "��������", No = "46/4" }        //20
                                               });
                context.SaveChanges();
            }

            // Seed AlternativeBuildingNumbers
            if (!context.AlternativeBuildingNumbers.Any())
            {
                var buildings = context.Buildings.ToList();

                foreach (var building in buildings)
                {
                    building.AlternativeBuildingNumbers = new List<AlternativeBuildingNumber>();
                    var alternativeBuildingNumbers = (new AlternativeBuildingNumbersGenerator()).Generate(building.No);
                    if (alternativeBuildingNumbers.Count > 0)
                    {
                        foreach (var alternativeBuildingNumber in alternativeBuildingNumbers)
                        {
                            building.AlternativeBuildingNumbers.Add(new AlternativeBuildingNumber { No = alternativeBuildingNumber });
                        }
                    }
                }
                context.SaveChanges();
            }

            // Seed Coordinates
            var allBuildings = context.Buildings.ToList();

            foreach (var building in allBuildings)
            {
                var latitude = string.Empty;
                var longitude = string.Empty;

                switch (building.Address)
                {
                    case "��������� ��������, 48":
                        latitude = "51.718361";
                        longitude = "39.176147";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 48�":
                        latitude = "51.717455";
                        longitude = "39.175300";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 48�":
                        latitude = "51.717802";
                        longitude = "39.174624";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 31�":
                        latitude = "51.718408";
                        longitude = "39.170289";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 31�":
                        latitude = "51.717582";
                        longitude = "39.170740";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 35":
                        latitude = "51.718407";
                        longitude = "39.167643";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 33":
                        latitude = "51.718440";
                        longitude = "39.164650";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 33�":
                        latitude = "51.718473";
                        longitude = "39.165508";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 35�":
                        latitude = "51.718527";
                        longitude = "39.166388";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 39�":
                        latitude = "51.719766";
                        longitude = "39.165165";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 39�":
                        latitude = "51.718940";
                        longitude = "39.165218";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 46/2":
                        latitude = "51.715672";
                        longitude = "39.170152";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 46/3":
                        latitude = "51.715266";
                        longitude = "39.170517";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "��������� ��������, 46/4":
                        latitude = "51.714799";
                        longitude = "39.171247";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "���������� ��������, 123":
                        latitude = "51.715665";
                        longitude = "39.177655";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "���������� ��������, 117�":
                        latitude = "51.713659";
                        longitude = "39.178063";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "���������� ��������, 117�":
                        latitude = "51.714019";
                        longitude = "39.178288";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "���������� ��������, 117�":
                        latitude = "51.714399";
                        longitude = "39.178943";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "���������� ��������, 131�":
                        latitude = "51.719339";
                        longitude = "39.177007";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "���������� ��������, 131�":
                        latitude = "51.719205";
                        longitude = "39.177597";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                }
            }
            context.SaveChanges();
        }
    }
}

