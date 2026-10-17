using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class ColorWheel() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FocusPower>(3M), new BoolVar("ModdedOrbs", Pile is { IsCombatPile: true } && ModdedOrbsDetection())];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FocusPower>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
    }

    private static bool ModdedOrbsDetection() => ModelDb.Orbs.Any(orb => orb.GetType().Assembly != typeof(OrbModel).Assembly);

    protected override void OnUpgrade()
    {
        DynamicVars.Power<FocusPower>().UpgradeValueBy(2M);
    }
}