using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class NuclearPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<PlasmaOrb>()];
    
    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player)
            return;
        await OrbCmd.Channel<PlasmaOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
        await PowerCmd.Decrement(this);
    }
}