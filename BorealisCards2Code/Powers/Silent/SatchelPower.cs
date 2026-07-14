using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace BorealisCards2.BorealisCards2Code.Powers.Silent;

public sealed class SatchelPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner.Player || Owner.Side != Owner.CombatState.CurrentSide || !card.Keywords.Contains(CardKeyword.Sly))
            return;
        var num = CombatManager.Instance.History.Entries.OfType<CardDiscardedEntry>().Count(o => o.Actor == Owner && o.HappenedThisTurn(Owner.CombatState) && o.Card.Keywords.Contains(CardKeyword.Sly));
        if (num != 1) return;
        Flash();
        await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
    }
}