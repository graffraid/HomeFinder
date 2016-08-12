namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class AlternativeBuildingNumbersGenerator
    {
        private List<string> result;

        public List<string> Generate(string buildingNo)
        {
            result = new List<string>();

            if (Validate(buildingNo))
            {
                var splitedBuildingNo = buildingNo.Split('/');

                switch (splitedBuildingNo.Count())
                {
                    case 1:
                        var letters = new Regex(@"[\d]").Replace(buildingNo, "");
                        var digits = new Regex(@"[\D]").Replace(buildingNo, "");

                        if (!letters.Any())
                        {
                            return result;
                        }

                        return GetVariants(digits, letters);

                    case 2:
                        var letters0 = new Regex(@"[\d]").Replace(splitedBuildingNo[0], "");
                        var digits0 = new Regex(@"[\D]").Replace(splitedBuildingNo[0], "");
                        var variants0 = new List<string>();
                        var letters1 = new Regex(@"[\d]").Replace(splitedBuildingNo[1], "");
                        var digits1 = new Regex(@"[\D]").Replace(splitedBuildingNo[1], "");
                        var variants1 = new List<string>();

                        if (!letters0.Any())
                        {
                            variants0.Add(splitedBuildingNo[0]);
                        }
                        else
                        {
                            variants0 = GetVariants(digits0, letters0);
                        }

                        if (!letters1.Any())
                        {
                            variants1.Add(splitedBuildingNo[1]);
                        }
                        else
                        {
                            variants1 = GetVariants(digits1, letters1);
                        }

                        return GetSlashVariants(variants0, variants1);

                    default:
                        throw new Exception("Building number validation failed");
                }
            }
            return result;
        }

        private List<string> GetVariants(string partOne, string partTwo)
        {
            return new List<string>
                       {
                           $"{partOne}{@partTwo.ToLower()}",
                           $"{partOne}{@partTwo.ToUpper()}",
                           $"{partOne} {@partTwo.ToLower()}",
                           $"{partOne} {@partTwo.ToUpper()}",
                           $"{partOne}-{@partTwo.ToLower()}",
                           $"{partOne}-{@partTwo.ToUpper()}",
                           $"{partOne} -{@partTwo.ToLower()}",
                           $"{partOne} -{@partTwo.ToUpper()}",
                           $"{partOne}- {@partTwo.ToLower()}",
                           $"{partOne}- {@partTwo.ToUpper()}",
                           $"{partOne} - {@partTwo.ToLower()}",
                           $"{partOne} - {@partTwo.ToUpper()}",
                           $"{partOne}/{@partTwo.ToLower()}",
                           $"{partOne}/{@partTwo.ToUpper()}",
                           $"{partOne} /{@partTwo.ToLower()}",
                           $"{partOne} /{@partTwo.ToUpper()}",
                           $"{partOne}/ {@partTwo.ToLower()}",
                           $"{partOne}/ {@partTwo.ToUpper()}",
                           $"{partOne} / {@partTwo.ToLower()}",
                           $"{partOne} / {@partTwo.ToUpper()}",
                           $"{partOne}\\{@partTwo.ToLower()}",
                           $"{partOne}\\{@partTwo.ToUpper()}",
                           $"{partOne} \\{@partTwo.ToLower()}",
                           $"{partOne} \\{@partTwo.ToUpper()}",
                           $"{partOne}\\ {@partTwo.ToLower()}",
                           $"{partOne}\\ {@partTwo.ToUpper()}",
                           $"{partOne} \\ {@partTwo.ToLower()}",
                           $"{partOne} \\ {@partTwo.ToUpper()}"
                       };
        }

        private List<string> GetSlashVariants(List<string> partOneList, List<string> partTwoList)
        {
            var result = new List<string>();

            foreach (var partOne in partOneList)
            {
                foreach (var partTwo in partTwoList)
                {
                    result.AddRange(new List<string>
                                        {
                                           $"{partOne}-{@partTwo}",
                                           $"{partOne} -{@partTwo}",
                                           $"{partOne}- {@partTwo}",
                                           $"{partOne} - {@partTwo}",
                                           $"{partOne}/{@partTwo}",
                                           $"{partOne} /{@partTwo}",
                                           $"{partOne}/ {@partTwo}",
                                           $"{partOne} / {@partTwo}",
                                           $"{partOne}\\{@partTwo}",
                                           $"{partOne} \\{@partTwo}",
                                           $"{partOne}\\ {@partTwo}",
                                           $"{partOne} \\ {@partTwo}",
                                           });
                }
            }

            return result;
        }

        private bool Validate(string buildingNo)
        {
            if (new Regex(@"[0-9а-яА-я/]").IsMatch(buildingNo) && buildingNo.Count(x => x =='/')<=1)
            {
                return true;
            }
            throw new Exception("Building number validation failed");
        }
    }
}
