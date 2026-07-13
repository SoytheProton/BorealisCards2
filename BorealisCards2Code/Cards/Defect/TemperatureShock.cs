using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Orbs;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public sealed class TemperatureShock() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{ 
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Channeling), HoverTipFactory.FromOrb<LightningOrb>(), HoverTipFactory.FromOrb<FrostOrb>()];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);
}