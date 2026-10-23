using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Defect;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class CatastrophicFailure() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(5)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // Thank you to pikcube for the code. 
        if (CombatState is null)
        {
            return;
        }

        foreach (var p in CombatState.Players.Where(p => p != Owner && p.Creature.IsAlive && CombatManager.Instance.IsPartOfPlayerTurn(p)))
        {
            BranchingPlayerChoiceContext bpcc = new(this, LocalContext.NetId ?? 0, GameActionType.CombatPlayPhaseOnly, choiceContext);
            await bpcc.AssignTaskAndWaitForPauseOrCompletion(Seek(bpcc, p, SelectionScreenPrompt));
        }

        await Seek(choiceContext, Owner, SelectionScreenPrompt);

        var statusPool = ModelDb.CardPool<StatusCardPool>()
            .GetUnlockedCards(
                Owner.UnlockState,
                Owner.RunState.CardMultiplayerConstraint);
        
        var status = CardFactory.GetForCombat(
            Owner,
            statusPool,
            DynamicVars.Repeat.IntValue,
            Owner.RunState.Rng.CombatCardGeneration);

        var cardModels = status.ToList();
        if (cardModels.Count != 0)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(cardModels, PileType.Discard, Owner));
        }
    }

    private static async Task Seek(PlayerChoiceContext choiceContext, Player target, LocString selectionScreenPrompt)
    {
        await CardPileCmd.ShuffleIfNecessary(choiceContext, target);
        var card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(target), target,
            new CardSelectorPrefs(selectionScreenPrompt, 1), null)).FirstOrDefault();
        if (card != null)
        {
            await CardPileCmd.Add(card, PileType.Hand);
        }
    }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}