using BaseLib.Extensions;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Silent;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Silent;






[Pool(typeof(SilentCardPool))]
public sealed class PocketDaggers() : BorealisCards2Card(2,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new PowerVar<PocketDaggersPower>(2)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        for (int i = 0; i < DynamicVars.Cards.IntValue; ++i)
        {
            await Shiv.CreateInHand(Owner, CombatState);
            await Cmd.Wait(0.1f);
        }
        await PowerCmd.Apply<PocketDaggersPower>(choiceContext, Owner.Creature, DynamicVars.Power<PocketDaggersPower>().BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}