using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Ironclad;

[Pool(typeof(IroncladCardPool))]
public sealed class Hearth() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    public override bool GainsBlock => true;
    protected override bool ShouldGlowGoldInternal => Owner.Creature.Block > 0;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8M, ValueProp.Move), new HealVar(2M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3M);
        DynamicVars.Heal.UpgradeValueBy(1M);
    }
}