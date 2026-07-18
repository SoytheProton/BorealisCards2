using BorealisCards2.BorealisCards2Code.Cards.Token.MagicalGirlCards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BorealisCards2.BorealisCards2Code.Powers.Regent;

public sealed class MagicalGirlPower : BorealisCards2Power
{
    private bool _isUpgraded;

    private bool IsUpgraded
    {
        get => _isUpgraded;
        set
        {
            AssertMutable();
            _isUpgraded = value;
            ((BoolVar)DynamicVars["IsUpgraded"]).BoolVal = value;
        }
    }
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BoolVar("IsUpgraded")];
    
    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner.Player)
            return;
        Flash();
        List<CardModel> cards = [];
        for (var index = 0; index < Amount; ++index)
        {
            var card = combatState.CreateCard<PowerOfFriendship>(Owner.Player);
            if(IsUpgraded) CardCmd.Upgrade(card);
            cards.Add(card);
        }
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner.Player);
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        IsUpgraded = cardSource.IsUpgraded;
        return Task.CompletedTask;
    }
}