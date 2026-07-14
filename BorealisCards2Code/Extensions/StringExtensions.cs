namespace BorealisCards2.BorealisCards2Code.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(BorealisCards2Main.ModId, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(BorealisCards2Main.ModId, "images", "card_portraits", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(BorealisCards2Main.ModId, "images", "card_portraits", "big", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(BorealisCards2Main.ModId, "images", "powers", path);
    }

    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(BorealisCards2Main.ModId, "images", "powers", "big", path);
    }

    public static string EnchantmentImagePath(this string path)
    {
        return Path.Join(BorealisCards2Main.ModId, "images", "enchantments", path);
    }
}