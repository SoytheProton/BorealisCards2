using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards;
using BorealisCards2.BorealisCards2Code.Cards.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class ChopShop() : BorealisCards2Card(0,
    CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5M, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        
        var card = Owner.RunState.Rng.CombatCardSelection.NextItem(PileType.Hand.GetPile(Owner).Cards);
        if(card == null)
            return;
        
        var zeroCostPool = Owner.Character.CardPool
            .GetUnlockedCards(
                Owner.UnlockState,
                Owner.RunState.CardMultiplayerConstraint)
            .Where(c =>
                c.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && c.Type == CardType.Attack);

        var zeroCost = CardFactory.GetDistinctForCombat(
                Owner,
                zeroCostPool,
                1,
                Owner.RunState.Rng.CombatCardGeneration)
            .FirstOrDefault();

        if (zeroCost != null)
        {
            ++zeroCost.BaseReplayCount;
            await CardCmd.Transform(card, zeroCost);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3M);
    }
}