using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class MinionScouts() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Token,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StarsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Minion];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
    }

    protected override void OnUpgrade() => DynamicVars.Stars.UpgradeValueBy(1M);
}