using BaseLib.Abstracts;
using BaseLib.Extensions;
using BorealisCards2.BorealisCards2Code.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards;

public abstract class BorealisCards2Card(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    public override string? CustomPortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".BigCardImagePath();;
        }
    }
    
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
}