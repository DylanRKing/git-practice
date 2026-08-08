public record Detector(string SerialNumber,
                           string SiteName,
                           DateTime LastCalibrated)
    {
        public bool IsOverdue => DateTime.UtcNow - LastCalibrated > TimeSpan.FromDays(180);
    }

internal class Program
{
    

    private static void Main(string[] args)
    {
        Console.WriteLine("Testing modern syntax and changes in C#");
        var detectors = CreateRecords();
        var valueRes = ValueEqualityTest(detectors);
        Console.WriteLine(valueRes);
        MutationTest(detectors[0]);
        var siteRes = FilterSites(detectors, null);
        Console.WriteLine(siteRes);
        var siteResFiltered = FilterSites(detectors, "Site A");
        Console.WriteLine(siteResFiltered);
        foreach (var detector in detectors)
        {
            var overdueStatus = OverdueStatusCheck(detectors[1]);
            Console.WriteLine(overdueStatus);
        }
    }

    private static string OverdueStatusCheck(Detector detector)
    {
        if (!detector.IsOverdue) return "Detector is within calibration period.";
        var daysSinceCalibration = (DateTime.UtcNow - detector.LastCalibrated).Days;
        return daysSinceCalibration switch
        {
            < 0 => "Invalid calibration date.",
            < 210 => "Detector is overdue for calibration.",
            < 240 => "Detector is no longer within safe calibration limits.",
            < 270 => "Detector is unsafe for use. Automatic shutdown is in effect.",
            _ => "Unknown status. Please check the detector manually."
        };
    }

    private static List<Detector> FilterSites(List<Detector> detectors, string? siteFilter)
    {
        var filteredSites = siteFilter is null
            ? detectors
            : detectors.Where(d => d.SiteName == siteFilter).ToList();
        return filteredSites;
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
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(20)
            ),
            new Detector(
                SerialNumber: "987654321",
                SiteName: "Site B",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(200)
            ),
            new Detector(
                SerialNumber: "123456789",
                SiteName: "Site A",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(20)
            ),
            new Detector(
                SerialNumber: "789123456",
                SiteName: "Site D",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(300)
            ),
            new Detector(
                SerialNumber: "321654987",
                SiteName: "Site E",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(50)
            )
        };
    }
}
