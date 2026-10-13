using BorealisCards2.BorealisCards2Code.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class CarbonCopyPower : BorealisCards2Power
{
    public const string _cardKey = "Card";

    public override PowerType Type => PowerType.Buff;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("Card")];

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        CardModel card;
        if (player == Owner.Player)
        {
            card = GetInternalData<NightmarePower.Data>().selectedCard;
            for (int i = 0; i < Amount; ++i)
            {
                await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Hand, Owner.Player);
            }
            await PowerCmd.Remove(this);
        }
    }

    public void SetSelectedCard(CardModel card)
    {
        CardModel clone = card.CreateClone();
        CardCmd.ClearAffliction(clone);
        GetInternalData<NightmarePower.Data>().selectedCard = clone;
        ((StringVar) DynamicVars["Card"]).StringValue = clone.Title;
    }

    public class Data
    {
        public CardModel? selectedCard;
    }
}