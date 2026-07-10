using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;


public sealed class ShockAndAwePower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<LightningOrb>()];

    public override async Task AfterOrbChanneled(PlayerChoiceContext choiceContext, Player player, OrbModel orb)
    {
        if (player != Owner.Player || ShockedAndAwed.Get(orb) || !(orb is LightningOrb))
            return;
        
        for (int i = 0; i < Amount; i++)
        {
            OrbModel orb2 = OrbModel.GetRandomOrb(Owner.Player.RunState.Rng.CombatOrbGeneration).ToMutable();
            ShockedAndAwed.Set(orb2, true);
            await OrbCmd.Channel(choiceContext, orb2, Owner.Player);
        }
    }

    public static readonly SpireField<OrbModel, bool> ShockedAndAwed = new(() => false);
}