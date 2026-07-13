using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace BorealisCards2.BorealisCards2Code.Powers.Ironclad;

public sealed class RoaringFlamesPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        if(card.Owner.Creature != Owner && card.Type != CardType.Attack)
            return;
        var pile = PileType.Exhaust.GetPile(Owner.Player);
        for (var i = 0; i < Amount; ++i)
        {
            var list = pile.Cards.Where(c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList();
            var exhaustedCard = Owner.Player.RunState.Rng.Shuffle.NextItem(list);
            
            if (exhaustedCard == null) continue;
            
            await CardCmd.AutoPlay(choiceContext, exhaustedCard, null);
            await CardPileCmd.Add(exhaustedCard, PileType.Exhaust, CardPilePosition.Bottom, this, true);
        }
    }
}