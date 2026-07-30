using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Powers.Defect;

public sealed class LiquidCoolingFocusPower : TemporaryFocusPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Power<LiquidCoolingPower>();
}