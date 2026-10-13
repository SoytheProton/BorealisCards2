using BorealisCards2.BorealisCards2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class PacketLossPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner.Player), PileType.Draw, Owner.Player, CardPilePosition.Random));
        await PowerCmd.Decrement(this);
    }
}