using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Token.MagicalGirlCards;

[Pool(typeof(TokenCardPool))]
public class PowerOfFriendship() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    public override int CanonicalStarCost => 2;
    public override bool CanBeGeneratedByModifiers => false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<LoveStrike>(IsUpgraded),HoverTipFactory.FromCard<JusticeDefend>(IsUpgraded),HoverTipFactory.FromCard<Happiness>(IsUpgraded),HoverTipFactory.FromCard<Courage>(IsUpgraded)];
    
    public override bool GainsBlock => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (var card in PileType.Hand.GetPile(Owner).Cards)
        {
            CardModel replacement = card.Type switch
            {
                CardType.Attack => ModelDb.Card<LoveStrike>(),
                CardType.Skill => ModelDb.Card<JusticeDefend>(),
                CardType.Power => ModelDb.Card<Happiness>(),
                _ => ModelDb.Card<Courage>()
            };
            if(IsUpgraded)
                CardCmd.Upgrade(replacement, CardPreviewStyle.None);
            await CardCmd.Transform(card, replacement);
        }
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3M);
}