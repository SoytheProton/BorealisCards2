using BaseLib.Extensions;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Necrobinder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class Shakedown() : BorealisCards2Card(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AllEnemies)
{
    public const int SummonAmount = 3;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new OstyDamageVar(5M, ValueProp.Move), new PowerVar<ShakedownPower>(2M), new SummonVar(SummonAmount)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon)];
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    
    protected override bool ShouldGlowRedInternal => Owner.IsOstyMissing;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Osty.CheckMissingWithAnim(Owner))
            return;
        await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this, play).TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        await PowerCmd.Apply<ShakedownPower>(choiceContext, Owner.Creature, DynamicVars.Power<ShakedownPower>().BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.OstyDamage.UpgradeValueBy(3M);
    }
}