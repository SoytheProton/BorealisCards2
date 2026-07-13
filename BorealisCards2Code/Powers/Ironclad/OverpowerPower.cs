using BaseLib.Abstracts;
using BorealisCards2.BorealisCards2Code.Cards.Ironclad;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace BorealisCards2.BorealisCards2Code.Powers.Ironclad;

public sealed class OverpowerPower : TemporaryStrengthPower, ICustomPower
{
    protected override bool IsPositive => false;
    public override AbstractModel OriginModel => ModelDb.Card<Overpower>();
}