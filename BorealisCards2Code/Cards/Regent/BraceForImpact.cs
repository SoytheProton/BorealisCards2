using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class BraceForImpact() : BorealisCards2Card(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    private const string IncreaseKey = "Increase";
    private int _currentCards = BaseCards;
    private int _increasedCards;
    private const int BaseCards = 2;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(40, ValueProp.Move), new CardsVar(CurrentCards), new (IncreaseKey, 2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Debris>()];
    
    [SavedProperty]
    private int CurrentCards
    {
        get => _currentCards;
        set
        {
            AssertMutable();
            _currentCards = value;
            DynamicVars.Cards.BaseValue = _currentCards;
        }
    }
    
    [SavedProperty]
    private int IncreasedCards
    {
        get => _increasedCards;
        set
        {
            AssertMutable();
            _increasedCards = value;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        for(var i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Debris>(Owner), PileType.Discard, Owner));
        }
        var intValue = DynamicVars[IncreaseKey].IntValue;
        BuffFromPlay(intValue);
        if (DeckVersion is not BraceForImpact deckVersion)
            return;
        deckVersion.BuffFromPlay(intValue);
        await Cmd.Wait(0.5f);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(20M);
    }
    
    protected override void AfterDowngraded() => UpdateCards();

    private void BuffFromPlay(int extraCards)
    {
        IncreasedCards += extraCards;
        UpdateCards();
    }

    private void UpdateCards() => CurrentCards = BaseCards + IncreasedCards;
}