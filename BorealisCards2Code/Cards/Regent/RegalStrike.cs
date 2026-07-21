using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class RegalStrike() : BorealisCards2Card(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6M, ValueProp.Move), new BlockVar(10M, ValueProp.Move)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, prefs)).FirstOrDefault();
        var flag2 = card != null;
        if (flag2)
        {
            flag2 = card.Pile?.Type switch
            {
                PileType.Draw or PileType.Discard => true,
                _ => false
            };
        }
        if (flag2)
        {
            await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
    }
}