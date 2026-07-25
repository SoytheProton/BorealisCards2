using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards.Token;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class ProtectMe() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<MinionDefend>(IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        for (var i = 0; i < DynamicVars.Cards.BaseValue; i++)
        {
            var card = Owner.RunState.Rng.CombatCardSelection.NextItem(PileType.Hand.GetPile(Owner).Cards);
            var minion = CombatState.CreateCard<MinionDefend>(Owner);
            if(IsUpgraded)
                CardCmd.Upgrade(minion);
            await CardCmd.Transform(card, minion);
        }
    }
}