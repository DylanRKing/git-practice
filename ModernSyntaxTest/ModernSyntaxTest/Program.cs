using System;

internal class Program
{
    public record Detector(string SerialNumber,
                           string SiteName,
                           DateTime LastCalibrated)
    {
        public bool IsOverdue => DateTime.UtcNow - LastCalibrated > TimeSpan.FromDays(180);
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Testing modern syntax and changes in C#");
        var detectors = CreateRecords();
        var valueRes = ValueEqualityTest(detectors);
        MutationTest(detectors[0]);
        var siteRes = FilterSites(detectors, null);
        var siteResFiltered = FilterSites(detectors, "Site A");
        var overdueStatus = OverdueStatusCheck(detectors[1]);
    }

    private static string OverdueStatusCheck(Detector detector) => (DateTime.UtcNow - detector.LastCalibrated).Days switch
    {
        < 180 => "Detector is within calibration period.",
        < 210 => "Detector is overdue for calibration.",
        < 240 => "Detector is no longer within safe calibration limits.",
        < 270 => "Detector is unsafe for use. Automatic shudown is in effect.",
        _ => "Unkown status. Please check the detector manually."
    };

    private static string FilterSites(List<Detector> detectors, string? siteFilter)
    {
        var filteredSites = siteFilter is null
            ? detectors
            : detectors.Where(d => d.SiteName == siteFilter).ToList();
        return string.Join(", ", filteredSites.Select(d => d.ToString()));
    }

    private static void MutationTest(Detector detector)
    {
        Console.WriteLine($"Original Detector: {detector}");
        var altDet = detector with { LastCalibrated = DateTime.UtcNow };
        Console.WriteLine($"Modified Detector: {altDet}");
    }

    private static string ValueEqualityTest(List<Detector> detectors)
    {
        for (int i = 0; i < detectors.Count; i++)
        {
            for (int j = i + 1; j < detectors.Count; j++)
            {
                if (detectors[i] == detectors[j])
                {
                    return $"Detector {i} and Detector {j} are equal.";
                }
            }
        }
        return "No equal detectors found.";
    }

    private static List<Detector> CreateRecords()
    {
        return new List<Detector>
        {
            new Detector(
                SerialNumber: "123456789",
                SiteName: "Site A",
                LastCalibrated: DateTime.Now - TimeSpan.FromDays(20)
            ),
            new Detector(
                SerialNumber: "987654321",
                SiteName: "Site B",
                LastCalibrated: DateTime.Now - TimeSpan.FromDays(200)
            ),
            new Detector(
                SerialNumber: "123456789",
                SiteName: "Site A",
                LastCalibrated: DateTime.Now - TimeSpan.FromDays(20)
            ),
            new Detector(
                SerialNumber: "789123456",
                SiteName: "Site D",
                LastCalibrated: DateTime.Now - TimeSpan.FromDays(300)
            ),
            new Detector(
                SerialNumber: "321654987",
                SiteName: "Site E",
                LastCalibrated: DateTime.Now - TimeSpan.FromDays(50)
            )
        };
    }
}
