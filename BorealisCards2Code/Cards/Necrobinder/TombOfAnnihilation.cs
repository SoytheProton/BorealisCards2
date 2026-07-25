using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Enchantments.Necrobinder;
using BorealisCards2.BorealisCards2Code.Powers.Necrobinder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class TombOfAnnihilation() : BorealisCards2Card(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Damned>().Prepend(HoverTipFactory.FromCard<Soul>(IsUpgraded));

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        if(IsUpgraded) // Yes I'm lazy.
            await PowerCmd.Apply<TombOfAnnihilationUpgradePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        else 
            await PowerCmd.Apply<TombOfAnnihilationPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}