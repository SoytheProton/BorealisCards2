using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Token.MagicalGirlCards;

[Pool(typeof(TokenCardPool))]
public sealed class LoveStrike() : PowerOfFriendship.MagicalGirlCard(1,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    public override bool CanBeGeneratedByModifiers => false;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(9M), new ExtraDamageVar(2M), new CalculatedDamageVar(ValueProp.Move).WithMultiplier((model, _) => model.Owner.PlayerCombatState.Stars)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Forge)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(3M);
    }
}