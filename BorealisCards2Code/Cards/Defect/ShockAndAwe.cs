using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Defect;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public sealed class ShockAndAwe() : BorealisCards2Card(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ShockAndAwePower>(1M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<LightningOrb>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<ShockAndAwePower>(choiceContext, Owner.Creature, DynamicVars["ShockAndAwePower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Ethereal);
}