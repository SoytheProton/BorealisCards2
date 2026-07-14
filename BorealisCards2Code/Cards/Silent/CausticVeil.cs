using BaseLib.Extensions;
using BaseLib.Patches.Features;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Silent;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Silent;





[Pool(typeof(SilentCardPool))]
public sealed class CausticVeil() : BorealisCards2Card(2,
    CardType.Skill, CardRarity.Rare,
    CustomTargetType.Everyone)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(6), new BlockVar(14M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        foreach (Creature hittableEnemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<PoisonPower>(choiceContext, hittableEnemy, DynamicVars.Poison.BaseValue, Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4M);
        DynamicVars.Poison.UpgradeValueBy(2M);
    }
}