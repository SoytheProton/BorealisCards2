using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class CarbonCopyPower : BorealisCards2Power
{
    private const string _cardKey = "Card";

    public override PowerType Type => PowerType.Buff;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(_cardKey)];

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player == Owner.Player)
        {
            var card = GetInternalData<Data>().selectedCard;
            for (var i = 0; i < Amount; ++i)
            {
                await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Hand, Owner.Player);
            }
            await PowerCmd.Remove(this);
        }
    }

    public void SetSelectedCard(CardModel card)
    {
        var clone = card.CreateClone();
        CardCmd.ClearAffliction(clone);
        GetInternalData<Data>().selectedCard = clone;
        ((StringVar) DynamicVars[_cardKey]).StringValue = clone.Title;
    }

    private class Data
    {
        public CardModel? selectedCard;
    }
}