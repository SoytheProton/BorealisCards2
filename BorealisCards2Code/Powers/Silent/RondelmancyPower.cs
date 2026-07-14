using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Powers.Silent;

public sealed class RondelmancyPower : BorealisCards2Power, IHasSecondAmount
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("ShivCount",3)];

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (RondelShiv.Get(card) || creator == null || creator.Creature != Owner || !card.Tags.Contains(CardTag.Shiv))
            return;
        --DynamicVars["ShivCount"].BaseValue;
        this.InvokeSecondAmountChanged();
        if (DynamicVars["ShivCount"].IntValue <= 0)
        {
            List<CardModel> shivs = [];
            for (var index = 0; index < Amount; ++index)
            {
                var shiv = Owner.CombatState.CreateCard<Shiv>(Owner.Player);
                RondelShiv.Set(shiv, true);
                shivs.Add(shiv);
            }
            await CardPileCmd.AddGeneratedCardsToCombat(shivs, PileType.Hand, Owner.Player);
            DynamicVars["ShivCount"].BaseValue = 3;
            this.InvokeSecondAmountChanged();
        }
    }
    
    public override async Task AfterCardChangedPilesLate(
        CardModel card,
        PileType oldPileType,
        AbstractModel? clonedBy)
    {
        if (card.Owner.PlayerCombatState == null || card.Owner.Creature != Owner || !card.Tags.Contains(CardTag.Shiv) || card.Pile.Type != PileType.Discard)
            return;
        await CardCmd.AutoPlay(new BlockingPlayerChoiceContext(), card, null);
    }

    private static readonly SpireField<CardModel, bool> RondelShiv = new(() => false);
    
    public string GetSecondAmount()
    {
        return DynamicVars["ShivCount"].IntValue.ToString();
    }
    
    /*
     Horrific code to scare your children with.
     public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || RondelMode.Get(creator.PlayerCombatState) || RondelShiv.Get(card) || creator.Creature != Owner || !card.Tags.Contains(CardTag.Shiv))
            return;
        Flash();
        var num = CombatManager.Instance.History.Entries.OfType<CardPlayStartedEntry>().LastOrDefault(c => c.Actor == Owner && c.HappenedThisTurn(Owner.CombatState));
        if(num != null && num.CardPlay.Card.HoverTips.Any(h => h is CardHoverTip { Card: Shiv }) && CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().Any(c => c.HappenedThisTurn(Owner.CombatState) && c.CardPlay != num.CardPlay))
        {
            RondelMode.Set(creator.PlayerCombatState, true);
            await Shiv.CreateInHand(Owner.Player, Amount, Owner.CombatState);
        }
        else
        {
            List<CardModel> shivs = [];
            for (var index = 0; index < Amount; ++index)
            {
                var shiv = Owner.CombatState.CreateCard<Shiv>(Owner.Player);
                RondelShiv.Set(shiv, true);
                shivs.Add(shiv);
            }
            await CardPileCmd.AddGeneratedCardsToCombat(shivs, PileType.Hand, Owner.Player);
        }
    }

    public override Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner || !RondelMode.Get(cardPlay.Card.Owner.PlayerCombatState))
            return Task.CompletedTask;
        RondelMode.Set(cardPlay.Card.Owner.PlayerCombatState, false);
        return Task.CompletedTask;
    }

    private static readonly SpireField<CardModel, bool> RondelShiv = new(() => false);
    private static readonly SpireField<PlayerCombatState, bool> RondelMode = new(() => false);*/
}