using BaseLib.Extensions;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Cards.Defect.ColorWheelCards;
using BorealisCards2.BorealisCards2Code.Powers.Defect;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class ColorWheel() : BorealisCards2Card(1,
    CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private static readonly List<CardModel> ColorWheelCards = [ModelDb.Card<ColorWheelLightning>(), ModelDb.Card<ColorWheelFrost>(), ModelDb.Card<ColorWheelDark>(), ModelDb.Card<ColorWheelGlass>(), ModelDb.Card<ColorWheelPlasma>()];

    private ModelId? _chosenOrb;

    [SavedProperty]
    private ModelId? ChosenOrb
    {
        get => _chosenOrb;
        set
        {
            AssertMutable();
            _chosenOrb = value;
            if(value == null)
                return;
            ((StringVar)DynamicVars["ChosenOrb"]).StringValue = ModelDb.GetById<OrbModel>(value).Title.GetFormattedText();
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<FocusPower>(3M), new StringVar("ChosenOrb")];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => GetOrbHoverTips();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (ChosenOrb != null)
        {
            var orb = ModelDb.GetById<OrbModel>(ChosenOrb).ToMutable();
            await OrbCmd.Channel(choiceContext, orb, Owner);
        }
        await PowerCmd.Apply<ColorWheelPower>(choiceContext, Owner.Creature, DynamicVars.Power<FocusPower>().BaseValue, Owner.Creature, this);
    }
    
    public override async Task AfterCardChangedPiles(
        CardModel card,
        PileType oldPileType,
        AbstractModel? clonedBy)
    {
        if(card != this || ChosenOrb != null)
            return;
        var cardModel = (await CardSelectCmd.FromSimpleGrid(new BlockingPlayerChoiceContext(), [.. ColorWheelCards.Select(c => Owner.RunState.CreateCard(c, Owner))], Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1))).FirstOrDefault();
        if (cardModel == null)
            return;
        ChosenOrb = ((IColorWheelChoosable) cardModel).OnChosen().Id;
    }

    private static bool ModdedOrbsDetection() => ModelDb.Orbs.Any(orb => orb.GetType().Assembly != typeof(OrbModel).Assembly);

    protected override void OnUpgrade()
    {
        DynamicVars.Power<FocusPower>().UpgradeValueBy(2M);
    }
    
    protected override void AddExtraArgsToDescription(LocString description)
    {
        description.Add("HasChosen", ChosenOrb != null);
        description.Add("Modded", Pile is { IsCombatPile: true } && ModdedOrbsDetection());
    }
    
    private IHoverTip[] GetOrbHoverTips()
    {
        List<IHoverTip> hoverTips = [HoverTipFactory.FromPower<FocusPower>()];
        if (ChosenOrb == null)
            return [.. hoverTips];
        hoverTips.Add(ModelDb.GetById<OrbModel>(ChosenOrb).DumbHoverTip);
        return [.. hoverTips];
    }
    
    public interface IColorWheelChoosable
    {
        OrbModel OnChosen();
    }
}