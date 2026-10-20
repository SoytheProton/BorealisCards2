using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class FifoQueuePower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Card")];

    public void SetSelectedCard(CardModel card)
    {
        var deckVersion = card.DeckVersion!;
        CardCmd.ClearAffliction(deckVersion);
        GetInternalData<Data>().selectedCard = deckVersion;
        ((StringVar) DynamicVars["Card"]).StringValue = deckVersion.Title;
    }
    
    public override async Task AfterCombatEnd(CombatRoom _)
    {
        var card = GetInternalData<Data>().selectedCard;
        if (card?.Pile?.Type != PileType.Deck)
            return;
        var teammates = CombatState.GetTeammatesOf(Owner).Where(c => c is { IsAlive: true, IsPlayer: true } && c.Player != Owner.Player).ToList();
        var player = Owner.Player?.RunState.Rng.CombatTargets.NextItem(teammates)?.Player;
        if(player == null)
            return;
        var clonedCard = card.CreateCloneForPlayer(player);
        await CardPileCmd.RemoveFromDeck(card);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(clonedCard, PileType.Deck));
    }

    private class Data
    {
        public CardModel? selectedCard;
    }
}