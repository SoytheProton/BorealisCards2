using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Powers.Ironclad;

public sealed class RushPower : BorealisCards2Power, IHasSecondAmount
{
    private int _secondAmount;
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        Flash();
        await CreatureCmd.Damage(choiceContext, Owner, Amount, ValueProp.Unpowered, Owner);
        await CreatureCmd.GainBlock(Owner, _secondAmount, ValueProp.Unpowered, null);
    }

    public void SetSecondAmount(int amount)
    {
        _secondAmount = amount;
    }

    public string GetSecondAmount()
    {
        return _secondAmount.ToString();
    }
}