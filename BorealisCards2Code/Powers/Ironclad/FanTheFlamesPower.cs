using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Powers.Ironclad;

public sealed class FanTheFlamesPower : BorealisCards2Power, IHasSecondAmount
{
    private int _secondAmount;

    private int SecondAmount
    {
        get => _secondAmount;
        set
        {
            AssertMutable();
            _secondAmount = value;
            DynamicVars.Damage.BaseValue = value;
            this.InvokeSecondAmountChanged();
        }
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(0M, ValueProp.Unpowered)];

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy || !participants.Contains(Owner))
            return;
        Flash();
        NFireBurningVfx.Create(Owner, 0.75f, false);
        await Cmd.CustomScaledWait(0.2f, 0.4f);
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, SecondAmount, ValueProp.Unpowered, Owner);
    }

    public override Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        if (card.Owner.Creature != Owner)
            return Task.CompletedTask;
        SecondAmount += Amount;
        return Task.CompletedTask;
    }
    
    public string GetSecondAmount()
    {
        return SecondAmount.ToString();
    }
}