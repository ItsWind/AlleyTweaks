using HarmonyLib;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace AlleyTweaks {
    public class SubModule : MBSubModuleBase {
        protected override void OnSubModuleLoad() {
            base.OnSubModuleLoad();

            new Harmony("Windwhistle.AlleyTweaks").PatchAll();
        }
    }
}