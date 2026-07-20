using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class EbonyGaze() : BorealisCards2Card(3,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DoomPower>(20), new SummonVar(12M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DoomPower>(), HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, MegaCrit.Sts2.Core.Models.Characters.Necrobinder.GetSummonAnimIfApplicable(Owner.Character), MegaCrit.Sts2.Core.Models.Characters.Necrobinder.GetSummonDelayIfApplicable(Owner.Character));
        await PowerCmd.Apply<DoomPower>(choiceContext, play.Target, DynamicVars.Doom.BaseValue, Owner.Creature, this);
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Doom.UpgradeValueBy(5M);
        DynamicVars.Summon.UpgradeValueBy(3M);
    }
}