using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect.ColorWheelCards;

[Pool(typeof(TokenCardPool))]
public sealed class ColorWheelPlasma() : BorealisCards2Card(-1,
    CardType.Skill, CardRarity.Token, TargetType.None), ColorWheel.IColorWheelChoosable
{
    public override int MaxUpgradeLevel => 0;
    public override bool CanBeGeneratedInCombat => false;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromOrb<PlasmaOrb>()];

    public OrbModel OnChosen()
    {
        return ModelDb.Orb<PlasmaOrb>();
    }
}