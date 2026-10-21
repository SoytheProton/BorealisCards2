using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Defect;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public sealed class Vignette() : BorealisCards2Card(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips {
        get
        {
            List<IHoverTip> hoverTips = [];
            if(IsUpgraded)
                hoverTips.Add(HoverTipFactory.Static(StaticHoverTip.Channeling));
            hoverTips.Add(HoverTipFactory.FromOrb<DarkOrb>());
            return [.. hoverTips];
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
            await OrbCmd.Channel<DarkOrb>(new ThrowingPlayerChoiceContext(), Owner);
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<VignettePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}