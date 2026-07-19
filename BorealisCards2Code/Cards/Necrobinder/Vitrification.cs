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
    public const int VitrificationAmount = 2;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VitrificationPower>(VitrificationAmount)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<VitrificationPower>(choiceContext, Owner.Creature, 1, Owner.Creature,
                this);
        foreach (var allCard in Owner.PlayerCombatState.AllCards)
        {
            if(allCard != this && !allCard.Keywords.Contains(CardKeyword.Retain) && allCard.EnergyCost.GetWithModifiers(CostModifiers.All) >= VitrificationAmount)
                CardCmd.ApplyKeyword(allCard, CardKeyword.Retain);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}