using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using FrooxEngine.LogiX;
using BaseX;
using System.Collections.Generic;

namespace NeosCustomNodeBrowser
{
    public class NeosCustomNodeBrowser : NeosMod
    {
        public override string Name => "NeosCustomNodeBrowser";
        public override string Author => "kka429";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/rassi0429/NeosCustomNodeBrowser"; // this line is optional and can be omitted

        public static Slot configSlot;

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("dev.kokoa.neosCustomNodeBrowser");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(LogixTip))]
        [HarmonyPatch("ToggleNodeSelector")]
        class ShowBrowser
        {
            static bool Prefix(LogixTip __instance, IButton button, ButtonEventData eventData)
            {
                if (__instance.World.InputInterface.GetKey(Key.LeftShift)) return true;
                __instance.ActiveTool?.CloseContextMenu();
                List<string> import = new List<string>();
                //TODO read config
                import.Add("neosdb:///faa62d07754f9b3a76f9f477747d6342d2a35f8e67954e03b6ce3ae2d9f44315.7zbson"); ;
                Slot slot = __instance.World.AddSlot("Batch import");
                slot.PersistentSelf = false;
                slot.PositionInFrontOfUser(new float3?(float3.Backward));
                BatchFolderImporter.BatchImport(slot, (IEnumerable<string>)import, __instance.World.InputInterface.GetKey(Key.LeftShift));
                return false;
            }
        }

        [HarmonyPatch(typeof(Userspace), "OnAttach")]
        class AddConfig
        {
            static void Postfix(Userspace __instance)
            {
                // TODO read/store config
                configSlot = __instance.World.RootSlot.AddSlot("CustomNodeBrowser Config", false);
                var config = configSlot.AttachComponent<ValueField<string>>();
            }
        }
    }
}