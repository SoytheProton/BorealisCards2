using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class KingOfHearts() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        for (var i = 0; i < DynamicVars.Cards._baseValue; i++)
        {
            CardModel card = Owner.Creature.CombatState.CreateCard(ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.Tags.Contains(CardTag.Minion)).TakeRandom(1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault(), Owner);
           
            if (card == null) {
                Log.Info("Minions don't exist, lol, lmao.");
                return; }
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}