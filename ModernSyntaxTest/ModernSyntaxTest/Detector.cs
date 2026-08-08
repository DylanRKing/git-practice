namespace ModernSyntaxTest;

public record Detector(string SerialNumber,
                           string SiteName,
                           DateTime LastCalibrated)
{
    public bool IsOverdue => DateTime.UtcNow - LastCalibrated > TimeSpan.FromDays(180);
}
