using BorealisCards2.BorealisCards2Code.Cards.Regent;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BorealisCards2.BorealisCards2Code.Powers.Regent;

public sealed class StarryHammerPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Forge)];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ForgeVar(StarryHammer.PowerForge)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        Flash();
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner.Player, this);
        await PowerCmd.Decrement(this);
    }
}