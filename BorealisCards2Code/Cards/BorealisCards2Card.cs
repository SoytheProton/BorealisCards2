using BaseLib.Abstracts;
using BaseLib.Extensions;
using BorealisCards2.BorealisCards2Code.ArtRoller;
using BorealisCards2.BorealisCards2Code.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards;

public abstract class BorealisCards2Card(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath
    {
        get
        {
            var cardId = Id.ToString();
            
            var userHsv = CardArtRoller.GetCardData(cardId);
            if (userHsv != null && !string.IsNullOrWhiteSpace(userHsv.PortraitPath))
            {
                return userHsv.PortraitPath;
            }

            var defaultHsv = CardArtRoller.GetDefaultHsvForCard(cardId);
            if (defaultHsv != null && !string.IsNullOrWhiteSpace(defaultHsv.PortraitPath))
            {
                return defaultHsv.PortraitPath;
            }
            
            return File.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath())
                ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath()
                : "card.png".BigCardImagePath();
        }
    }

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
}