using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class Betelgeuse() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override int CanonicalStarCost => 9;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(2)]; // technically shouldn't need a repeat var but in-case we change it in the future... // I WAS RIGHT
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var cards = await CardPileCmd.Draw(choiceContext, CardPile.MaxCardsInHand - Owner.PlayerCombatState.Hand.Cards.Count, Owner);
        foreach (var card in cards)
        {
            if (card.Type != CardType.Attack) continue;
            for (var i = 0; i < DynamicVars.Repeat.IntValue; ++i)
                await CardCmd.AutoPlay(choiceContext, card, null);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1M);
    }
}