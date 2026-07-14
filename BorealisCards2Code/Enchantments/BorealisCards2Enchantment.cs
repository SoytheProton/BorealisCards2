using BaseLib.Abstracts;
using BaseLib.Extensions;
using BorealisCards2.BorealisCards2Code.Extensions;

namespace BorealisCards2.BorealisCards2Code.Enchantments;

public abstract class BorealisCards2Enchantment : CustomEnchantmentModel
{
    protected override string? CustomIconPath => File.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath()) ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath() : null;
}