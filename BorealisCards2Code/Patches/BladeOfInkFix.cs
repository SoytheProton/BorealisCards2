using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace BorealisCards2.BorealisCards2Code.Patches;

public class BladeOfInkFix
{
    [HarmonyPatch(typeof(BladeOfInk), "OnPlay", MethodType.Async)]
    public class CanEnchantPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codeMatcher = new CodeMatcher(instructions);
            
            var canEnchant = AccessTools.Method(typeof(CanEnchantPatch), nameof(CanEnchantInky));
            var label = generator.DefineLabel();

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Stloc_S)
                )
                .ThrowIfInvalid("Couldn't find get_Current for BladeOfInkFix")
                .Advance(3)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldloc_S, (byte)6),
                    new CodeInstruction(OpCodes.Call, canEnchant),
                    new CodeInstruction(OpCodes.Brfalse_S, label)
                );

            codeMatcher.MatchStartForward(
                    new CodeMatch(OpCodes.Pop))
                .ThrowIfInvalid("Couldn't find pop for BladeOfInkFix")
                .Advance()
                .AddLabels([label]);

            return codeMatcher.InstructionEnumeration();
        }

        private static bool CanEnchantInky(CardModel card)
        {
            return ModelDb.Enchantment<Inky>().CanEnchant(card);
        }
    }
}