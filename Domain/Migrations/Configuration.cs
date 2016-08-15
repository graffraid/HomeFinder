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
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "48" },         //1
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "48В" },        //2
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "48Г" },        //3
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "31А" },        //4
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "31Б" },        //5
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "35" },         //6
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "33" },         //7
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "33А" },        //8
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "35А" },        //9
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "39Б" },        //10
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "39Д" },        //11
                                                   new Building { Street = "Московский Проспект", ShortStreet = "Московский", No = "123" },     //12
                                                   new Building { Street = "Московский Проспект", ShortStreet = "Московский", No = "117А" },    //13
                                                   new Building { Street = "Московский Проспект", ShortStreet = "Московский", No = "117Б" },    //14
                                                   new Building { Street = "Московский Проспект", ShortStreet = "Московский", No = "117В" },    //15
                                                   new Building { Street = "Московский Проспект", ShortStreet = "Московский", No = "131Б" },    //16
                                                   new Building { Street = "Московский Проспект", ShortStreet = "Московский", No = "131В" },    //17
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "46/2" },       //18
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "46/3" },       //19
                                                   new Building { Street = "Владимира Невского", ShortStreet = "Невского", No = "46/4" }        //20
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
                    case "Владимира Невского, 48":
                        latitude = "51.718361";
                        longitude = "39.176147";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 48В":
                        latitude = "51.717455";
                        longitude = "39.175300";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 48Г":
                        latitude = "51.717802";
                        longitude = "39.174624";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 31А":
                        latitude = "51.718408";
                        longitude = "39.170289";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 31Б":
                        latitude = "51.717582";
                        longitude = "39.170740";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 35":
                        latitude = "51.718407";
                        longitude = "39.167643";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 33":
                        latitude = "51.718440";
                        longitude = "39.164650";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 33А":
                        latitude = "51.718473";
                        longitude = "39.165508";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 35А":
                        latitude = "51.718527";
                        longitude = "39.166388";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 39Б":
                        latitude = "51.719766";
                        longitude = "39.165165";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 39Д":
                        latitude = "51.718940";
                        longitude = "39.165218";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 46/2":
                        latitude = "51.715672";
                        longitude = "39.170152";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 46/3":
                        latitude = "51.715266";
                        longitude = "39.170517";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Владимира Невского, 46/4":
                        latitude = "51.714799";
                        longitude = "39.171247";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Московский Проспект, 123":
                        latitude = "51.715665";
                        longitude = "39.177655";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Московский Проспект, 117А":
                        latitude = "51.713659";
                        longitude = "39.178063";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Московский Проспект, 117Б":
                        latitude = "51.714019";
                        longitude = "39.178288";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Московский Проспект, 117В":
                        latitude = "51.714399";
                        longitude = "39.178943";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Московский Проспект, 131Б":
                        latitude = "51.719339";
                        longitude = "39.177007";
                        building.Coordinate = DbGeography.PointFromText($"POINT({latitude} {longitude})", DbGeography.DefaultCoordinateSystemId);
                        break;
                    case "Московский Проспект, 131В":
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

