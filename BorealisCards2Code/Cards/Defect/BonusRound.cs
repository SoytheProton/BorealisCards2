using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class BonusRound() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new RepeatVar(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        for (var i = 0; i < DynamicVars.Repeat.IntValue; ++i)
            await OrbCmd.Channel(choiceContext, OrbModel.GetRandomOrb(Owner.RunState.Rng.CombatOrbGeneration).ToMutable(), Owner);
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        var locationForCardPlay = base.GetResultLocationForCardPlay();
        if (CombatState == null)
            return locationForCardPlay;
        var list = CombatState.GetTeammatesOf(Owner.Creature).Where(c => c is { IsAlive: true, IsPlayer: true } && c.Player != Owner).ToList();
        if (list.Count == 0)
            return locationForCardPlay;
        locationForCardPlay.player = Owner.RunState.Rng.CombatTargets.NextItem(list).Player;
        if (locationForCardPlay.pileType == PileType.Discard)
        {
            locationForCardPlay.pileType = PileType.Draw;
            locationForCardPlay.position = CardPilePosition.Random;
        }
        return locationForCardPlay;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}