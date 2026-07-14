using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Powers.Silent;

public sealed class RondelmancyPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != Owner || !card.Tags.Contains(CardTag.Shiv) || RondelShiv.Get(card))
            return;
        Flash();
        var shivs = await Shiv.CreateInHand(Owner.Player, Amount, Owner.CombatState);
        foreach (var shiv in shivs)
        {
            RondelShiv.Set(shiv, true);
        }
    }
    
    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner.Player || !card.Tags.Contains(CardTag.Shiv))
            return; 
        await CardCmd.AutoPlay(choiceContext, card, null);   
    }
    
    private static readonly SpireField<CardModel, bool> RondelShiv = new(() => false);
}