using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Enchantments.Silent;

public sealed class Crystalline : BorealisCards2Enchantment
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2M, ValueProp.Move), new DamageVar(1M, ValueProp.Move), new RepeatVar("Times",1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, DynamicVars["Times"])];
    
    public override bool HasExtraCardText => true;
    
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await CreatureCmd.GainBlock(Card.Owner.Creature, DynamicVars.Block, cardPlay);
    }

    public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
    {
        return !props.IsPoweredAttack() ? 0M : DynamicVars.Damage.BaseValue;
    }
    
    public override int EnchantPlayCount(int originalPlayCount)
    {
        return originalPlayCount + DynamicVars["Times"].IntValue;
    }
}