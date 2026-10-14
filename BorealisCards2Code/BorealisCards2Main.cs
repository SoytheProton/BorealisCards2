using BaseLib.Config;
using BorealisCards2.BorealisCards2Code.Metrics;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace BorealisCards2.BorealisCards2Code;

[ModInitializer(nameof(Initialize))]
public partial class BorealisCards2Main : Node
{
    public const string ModId = "BorealisCards2"; //Used for resource filepath

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);
    
    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        
        ModManager.OnMetricsUpload += Telemetry.OnMetricsUpload;
        ModConfigRegistry.Register(ModId, new Config());
        MetricsTest.SendTest();
    }
}