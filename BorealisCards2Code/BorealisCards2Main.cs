using System.Reflection;
using BorealisCards2.BorealisCards2Code.ArtRoller;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace BorealisCards2.BorealisCards2Code;

[ModInitializer(nameof(Initialize))]
public partial class BorealisCards2Main : Node
{
    public const string ModId = "BorealisCards2"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static readonly string CardsDirectory = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
        "ArtRoller");
    
    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        Directory.CreateDirectory(CardsDirectory);
        CardArtRoller.RegisterAllFromDirectory(CardsDirectory);
        harmony.PatchAll();
    }
}