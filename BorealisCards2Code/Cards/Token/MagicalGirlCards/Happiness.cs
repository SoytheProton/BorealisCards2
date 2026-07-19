using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Regent;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Cards.Token.MagicalGirlCards;

[Pool(typeof(TokenCardPool))]
public sealed class Happiness() : PowerOfFriendship.MagicalGirlCard(1,
    CardType.Power, CardRarity.Token,
    TargetType.Self)
{
    public override bool CanBeGeneratedByModifiers => false;
    public override int CanonicalStarCost => 2;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Power", 1M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        if (IsUpgraded) await CardPileCmd.Draw(choiceContext, Owner);
        await PowerCmd.Apply<HappinessPower>(choiceContext, Owner.Creature, DynamicVars["Power"].BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}