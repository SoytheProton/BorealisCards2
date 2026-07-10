using BorealisCards2.BorealisCards2Code.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public abstract class TemporaryFocusNextTurnPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    protected virtual PowerModel TemporaryFocusPower => ModelDb.Power<TemporaryFocusPower>();
    protected abstract AbstractModel OriginModel { get; }


    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || AmountOnTurnStart == 0)
            return;
        await PowerCmd.Apply(choiceContext, TemporaryFocusPower.ToMutable(), Owner, Amount, Owner, null);
        await PowerCmd.Remove(this);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            var items = new List<IHoverTip>();
            var hoverTipList = items;
            IEnumerable<IHoverTip> collection;
            switch (OriginModel)
            {
                case CardModel card:
                    collection = [HoverTipFactory.FromCard(card)];
                    break;
                case PotionModel model:
                    collection = [HoverTipFactory.FromPotion(model)];
                    break;
                case RelicModel relic:
                    collection = HoverTipFactory.FromRelic(relic);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            hoverTipList.AddRange(collection);
            items.Add(HoverTipFactory.FromPower<FocusPower>());
            return items;
        }
    }
    
    public override string CustomPackedIconPath => "temporary_focus_next_turn_power.png".PowerImagePath();

    public override string CustomBigIconPath => "temporary_focus_next_turn_power.png".BigPowerImagePath();

    
    public override LocString Title => new("powers", "BOREALISCARDS2-TEMPORARY_FOCUS_NEXT_TURN_POWER.title");

    public override LocString Description => new("powers", "BOREALISCARDS2-TEMPORARY_FOCUS_NEXT_TURN_POWER.description");

    protected override string SmartDescriptionLocKey => "BOREALISCARDS2-TEMPORARY_FOCUS_NEXT_TURN_POWER.smartDescription";
}