using BorealisCards2.BorealisCards2Code.Cards.Defect;
using MegaCrit.Sts2.Core.Models;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public class NextTurnPowerUpPower : TemporaryFocusNextTurnPower
{
    protected override AbstractModel OriginModel => ModelDb.Card<PowerUp>();
    protected override PowerModel TemporaryFocusPower => ModelDb.Power<PowerUpPower>();
}