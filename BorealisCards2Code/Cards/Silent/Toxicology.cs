using BaseLib.Extensions;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Ironclad;
using BorealisCards2.BorealisCards2Code.Powers.Silent;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Cards.Silent;


[Pool(typeof(SilentCardPool))]
public sealed class Toxicology() : BorealisCards2Card(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ToxicologyPower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>()];
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<ToxicologyPower>(choiceContext, Owner.Creature, DynamicVars.Power<ToxicologyPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars.Power<ToxicologyPower>().UpgradeValueBy(1M);
}