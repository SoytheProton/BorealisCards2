using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards.Token;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class GetMad() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<MinionDefend>(IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var selection = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, DynamicVars.Cards.IntValue))).ToList();
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (var original in selection)
        {
            await CardCmd.TransformTo<MinionDefend>(original);
        }
    }
}