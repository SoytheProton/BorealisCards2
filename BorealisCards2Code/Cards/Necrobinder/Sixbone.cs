using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class Sixbone() : BorealisCards2Card(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new OstyDamageVar(10M, ValueProp.Move)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.OstyAttack];
    protected override bool ShouldGlowRedInternal => Owner.IsOstyMissing;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (Osty.CheckMissingWithAnim(Owner))
            return;
        await DamageCmd.Attack(DynamicVars.OstyDamage.BaseValue).FromOsty(Owner.Osty, this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.OstyDamage.UpgradeValueBy(3M);
    }
    
    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (delta >= 0M || creature.Monster is not Osty || creature.PetOwner != Owner || Pile.Type == PileType.Hand)
            return;
        await CardPileCmd.Add(this, PileType.Hand);
    }
}