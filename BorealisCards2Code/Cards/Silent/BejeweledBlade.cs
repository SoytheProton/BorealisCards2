using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Enchantments;
using BorealisCards2.BorealisCards2Code.Enchantments.Silent;
using BorealisCards2.BorealisCards2Code.Powers.Silent;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards.Silent;

[Pool(typeof(SilentCardPool))]
public sealed class BejeweledBlade() : BorealisCards2Card(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Crystalline>().Prepend(HoverTipFactory.FromCard<Shiv>());
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<BejeweledBladePower>(choiceContext, Owner.Creature, DynamicVars.Repeat.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars.Repeat.UpgradeValueBy(1);
}