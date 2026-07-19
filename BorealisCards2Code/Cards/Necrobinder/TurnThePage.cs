using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace BorealisCards2.BorealisCards2Code.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class TurnThePage() : BorealisCards2Card(5,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override bool CanBeGeneratedByModifiers => false;
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Soul>(), EnergyHoverTip];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        var cards = PileType.Hand.GetPile(Owner).Cards.ToList();
        var energy = cards.Count;
        foreach (var card in cards)
        {
            var soul = Soul.Create(Owner, 1, CombatState).FirstOrDefault();
            await CardCmd.Transform(card, soul);
        }
        await PlayerCmd.GainEnergy(energy, Owner);
    }
    
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}