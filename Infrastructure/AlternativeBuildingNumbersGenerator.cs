namespace Infrastructure
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class AlternativeBuildingNumbersGenerator
    {
        public List<string> Generate(string buildingNo)
        {
            var result = new List<string>();

            var let = new Regex(@"[\d-]").Replace(buildingNo, "");
            if (let != string.Empty)
            {
                var dig = buildingNo.Replace(let, "");
                result.AddRange(new List<string>
                                        { $"{dig}{@let.ToLower()}",
                                          $"{dig}{@let.ToUpper()}",
                                          $"{dig} {@let.ToLower()}",
                                          $"{dig} {@let.ToUpper()}",
                                          $"{dig}-{@let.ToLower()}",
                                          $"{dig}-{@let.ToUpper()}",
                                          $"{dig} -{@let.ToLower()}",
                                          $"{dig} -{@let.ToUpper()}",
                                          $"{dig}- {@let.ToLower()}",
                                          $"{dig}- {@let.ToUpper()}",
                                          $"{dig} - {@let.ToLower()}",
                                          $"{dig} - {@let.ToUpper()}",
                                          $"{dig}/{@let.ToLower()}",
                                          $"{dig}/{@let.ToUpper()}",
                                          $"{dig} /{@let.ToLower()}",
                                          $"{dig} /{@let.ToUpper()}",
                                          $"{dig}/ {@let.ToLower()}",
                                          $"{dig}/ {@let.ToUpper()}",
                                          $"{dig} / {@let.ToLower()}",
                                          $"{dig} / {@let.ToUpper()}",
                                          $"{dig}\\{@let.ToLower()}",
                                          $"{dig}\\{@let.ToUpper()}",
                                          $"{dig} \\{@let.ToLower()}",
                                          $"{dig} \\{@let.ToUpper()}",
                                          $"{dig}\\ {@let.ToLower()}",
                                          $"{dig}\\ {@let.ToUpper()}",
                                          $"{dig} \\ {@let.ToLower()}",
                                          $"{dig} \\ {@let.ToUpper()}"
                                        });
            }
            return result;
        }
    }
}
