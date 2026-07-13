using BaseLib.Abstracts;
using BorealisCards2.BorealisCards2Code.Cards.Defect;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class LiquidCoolingFocusPower : TemporaryFocusPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<PowerUp>();
}