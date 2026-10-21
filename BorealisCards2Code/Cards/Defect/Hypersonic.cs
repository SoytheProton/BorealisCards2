using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class Hypersonic() : BorealisCards2Card(0,
    CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3M, ValueProp.Move), new CardsVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        if (CombatState == null)
            return;
        var list = CombatState.GetTeammatesOf(Owner.Creature)
            .Where(c => c is { IsAlive: true, IsPlayer: true }).ToList();
        if(list.Count == 0)
            return;
        var playerTarget = Owner.RunState.Rng.CombatTargets.NextItem(list)?.Player;
        if(playerTarget == null)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CreateCloneForPlayer(playerTarget), PileType.Discard, Owner), 2.2f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}