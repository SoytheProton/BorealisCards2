using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using Void = MegaCrit.Sts2.Core.Models.Cards.Void;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class StackTrace() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            if (IsUpgraded)
                foreach (var tip in HoverTipFactory.FromCardWithCardHoverTips<Dazed>()) yield return tip;
            else 
                foreach (var tip in HoverTipFactory.FromCardWithCardHoverTips<Void>()) yield return tip;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await OrbCmd.AddSlots(Owner, 1);
        if (IsUpgraded)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner), PileType.Draw, Owner));
        else
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Void>(Owner), PileType.Draw, Owner));
    }
}