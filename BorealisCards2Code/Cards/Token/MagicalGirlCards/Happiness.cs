using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Cards.Token.MagicalGirlCards;

[Pool(typeof(TokenCardPool))]
public sealed class Happiness() : BorealisCards2Card(0,
    CardType.Power, CardRarity.Token,
    TargetType.Self)
{
    public override bool CanBeGeneratedByModifiers => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Power", 1M)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["Power"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars["Power"].BaseValue,
            Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars["Power"].UpgradeValueBy(1M);
}