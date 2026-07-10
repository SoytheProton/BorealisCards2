using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class IonizePower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<LightningOrb>()];

    public override async Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
    {
        if (player != Owner.Player || orb is not LightningOrb)
            return;
        var num = CombatManager.Instance.History.Entries.OfType<OrbChanneledEntry>().Count(o => o.Actor == Owner && o.HappenedThisTurn(Owner.CombatState) && o.Orb is LightningOrb);
        if(num != 1) return;
        for (var i = 0; i < Amount; i++)
            await OrbCmd.Channel(choiceContext, ModelDb.Orb<LightningOrb>().ToMutable(), Owner.Player);
    }
}