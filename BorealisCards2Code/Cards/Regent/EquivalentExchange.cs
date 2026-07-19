using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class EquivalentExchange() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    public override int CanonicalStarCost => 2;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StarsVar(2), new BlockVar(4M, ValueProp.Move)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Stars.UpgradeValueBy(1M);
        DynamicVars.Block.UpgradeValueBy(2M);
    }
    
    public override async Task AfterAutoPrePlayPhaseEntered(
        PlayerChoiceContext choiceContext,
        Player player)
    {
        if (player != Owner || player.PlayerCombatState.TurnNumber > 1)
            return;
        await CardCmd.AutoPlay(choiceContext, this, null);
    }
}