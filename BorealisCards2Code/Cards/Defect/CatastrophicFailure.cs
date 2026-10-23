using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Defect;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
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
        /*List<Task> taskList =
        [
            .. from player in CombatState.GetTeammatesOf(Owner.Creature).Select(creature => creature.Player)
                .OfType<Player>()
            where !player.Creature.IsDead && CombatManager.Instance.IsPartOfPlayerTurn(player)
            select Seek(choiceContext, player)
        ];
        await Task.WhenAll(taskList);*/

        var players = CombatState.GetTeammatesOf(Owner.Creature).Select(creature => creature)
            .Where(c => c.IsAlive && CombatManager.Instance.IsPartOfPlayerTurn(c.Player));

        await PowerCmd.Apply<SeekPower>(choiceContext, players, 1, Owner.Creature, this);
        
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

    private async Task Seek(PlayerChoiceContext choiceContext, Player target)
    {
        var card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(target), target,
            new CardSelectorPrefs(SelectionScreenPrompt, 1), null)).FirstOrDefault();
        if (card == null)
            return;
        await CardPileCmd.Add(card, PileType.Hand);
    }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
}