using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class Glassblower() : BorealisCards2Card(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<GlassOrb>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await OrbCmd.Channel<GlassOrb>(choiceContext, Owner);
    }
    
    public override async Task AfterCardPlayedLate(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card.Type == CardType.Attack && Filter(cardPlay.Card) && Pile.Type != PileType.Hand)
        {
            var num = CombatManager.Instance.History.CardPlaysFinished.Count(e =>
                e.CardPlay.Card.Type == CardType.Attack && Filter(e.CardPlay.Card) &&
                e.CardPlay.Card.Owner == Owner);
            if (num % DynamicVars.Cards.IntValue != 0)
                return;
            await CardPileCmd.Add(this, PileType.Hand);
        }
    }

    private static bool Filter(CardModel card)
    {
        var flag1 = card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX;
        if (!flag1) return flag1;
        var flag2 = card.Type switch
        {
            CardType.Attack or CardType.Skill or CardType.Power => true,
            _ => false
        };
        flag1 = flag2;
        return flag1;
    }
    
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(-1M);
}
