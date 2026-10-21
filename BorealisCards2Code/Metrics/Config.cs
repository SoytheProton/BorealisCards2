using BaseLib.Config;

namespace BorealisCards2.BorealisCards2Code.Metrics;

public class Config : SimpleModConfig
{
    [ConfigHoverTip]
    public static bool UploadMetrics { get; set; } = false;

    [ConfigHideInUI]
    [ConfigIgnoreRestoreDefaults]
    public static bool UploadMetricsFtueSeen { get; set; } = false;
}