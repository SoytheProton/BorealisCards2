using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public sealed class AlphaTesting() : BorealisCards2Card(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(10M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        for (var i = 0; i < (IsUpgraded ? 2 : 1); i++)
        {
            foreach (var orb in Owner.PlayerCombatState.OrbQueue.Orbs)
            {
                await OrbCmd.Passive(choiceContext, orb, null);
                await Cmd.Wait(0.10f);
            }
        }
    }
}