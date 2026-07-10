using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using Void = MegaCrit.Sts2.Core.Models.Cards.Void;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class Metallurgy() : BorealisCards2Card(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private decimal _extraDamageFromPlays;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6M, ValueProp.Move), new ("Increase", 6M)];

    private decimal ExtraDamageFromPlays
    {
        get => _extraDamageFromPlays;
        set
        {
            AssertMutable();
            _extraDamageFromPlays = value;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.Damage(choiceContext, play.Target, DynamicVars.Damage, this, play);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Void>(Owner), PileType.Draw, Owner));
        await Cmd.Wait(0.5f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars["Increase"].UpgradeValueBy(2M);
    }

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        if (card.Owner != Owner)
            return;
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += DynamicVars["Increase"].BaseValue;
        ExtraDamageFromPlays += DynamicVars["Increase"].BaseValue;
        await CardPileCmd.Add(this, PileType.Hand);
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += ExtraDamageFromPlays;
    }
}