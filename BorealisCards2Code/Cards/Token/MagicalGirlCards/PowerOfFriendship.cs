using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace BorealisCards2.BorealisCards2Code.Cards.Token.MagicalGirlCards;

[Pool(typeof(TokenCardPool))]
public class PowerOfFriendship() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    public override int CanonicalStarCost => 2;
    public override bool CanBeGeneratedByModifiers => false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        var cards = PileType.Hand.GetPile(Owner).Cards.ToList();
        foreach (var card in cards)
        {
            CardModel replacement = card.Type switch
            {
                CardType.Attack => CombatState.CreateCard<LoveStrike>(Owner),
                CardType.Skill => CombatState.CreateCard<JusticeDefend>(Owner),
                CardType.Power => CombatState.CreateCard<Happiness>(Owner),
                _ => CombatState.CreateCard<Courage>(Owner)
            };
            if(IsUpgraded)
                CardCmd.Upgrade(replacement, CardPreviewStyle.None);
            var clone = card.CreateClone();
            clone.DeckVersion = card.DeckVersion;
            ((MagicalGirlCard)replacement).OriginalCard = clone;
            await CardCmd.Transform(card, replacement);
        }
    }

    public abstract class MagicalGirlCard(int cost, CardType type, CardRarity rarity, TargetType target) : BorealisCards2Card(cost, type, rarity, target)
    {
        public CardModel OriginalCard;
        
        public override async Task BeforeSideTurnEnd(
            PlayerChoiceContext choiceContext,
            CombatSide side,
            IEnumerable<Creature> participants)
        {
            if (!participants.Contains(Owner.Creature))
                return;
            await CardCmd.Transform(this, OriginalCard, CardPreviewStyle.None);
        }
    }
}