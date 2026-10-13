using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards;
using BorealisCards2.BorealisCards2Code.Powers.Defect;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class CarbonCopy() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (Func<CardModel, bool>) (c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 0), this);
        
        CardModel selectedCard = cards.FirstOrDefault<CardModel>();
        if (selectedCard == null)
        {
            cards = (IEnumerable<CardModel>) null;
            selectedCard = (CardModel) null;
        }
        else
        {
            (await PowerCmd.Apply<CarbonCopyPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this)).SetSelectedCard(selectedCard);
            cards = (IEnumerable<CardModel>) null;
            selectedCard = (CardModel) null;
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}