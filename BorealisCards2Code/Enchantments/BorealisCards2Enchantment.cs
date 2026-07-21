using BaseLib.Abstracts;
using BaseLib.Extensions;
using BorealisCards2.BorealisCards2Code.Extensions;
using Godot;

namespace BorealisCards2.BorealisCards2Code.Enchantments;

public abstract class BorealisCards2Enchantment : CustomEnchantmentModel
{
    protected override string? CustomIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath();
            return ResourceLoader.Exists(path) ? path : null;
        }
    }
}