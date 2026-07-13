using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public sealed class Err() : BorealisCards2Card(-1,
    CardType.Skill, CardRarity.Rare,
    TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return;
        for (int i = 0; i < DynamicVars.Cards._baseValue; ++i) {
            CardModel card1 = CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.Type == CardType.Power), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
            if (card1 == null)
               return;
            await CardCmd.AutoPlay(choiceContext, card1, null);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1M);
    }
}