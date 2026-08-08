using ModernSyntaxTest;


Console.WriteLine("Testing modern syntax and changes in C#");
var detectors = CreateRecords();
var valueRes = ReturnFirstDuplicate(detectors);
Console.WriteLine(valueRes);
MutationTest(detectors[0]);
var siteRes = FilterSites(detectors, null);
Console.WriteLine(siteRes.ToString());
var siteResFiltered = FilterSites(detectors, "Site A");
Console.WriteLine(string.Join(", ", siteResFiltered.Select(d => d.SiteName)));
foreach (var detector in detectors)
{
    var overdueStatus = OverdueStatusCheck(detector);
    Console.WriteLine(overdueStatus);
}

static string OverdueStatusCheck(Detector detector)
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

static List<Detector> FilterSites(List<Detector> detectors, string? siteFilter)
{
    var filteredSites = siteFilter is null
        ? detectors
        : detectors.Where(d => d.SiteName == siteFilter).ToList();
    return filteredSites;
}

static void MutationTest(Detector detector)
{
    Console.WriteLine($"Original Detector: {detector}");
    var altDet = detector with { LastCalibrated = DateTime.UtcNow };
    Console.WriteLine($"Modified Detector: {altDet}");
}

static string ReturnFirstDuplicate(List<Detector> detectors)
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

static List<Detector> CreateRecords()
{
    return
        [
            new (
                SerialNumber: "123456789",
                SiteName: "Site A",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(20)
            ),
            new (
                SerialNumber: "987654321",
                SiteName: "Site B",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(200)
            ),
            new (
                SerialNumber: "123456789",
                SiteName: "Site A",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(20)
            ),
            new (
                SerialNumber: "789123456",
                SiteName: "Site D",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(300)
            ),
            new (
                SerialNumber: "321654987",
                SiteName: "Site E",
                LastCalibrated: DateTime.UtcNow - TimeSpan.FromDays(50)
            )
        ];
}