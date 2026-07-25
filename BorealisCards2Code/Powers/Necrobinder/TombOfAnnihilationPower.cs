using BorealisCards2.BorealisCards2Code.Enchantments.Necrobinder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Rooms;

namespace BorealisCards2.BorealisCards2Code.Powers.Necrobinder;

public sealed class TombOfAnnihilationPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Damned>().Prepend(HoverTipFactory.FromCard<Soul>());

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        for (var i = 0; i < Amount; i++)
        {
            var soul = Owner.Player.RunState.CreateCard<Soul>(Owner.Player);
            CardCmd.Enchant<Damned>(soul, 1);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(soul, PileType.Deck));
        }
    }
}