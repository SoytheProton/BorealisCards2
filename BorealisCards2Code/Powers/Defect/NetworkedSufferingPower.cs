using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class NetworkedSufferingPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Evoke)];
    
    public override async Task AfterOrbEvoked(
        PlayerChoiceContext choiceContext,
        OrbModel orb,
        IEnumerable<Creature> targets)
    {
        if (orb.Owner != Owner.Player || Owner.Player.PlayerCombatState?.OrbQueue.Orbs[0] == null)
            return;
        Flash();
        for(var i = 0; i < Amount; i++)
        {
            await OrbCmd.Passive(choiceContext, Owner.Player.PlayerCombatState.OrbQueue.Orbs[0], null);
        }
    }
}