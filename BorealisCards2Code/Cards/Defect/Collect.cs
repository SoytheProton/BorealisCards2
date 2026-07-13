using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public sealed class Collect() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{ 
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, prefs, model => Filter(model))).FirstOrDefault();
        if (card != null)
            await CardPileCmd.Add(card, PileType.Hand);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    private static bool Filter(CardModel card)
    {
        var flag1 = card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !card.EnergyCost.CostsX;
        if (!flag1) return flag1;
        var flag2 = card.Type switch
        {
            CardType.Attack or CardType.Skill or CardType.Power => true,
            _ => false
        };
        flag1 = flag2;
        return flag1;
    }

    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1);
}