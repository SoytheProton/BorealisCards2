using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;


[Pool(typeof(DefectCardPool))]
public class Weld() : BorealisCards2Card(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, ValueProp.Move)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Burn>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Burn>(Owner), PileType.Hand, Owner);
        await Cmd.Wait(0.5f);
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        var locationForCardPlay = base.GetResultLocationForCardPlay();
        if (locationForCardPlay.pileType == PileType.Discard)
            locationForCardPlay.pileType = PileType.Hand;
        return locationForCardPlay;
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);
}