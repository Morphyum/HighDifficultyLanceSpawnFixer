using BattleTech.Data;
using BattleTech.Framework;
using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HighDifficultyLanceSpawnFixer
{
    public class HighDifficultyLanceSpawnFixer {
        internal static string ModDirectory;
        public static void Init(string directory, string settingsJSON) {
            ModDirectory = directory;
            var harmony = HarmonyInstance.Create("de.morphyum.LanceSpawnFixer");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void LogLine(string line) {
            string filePath = $"{ModDirectory}/Log.txt";
            (new FileInfo(filePath)).Directory.Create();
            using (StreamWriter writer = new StreamWriter(filePath, true)) {
                writer.WriteLine(line + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }

    [HarmonyPatch(typeof(LanceOverride))]
    [HarmonyPatch("SelectLanceDefFromList")]
    public static class SimGameState_CreateMechArmorModifyWorkOrder_Patch {
        public static void Prefix(ref int ___MAX_DIFF_DIVERGENCE) {
            ___MAX_DIFF_DIVERGENCE = 50;
        }

        public static void Postfix(LanceDef_MDD __result, int requestedDifficulty, int ___MAX_DIFF_DIVERGENCE) {
            if (__result == null) {
                HighDifficultyLanceSpawnFixer.LogLine("No Lance found");
                HighDifficultyLanceSpawnFixer.LogLine("Diff: " + requestedDifficulty);
                HighDifficultyLanceSpawnFixer.LogLine("Conver: " + ___MAX_DIFF_DIVERGENCE);
            }
        }
    }
}
