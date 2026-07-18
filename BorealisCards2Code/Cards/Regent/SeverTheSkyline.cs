using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards.Token;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class SeverTheSkyline() : BorealisCards2Card(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ForgeVar(11), new StarsVar(3), new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Forge), HoverTipFactory.FromCard<MinionDefend>(IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await ForgeCmd.Forge(DynamicVars.Forge.IntValue, Owner, this);
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, (int)DynamicVars.Cards._baseValue);
        var original = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).ToList();
        if (original == null)
            return;
        foreach (var item in original)
        {
            CardModel card = CombatState.CreateCard<MinionDiveBomb>(Owner);
            if (IsUpgraded)
                CardCmd.Upgrade(card);
            await CardCmd.Transform(item, card);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Forge.UpgradeValueBy(4M);
        DynamicVars.Stars.UpgradeValueBy(1M);
    }
}