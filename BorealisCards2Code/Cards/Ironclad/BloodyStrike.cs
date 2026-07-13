using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Ironclad;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Ironclad;

[Pool(typeof(IroncladCardPool))]
public sealed class BloodyStrike() : BorealisCards2Card(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(17M, ValueProp.Move), new PowerVar<BloodyStrikePower>(1M)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Attack", Owner.Character.AttackAnimDelay);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<BloodyStrikePower>(choiceContext, Owner.Creature, DynamicVars["BloodyStrikePower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6M);
    }
}