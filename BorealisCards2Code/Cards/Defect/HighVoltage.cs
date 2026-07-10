using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using Void = MegaCrit.Sts2.Core.Models.Cards.Void;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;


[Pool(typeof(DefectCardPool))]
public class HighVoltage() : BorealisCards2Card(3,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, play.Target, DynamicVars.Damage, this, play);
        await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        if(IsUpgraded)
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}