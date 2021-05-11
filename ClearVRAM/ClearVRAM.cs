using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MelonLoader;
using UnityEngine;
using UIExpansionKit.API;
using VRC;
using Object = UnityEngine.Object;

namespace ClearVRAM
{
    public static class BuildInfo
    {
        public const string Name = "ClearVRAM";
        public const string Author = "Requi";
        public const string Company = "RequiDev";
        public const string Version = "1.0.3";
        public const string DownloadLink = "https://github.com/RequiDev/ClearVRAM";
    }

    public class ClearVRAM : MelonMod
    {
        public override void OnApplicationStart()
        {
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton("Clear VRAM", () =>
            {
                var currentAvatars = (from player in PlayerManager.prop_PlayerManager_0.prop_ArrayOf_Player_0 where player != null select player.prop_ApiAvatar_0 into apiAvatar where apiAvatar != null select apiAvatar.assetUrl).ToList();

                var dict = new Dictionary<string, Il2CppSystem.Object>();
                var abdm = AssetBundleDownloadManager.prop_AssetBundleDownloadManager_0;
                foreach (var key in abdm.field_Private_Dictionary_2_String_Object_0.Keys)
                {
                    dict.Add(key, abdm.field_Private_Dictionary_2_String_Object_0[key]);
                }

                foreach (var key in dict.Keys.Where(key => !abdm.field_Private_Dictionary_2_String_Object_0[key].name.Contains("vrcw") && !currentAvatars.Contains(key)))
                {
                    var avatarObj = abdm.field_Private_Dictionary_2_String_Object_0[key];
                    abdm.field_Private_Dictionary_2_String_AssetBundleDownload_0.Remove(key);
                    abdm.field_Private_Dictionary_2_String_Object_0.Remove(key);
                    Object.Destroy(avatarObj);
                    Resources.UnloadAsset(avatarObj);
                }
                dict.Clear();
            });
        }
    }
}
