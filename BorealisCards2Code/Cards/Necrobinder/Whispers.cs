using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class Whispers() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Common,
    TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DoomPower>(6),new RepeatVar(3)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        for (int i = 0; i < DynamicVars.Repeat.IntValue; ++i)
        {
            Creature enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            await PowerCmd.Apply<DoomPower>(choiceContext, enemy, DynamicVars.Doom.BaseValue, Owner.Creature, this);
            await Cmd.Wait(0.2f);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1M);
    }
}