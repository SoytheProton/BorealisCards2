using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Powers.Ironclad;

public sealed class BerserkerPower : BorealisCards2Power
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0 || Owner.CombatState.CurrentSide != Owner.Side)
            return;
        for (int i = 0; i < Amount; i++)
        {
            CardModel card2 = PileType.Draw.GetPile(Owner.Player).Cards.Where( c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(Owner.Player.RunState.Rng.Shuffle).FirstOrDefault() ?? PileType.Draw.GetPile(Owner.Player).Cards.ToList().StableShuffle(Owner.Player.RunState.Rng.Shuffle).FirstOrDefault();
            if (card2 == null)
                return;
            await CardCmd.AutoPlay(choiceContext, card2, null);   
        }
    }
}