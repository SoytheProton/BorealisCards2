using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class VignettePower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromOrb<DarkOrb>()];
    
    public override async Task BeforeSideTurnEndEarly(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        var darkOrbs = Owner.Player?.PlayerCombatState?.OrbQueue.Orbs.OfType<DarkOrb>().ToList();
        if(darkOrbs == null || darkOrbs.Count == 0)
            return;
        Flash();
        for(var i = 0; i < darkOrbs.Count * Amount; i++)
        {
            await OrbCmd.Passive(choiceContext, darkOrbs[0], null);
        }
    }
}