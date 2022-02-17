﻿using Dalamud.Game;
using System;
using System.Runtime.InteropServices;
using Dalamud.Logging;

namespace Orchestrion
{
    static class BGMAddressResolver
    {
        private static IntPtr _baseAddress;
        private static IntPtr _addRestartId;
        private static IntPtr _getSpecialMode;

        public static void Init(SigScanner sig)
        {
            _baseAddress = sig.GetStaticAddressFromSig("48 8B 05 ?? ?? ?? ?? 48 85 C0 74 37 83 78 08 04", 2);
            _addRestartId = sig.ScanText("48 89 5C 24 ?? 57 48 83 EC 30 48 8B 41 20 48 8D 79 18");
            _getSpecialMode = sig.ScanText("48 89 5C 24 ?? 57 48 83 EC 20 8B 41 10 33 DB");
            PluginLog.Debug($"BGMAddressResolver init: baseaddress at {_baseAddress.ToInt64():X}");
        }
        
        public static IntPtr AddRestartId => _addRestartId;
        public static IntPtr GetSpecialMode => _getSpecialMode;

        public static IntPtr BGMSceneManager
        {
            get
            {
                var baseObject = Marshal.ReadIntPtr(_baseAddress);

                return baseObject;
            }
        }
        
        public static IntPtr BGMSceneList
        {
            get
            {
                var baseObject = Marshal.ReadIntPtr(_baseAddress);

                // I've never seen this happen, but the game checks for it in a number of places
                return baseObject == IntPtr.Zero ? IntPtr.Zero : Marshal.ReadIntPtr(baseObject + 0xC0);
            }
        }
    }
}