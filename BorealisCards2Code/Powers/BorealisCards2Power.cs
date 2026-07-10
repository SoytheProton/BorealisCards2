using BaseLib.Abstracts;
using BaseLib.Extensions;
using BorealisCards2.BorealisCards2Code.Extensions;
using Godot;

namespace BorealisCards2.BorealisCards2Code.Powers;

public abstract class BorealisCards2Power : CustomPowerModel
{
    //Loads from BorealisCards2/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}