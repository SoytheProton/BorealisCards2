using BaseLib.Extensions;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Necrobinder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class Vitrification() : BorealisCards2Card(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    private const string ThresholdKey = "Threshold";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar(ThresholdKey, 3)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        if (Owner.HasPower<VitrificationPower>())
        {
            var power = Owner.Creature.GetPower<VitrificationPower>();
            if(power.DynamicVars[ThresholdKey].BaseValue > DynamicVars[ThresholdKey].BaseValue)
                power.DynamicVars[ThresholdKey].BaseValue = DynamicVars[ThresholdKey].BaseValue;
            await PowerCmd.Apply<VitrificationPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this); // Applied just for the visuals and sfx.
        }
        else
        {
            var power = ModelDb.Power<VitrificationPower>().ToMutable();
            power.DynamicVars[ThresholdKey].BaseValue = DynamicVars[ThresholdKey].BaseValue;
            await PowerCmd.Apply(choiceContext, power, Owner.Creature, 1, Owner.Creature,
                this);
        }
        foreach (var allCard in Owner.PlayerCombatState.AllCards)
        {
            if(allCard != this && !allCard.Keywords.Contains(CardKeyword.Retain) && allCard.EnergyCost.GetWithModifiers(CostModifiers.All) >= DynamicVars[ThresholdKey].BaseValue)
                CardCmd.ApplyKeyword(allCard, CardKeyword.Retain);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars[ThresholdKey].UpgradeValueBy(-1);
    }
}