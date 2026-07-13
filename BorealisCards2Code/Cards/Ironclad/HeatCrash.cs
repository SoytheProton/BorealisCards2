using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Ironclad;

[Pool(typeof(IroncladCardPool))]
public class HeatCrash() : BorealisCards2Card(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(13M, ValueProp.Move), new CardsVar(3)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithAttackerAnim(MegaCrit.Sts2.Core.Models.Characters.Ironclad.GetHeavyAnimIfApplicable(Owner.Character), MegaCrit.Sts2.Core.Models.Characters.Ironclad.GetHeavyAttackDelayIfApplicable(Owner.Character)).
            WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").
            WithHitVfxSpawnedAtBase().Execute(choiceContext);
        var cards = await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        foreach (var card in cards)
        {
            if (card.Type == CardType.Attack && card.CostsEnergyOrStars(true))
            {
                card.EnergyCost.SetUntilPlayed(0);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4M);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}