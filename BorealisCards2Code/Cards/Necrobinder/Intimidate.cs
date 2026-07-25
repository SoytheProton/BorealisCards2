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
public class Intimidate() : BorealisCards2Card(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar(10), new PowerVar<VulnerablePower>(2M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon), HoverTipFactory.FromPower<VulnerablePower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, MegaCrit.Sts2.Core.Models.Characters.Necrobinder.GetSummonAnimIfApplicable(Owner.Character), MegaCrit.Sts2.Core.Models.Characters.Necrobinder.GetSummonDelayIfApplicable(Owner.Character));
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.BaseValue, this);
        foreach (var enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Summon.UpgradeValueBy(2M);
        DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}