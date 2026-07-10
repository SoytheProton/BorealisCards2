using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using Void = MegaCrit.Sts2.Core.Models.Cards.Void;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class HyperspeedStrike() : BorealisCards2Card(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, play.Target, DynamicVars.Damage, this, play);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Void>(Owner), PileType.Draw, Owner));
        await Cmd.Wait(0.5f);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(5m);
}