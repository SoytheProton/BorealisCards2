using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace BorealisCards2.BorealisCards2Code.Powers.Regent;

public sealed class LoseStarNextTurnPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        await PlayerCmd.GainStars(Amount, player);
        await PowerCmd.Remove(this);
    }
}