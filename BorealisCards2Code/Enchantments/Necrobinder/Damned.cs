using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Enchantments.Necrobinder;

public sealed class Damned : BorealisCards2Enchantment
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DoomPower>(3M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DoomPower>()];
    
    public override bool HasExtraCardText => true;
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await PowerCmd.Apply<DoomPower>(choiceContext, Card.Owner.Creature, DynamicVars.Doom.BaseValue, Card.Owner.Creature, Card);
    }
}