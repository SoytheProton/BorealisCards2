using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace BorealisCards2.BorealisCards2Code.Powers.Silent;

public sealed class BejeweledBladePower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    
    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != Owner || !card.Tags.Contains(CardTag.Shiv))
            return Task.CompletedTask;
       /* var num = CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Count(o => o.Actor == Owner && o.HappenedThisTurn(Owner.CombatState) && o.Card.Tags.Contains(CardTag.Shiv));
        if(num != 1) return Task.CompletedTask; */
        Flash();
        card.BaseReplayCount+= Amount;
        return Task.CompletedTask;
    }
}