using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Enchantments.Necrobinder;

public sealed class Living : BorealisCards2Enchantment
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar(5M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon)];
    
    public override bool HasExtraCardText => true;
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await OstyCmd.Summon(choiceContext, Card.Owner, DynamicVars.Summon.BaseValue, this);
    }
}