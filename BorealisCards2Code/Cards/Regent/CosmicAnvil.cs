using BaseLib.Extensions;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Regent;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class CosmicAnvil() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ForgeVar(9), new PowerVar<CosmicAnvilPower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Forge)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
        await PowerCmd.Apply<CosmicAnvilPower>(choiceContext, Owner.Creature,
            DynamicVars.Power<CosmicAnvilPower>().BaseValue, Owner.Creature, this); 
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Forge.UpgradeValueBy(4M);
        DynamicVars.Power<CosmicAnvilPower>().UpgradeValueBy(1M);
    }
}