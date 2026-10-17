using ArtRoller;
using BaseLib.Utils;
using BorealisCards2.BorealisCards2Code.Powers.Defect;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace BorealisCards2.BorealisCards2Code.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class PatternScreamer() : BorealisCards2Card(0,
    CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private PatternScreamerEffects _normalEffect;
    private PatternScreamerSpecialEffects _specialEffect;
    
    private enum PatternScreamerEffects
    {
        None,
        Lightning,
        Frost,
        Dark,
        Glass,
        Plasma,
        Block,
        Draw,
        Energy,
        Poison,
        Weak,
        Vulnerable,
        Power,
        ZeroCost,
        Dazes,
        TempFocus,
        Focus,
        Strength,
        Dexterity,
        Plating,
        Wish,
        Hologram,
        Exhaust,
        Special
    }
    
    private enum PatternScreamerSpecialEffects
    {
        None,
        Details,
        Reroll,
        Jackpot,
        Kaboom,
        Claw,
        Yaoi,
        Phone,
        EightOption
    }
    
    private PatternScreamerEffects NormalEffect
    {
        get => _normalEffect;
        set
        {
            AssertMutable();
            _normalEffect = value;
        }
    }
    
    private PatternScreamerSpecialEffects SpecialEffect
    {
        get => _specialEffect;
        set
        {
            AssertMutable();
            _specialEffect = value;
        }
    }

    public override bool GainsBlock => NormalEffect == PatternScreamerEffects.Block;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        switch (NormalEffect)
        {
            case PatternScreamerEffects.None:
                break;
            case PatternScreamerEffects.Lightning:
                await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
                break;
            case PatternScreamerEffects.Frost:
                await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
                break;
            case PatternScreamerEffects.Dark:
                await OrbCmd.Channel<DarkOrb>(choiceContext, Owner);
                break;
            case PatternScreamerEffects.Glass:
                await OrbCmd.Channel<GlassOrb>(choiceContext, Owner);
                break;
            case PatternScreamerEffects.Plasma:
                await OrbCmd.Channel<PlasmaOrb>(choiceContext, Owner);
                break;
            case PatternScreamerEffects.Block:
                await CreatureCmd.GainBlock(Owner.Creature, 5M, ValueProp.Move, play);
                break;
            case PatternScreamerEffects.Draw:
                await CardPileCmd.Draw(choiceContext, 2M, Owner);
                break;
            case PatternScreamerEffects.Energy:
                await PlayerCmd.GainEnergy(1M, Owner);
                break;
            case PatternScreamerEffects.Poison:
                await PowerCmd.Apply<PoisonPower>(choiceContext, CombatState.HittableEnemies, 5M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Weak:
                await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, 1M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Vulnerable:
                await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies, 1M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Power:
                var powerCard = CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.Type == CardType.Power), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
                if (powerCard == null)
                    return;
                powerCard.SetToFreeThisTurn();
                await CardPileCmd.AddGeneratedCardToCombat(powerCard, PileType.Hand, Owner);
                break;
            case PatternScreamerEffects.ZeroCost:
                var card = CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !c.EnergyCost.CostsX), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
                if (card == null)
                    return;
                card.SetToFreeThisTurn();
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
                break;
            case PatternScreamerEffects.Dazes:
                await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner), PileType.Hand, Owner);
                await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner), PileType.Hand, Owner);
                await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Dazed>(Owner), PileType.Hand, Owner);
                break;
            case PatternScreamerEffects.TempFocus:
                await PowerCmd.Apply<PatternScreamerPower>(choiceContext, Owner.Creature, 3M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Focus:
                await PowerCmd.Apply<FocusPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Strength:
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Dexterity:
                await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Plating:
                await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, 3M, Owner.Creature, this);
                break;
            case PatternScreamerEffects.Wish:
                var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
                var wishCard = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner), Owner, prefs)).FirstOrDefault();
                if (wishCard == null)
                    return;
                await CardPileCmd.Add(wishCard, PileType.Hand);
                break;
            case PatternScreamerEffects.Hologram:
                var prefs2 = new CardSelectorPrefs(SelectionScreenPrompt, 1);
                var holoCard = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Discard.GetPile(Owner), Owner, prefs2)).FirstOrDefault();
                if (holoCard == null)
                    return;
                await CardPileCmd.Add(holoCard, PileType.Hand);
                break;
            case PatternScreamerEffects.Exhaust:
                var prefs3 = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
                var exhaustCard = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs3, null, this)).FirstOrDefault();
                if (exhaustCard == null)
                    return;
                await CardCmd.Exhaust(choiceContext, exhaustCard);
                break;
            case PatternScreamerEffects.Special:
                switch (SpecialEffect)
                {
                    case PatternScreamerSpecialEffects.None:
                    case PatternScreamerSpecialEffects.Details:
                    case PatternScreamerSpecialEffects.Reroll:
                        break;
                    case PatternScreamerSpecialEffects.Jackpot:
                        await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
                        await OrbCmd.Channel<FrostOrb>(choiceContext, Owner);
                        await OrbCmd.Channel<DarkOrb>(choiceContext, Owner);
                        await OrbCmd.Channel<GlassOrb>(choiceContext, Owner);
                        await OrbCmd.Channel<PlasmaOrb>(choiceContext, Owner);
                        break;
                    case PatternScreamerSpecialEffects.Kaboom:
                        await CreatureCmd.Damage(choiceContext, Owner.Creature.CombatState.Creatures.Where(c => !c.IsPet), 10M, ValueProp.Unpowered, Owner.Creature, null, null);
                        break;
                    case PatternScreamerSpecialEffects.Claw:
                        var list = PileType.Hand.GetPile(Owner).Cards.Where(c => c is not Claw).ToList();
        
                        foreach (var handCard in list)
                        {
                            var claw = CombatState.CreateCard<Claw>(Owner);
                            if (IsUpgraded)
                            {
                                CardCmd.Upgrade(claw);
                            }
                            await CardCmd.Transform(handCard, claw);
                        }
                        break;
                    case PatternScreamerSpecialEffects.Yaoi:
                        await PlayerCmd.GainStars(1, Owner);
                        await ForgeCmd.Forge(10M, Owner, this);
                        break;
                    case PatternScreamerSpecialEffects.Phone:
                        await OstyCmd.Summon(choiceContext, Owner, 5M, this);
                        break;
                    case PatternScreamerSpecialEffects.EightOption:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public override async Task AfterCardPlayedLate(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        if (cardPlay.Player != Owner || cardPlay.Card.Type != CardType.Attack || cardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.All) != 0 || cardPlay.Card.EnergyCost.CostsX || Pile?.Type == PileType.Hand)
            return;
        var num = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.HappenedThisTurn(CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.EnergyCost.GetWithModifiers(CostModifiers.All) == 0 && !e.CardPlay.Card.EnergyCost.CostsX && e.CardPlay.Player == Owner);
        BorealisCards2Main.Logger.Info("------------------------------------------------------------------ "+ num);
        if (num == 1)
        {
            RerollRng();
            await CardPileCmd.Add(this, PileType.Hand);
        }
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        if (SpecialEffect != PatternScreamerSpecialEffects.Reroll && NormalEffect != PatternScreamerEffects.Special)
            return base.GetResultLocationForCardPlay();
        
        var locationForCardPlay = base.GetResultLocationForCardPlay();
        if (locationForCardPlay.pileType == PileType.Discard)
        {
            locationForCardPlay.pileType = PileType.Hand;
            RerollRng();
        }
        return locationForCardPlay;
    }
    
    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState?.TurnNumber > 1)
            return Task.CompletedTask;
        RerollRng();
        return Task.CompletedTask;
    }

    private void RerollRng()
    {
        var rng = Owner.RunState.Rng.CombatCardSelection.NextInt(0, (int) PatternScreamerEffects.Special) + 1;
        NormalEffect = (PatternScreamerEffects) rng;
        if (rng == (int)PatternScreamerEffects.Special)
            SpecialEffect = (PatternScreamerSpecialEffects)Owner.RunState.Rng.CombatCardSelection.NextInt(0, 8) + 1;
        else
            SpecialEffect = PatternScreamerSpecialEffects.None;
    } 
    
    protected override void AddExtraArgsToDescription(LocString description)
    {
        description.Add("HasEffect", NormalEffect != 0);
        foreach (var effect in Enum.GetValues<PatternScreamerEffects>())
            description.Add(effect.ToString(), NormalEffect == effect);
        foreach (var effect in Enum.GetValues<PatternScreamerSpecialEffects>())
            description.Add(effect.ToString(), SpecialEffect == effect);
    }
    
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => GetEffectHoverTips();
    
    private IHoverTip[] GetEffectHoverTips()
    {
        switch (NormalEffect)
        {
            case PatternScreamerEffects.None:
            case PatternScreamerEffects.Block:
            case PatternScreamerEffects.Draw:
            case PatternScreamerEffects.Power:
            case PatternScreamerEffects.ZeroCost:
            case PatternScreamerEffects.Wish:
            case PatternScreamerEffects.Hologram:
            case PatternScreamerEffects.Special:
                break;
            case PatternScreamerEffects.Lightning:
                return [HoverTipFactory.FromOrb<LightningOrb>()];
            case PatternScreamerEffects.Frost:
                return [HoverTipFactory.FromOrb<FrostOrb>()];
            case PatternScreamerEffects.Dark:
                return [HoverTipFactory.FromOrb<DarkOrb>()];
            case PatternScreamerEffects.Glass:
                return [HoverTipFactory.FromOrb<GlassOrb>()];
            case PatternScreamerEffects.Plasma:
                return [HoverTipFactory.FromOrb<PlasmaOrb>()];
            case PatternScreamerEffects.Energy:
                return [EnergyHoverTip];
            case PatternScreamerEffects.Poison:
                return [HoverTipFactory.FromPower<PoisonPower>()];
            case PatternScreamerEffects.Weak:
                return [HoverTipFactory.FromPower<WeakPower>()];
            case PatternScreamerEffects.Vulnerable:
                return [HoverTipFactory.FromPower<VulnerablePower>()];
            case PatternScreamerEffects.Dazes:
                return [HoverTipFactory.FromCard<Dazed>()];
            case PatternScreamerEffects.TempFocus:
            case PatternScreamerEffects.Focus:
                return [HoverTipFactory.FromPower<FocusPower>()];
            case PatternScreamerEffects.Strength:
                return [HoverTipFactory.FromPower<StrengthPower>()];
            case PatternScreamerEffects.Dexterity:
                return [HoverTipFactory.FromPower<DexterityPower>()];
            case PatternScreamerEffects.Plating:
                return [HoverTipFactory.FromPower<PlatingPower>()];
            case PatternScreamerEffects.Exhaust:
                return [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (SpecialEffect)
        {
            case PatternScreamerSpecialEffects.None:
            case PatternScreamerSpecialEffects.Details:
            case PatternScreamerSpecialEffects.Reroll:
            case PatternScreamerSpecialEffects.Kaboom:
            case PatternScreamerSpecialEffects.Yaoi:
            case PatternScreamerSpecialEffects.Phone:
                break;
            case PatternScreamerSpecialEffects.Jackpot:
                return [HoverTipFactory.FromOrb<LightningOrb>(),HoverTipFactory.FromOrb<FrostOrb>(),HoverTipFactory.FromOrb<DarkOrb>(),HoverTipFactory.FromOrb<GlassOrb>(),HoverTipFactory.FromOrb<PlasmaOrb>()];
            case PatternScreamerSpecialEffects.Claw:
                return [HoverTipFactory.FromCard<Claw>()];
            case PatternScreamerSpecialEffects.EightOption:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return [];
    }
}