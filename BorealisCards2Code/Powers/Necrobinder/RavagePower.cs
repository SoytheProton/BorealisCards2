using BaseLib.Abstracts;
using BorealisCards2.BorealisCards2Code.Cards.Necrobinder;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Powers.Necrobinder;

public sealed class RavagePower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Ravage>();
    protected override bool IsPositive => false;
}