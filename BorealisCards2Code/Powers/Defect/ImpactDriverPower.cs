using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class ImpactDriverPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override object InitInternalData() => new Data();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Power)
            return Task.CompletedTask;
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var energy) || energy <= 0)
            return;
        Flash();
        VfxCmd.PlayOnCreatureCenters(CombatState.HittableEnemies, "vfx/vfx_attack_slash");
        SfxCmd.Play("slash_attack.mp3");
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, Amount, ValueProp.Unpowered, Owner);
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }
}