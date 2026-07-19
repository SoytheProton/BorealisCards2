using BorealisCards2.BorealisCards2Code.Cards.Necrobinder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Powers.Necrobinder;

public sealed class ShakedownPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar(Shakedown.SummonAmount)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        Flash();
        await OstyCmd.Summon(choiceContext, Owner.Player, DynamicVars.Summon.BaseValue, this);
        await PowerCmd.Decrement(this);
    }
}