using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyKeyChangerForDebug.AppSettingData;

namespace MyKeyChangerForDebug {
    class KeymappingHandler {

        #region Declaration - Dll
        private static class NativeMethods {
            // callback 
            public delegate IntPtr KeyboardGlobalHookCallback(int code, uint msg, ref KBDLLHOOKSTRUCT hookData);

            // https://msdn.microsoft.com/ja-jp/library/cc430103.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr SetWindowsHookEx(int idHook, KeyboardGlobalHookCallback lpfn, IntPtr hMod, uint dwThreadId);

            // https://msdn.microsoft.com/ja-jp/library/cc429591.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, uint msg, ref KBDLLHOOKSTRUCT hookData);

            // https://msdn.microsoft.com/ja-jp/library/cc430120.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool UnhookWindowsHookEx(IntPtr hhk);

            // https://msdn.microsoft.com/ja-jp/library/cc364822.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        private static IntPtr _keyEventHandle;
        private static event NativeMethods.KeyboardGlobalHookCallback _hookCallback;
        #endregion

        #region Declaration
        private static class Const {
            public const int Action = 0;            // 0のみフックするのがお約束らしい
            public const int HookTypeLL = 13;       // WH_KEYBOARD_LL
        }

        private static class KeyStroke {
            public const int KeyDown = 0x100;
            public const int KeyUp = 0x101;
            public const int SysKeyDown = 0x104;
            public const int SysKeyup = 0x105;
        }
        private static class Flags {
            public const uint None = 0x16;
            public const uint KeyDown = 0x00;
            public const uint KeyUp = 0x02;
            public const uint ExtendeKey = 0x01;     //  拡張コード(これを設定することで修飾キーも有効になる)
            public const uint Unicode = 0x04;
            public const uint ScanCode = 0x08;
        }
        private static class ExtraInfo {
            public const int SendKey = 1;
            public const int LLKHF_EXTENDED = 0x00000001;
        }

        private class KeyData {
            public byte[] KeySet;
            public uint Flag;
            public KeyData(byte[] keySet, uint flag) {
                this.KeySet = keySet;
                this.Flag = flag;
            }
        }
        
        /// <summary>
        ///  key set of index
        /// </summary>
        private static class KeySetIndex {
            public const int VirtualKey = 0;
            public const int ScanCode = 1;
        }

        // Android Studio
        private static Dictionary<ushort, List<KeyData>> _mappingForAndroidStudioTemplate = new Dictionary<ushort, List<KeyData>> {
            { KeySet.TenKeyNum7[KeySetIndex.ScanCode], new List<KeyData>() {             // Step Into(F7)
                { new KeyData(KeySet.F7, Flags.None)},
            }},
            { KeySet.TenKeyNum8[KeySetIndex.ScanCode], new List<KeyData>() {             // Step Over(F8)
                { new KeyData(KeySet.F8, Flags.None)},
            }},
            { KeySet.TenKeyNum9[KeySetIndex.ScanCode], new List<KeyData>() {             // Step Out(Shift F8)
                { new KeyData(KeySet.ShiftL, Flags.None)},
                { new KeyData(KeySet.F8, Flags.None)},
            }},
            { KeySet.TenKeySubtract[KeySetIndex.ScanCode], new List<KeyData>() {         // Resume Program(F9)
                { new KeyData(KeySet.F9, Flags.None)},
            }},
            { KeySet.TenKeyNum4[KeySetIndex.ScanCode], new List<KeyData>() {             // Go to Declaration(Ctonrol B)
                { new KeyData(KeySet.ControlL, Flags.None)},
                { new KeyData(KeySet.B, Flags.None)},
            }},
            { KeySet.TenKeyNum5[KeySetIndex.ScanCode], new List<KeyData>() {             // Find Usages(Alt F7)
                { new KeyData(KeySet.AltL, Flags.None)},
                { new KeyData(KeySet.F7, Flags.None)},
            }},
            { KeySet.TenKeyNum6[KeySetIndex.ScanCode], new List<KeyData>() {             // Back(Control Alt ←)
                { new KeyData(KeySet.ControlL, Flags.None)},
                { new KeyData(KeySet.AltL, Flags.None)},
                { new KeyData(KeySet.Left, Flags.None)},
            }},
            { KeySet.TenKeyAdd[KeySetIndex.ScanCode], new List<KeyData>() {             // BreakPoint(Control F8)
                { new KeyData(KeySet.ControlL, Flags.None)},
                { new KeyData(KeySet.F8, Flags.None)},
            }},
            { KeySet.TenKeyNum1[KeySetIndex.ScanCode], new List<KeyData>() {             // Run app(Shift F10)
                { new KeyData(KeySet.ShiftL, Flags.None)},
                { new KeyData(KeySet.F10, Flags.None)},
            }},
            { KeySet.TenKeyNum2[KeySetIndex.ScanCode], new List<KeyData>() {             // Stop(Control F2)
                { new KeyData(KeySet.ControlL, Flags.None)},
                { new KeyData(KeySet.F2, Flags.None)},
            }},
            { KeySet.TenKeyNum3[KeySetIndex.ScanCode], new List<KeyData>() {             // Make Project(Control F9)
                { new KeyData(KeySet.ControlL, Flags.None)},
                { new KeyData(KeySet.F9, Flags.None)},
            }},
        };

        // Visual Studio
        private static Dictionary<ushort, List<KeyData>> _mappingForVisualStudioTemplate = new Dictionary<ushort, List<KeyData>> {
            { KeySet.TenKeyNum7[KeySetIndex.ScanCode], new List<KeyData>() {             // Step Into(F11)
                { new KeyData(KeySet.F11, Flags.None)},
            }},
            { KeySet.TenKeyNum8[KeySetIndex.ScanCode], new List<KeyData>() {             // Step Over(F10)
                { new KeyData(KeySet.F10, Flags.None)},
            }},
            { KeySet.TenKeyNum9[KeySetIndex.ScanCode], new List<KeyData>() {             // Step Out(Shift F11)
                { new KeyData(KeySet.ShiftL, Flags.None)},
                { new KeyData(KeySet.F11, Flags.None)},
            }},
            { KeySet.TenKeySubtract[KeySetIndex.ScanCode], new List<KeyData>() {         // Resume Program(F5)
                { new KeyData(KeySet.F5, Flags.None)},
            }},
            { KeySet.TenKeyNum4[KeySetIndex.ScanCode], new List<KeyData>() {             // Go to Declaration(Control F12)
//                { new KeyData(KeySet.ControlL, Flags.None)},
                { new KeyData(KeySet.F12, Flags.None)},
            }},
            { KeySet.TenKeyNum5[KeySetIndex.ScanCode], new List<KeyData>() {             // All Preferences(Shift F12)
                { new KeyData(KeySet.ShiftL, Flags.None)},
                { new KeyData(KeySet.F12, Flags.None)},
            }},
            { KeySet.TenKeyNum6[KeySetIndex.ScanCode], new List<KeyData>() {             // Back(Ctrl + -)
                { new KeyData(KeySet.ControlL, Flags.KeyDown)},
                { new KeyData(KeySet.SemiColon, Flags.KeyDown)},
                { new KeyData(KeySet.SemiColon, Flags.KeyUp)},
                { new KeyData(KeySet.Minus, Flags.KeyDown)},
                { new KeyData(KeySet.Minus, Flags.KeyUp)},
                { new KeyData(KeySet.ControlL, Flags.KeyUp)},
            }},
            { KeySet.TenKeyAdd[KeySetIndex.ScanCode], new List<KeyData>() {             // BreakPoint(F9)
                { new KeyData(KeySet.F9, Flags.None)},
            }},
            { KeySet.TenKeyNum1[KeySetIndex.ScanCode], new List<KeyData>() {             // Run app(F5)
                { new KeyData(KeySet.F5, Flags.None)},
            }},
            { KeySet.TenKeyNum2[KeySetIndex.ScanCode], new List<KeyData>() {             // Stop(F5)
                { new KeyData(KeySet.ShiftL, Flags.None)},
                { new KeyData(KeySet.F5, Flags.None)},
            }},
            { KeySet.TenKeyNum3[KeySetIndex.ScanCode], new List<KeyData>() {             // Build(Ctrl Shift F5)
                { new KeyData(KeySet.ControlL, Flags.None)},
                { new KeyData(KeySet.ShiftL, Flags.None)},
                { new KeyData(KeySet.B, Flags.None)},
            }},
        };

        private static Dictionary<ushort, List<KeyData>> _currentMapping;
        #endregion

        #region Constructor

        #endregion

        #region Public Property
        public static bool IsHooking {
            get;
            private set;
        }
        #endregion

        #region Public Method
        /// <summary>
        ///  start global hook.
        /// </summary>
        public static void Start(MappingMode mode) {
            if (IsHooking) {
                return;
            }

            ChangeMapping(mode);
            IsHooking = true;

            _hookCallback = HookProcedure;
            IntPtr hinst = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(KeymappingHandler).Assembly.GetModules()[0]);

            _keyEventHandle = NativeMethods.SetWindowsHookEx(Const.HookTypeLL, _hookCallback, hinst, 0);
            if (_keyEventHandle == IntPtr.Zero) {
                IsHooking = false;
                throw new System.ComponentModel.Win32Exception();
            }
        }

        /// <summary>
        /// stop global hook.
        /// </summary>
        public static void Stop() {
            if (!IsHooking) {
                return;
            }

            if (_keyEventHandle != IntPtr.Zero) {
                IsHooking = false;
                NativeMethods.UnhookWindowsHookEx(_keyEventHandle);
                _keyEventHandle = IntPtr.Zero;
                _hookCallback -= HookProcedure;
            }
        }

        /// <summary>
        /// change mapping mode
        /// </summary>
        /// <param name="mode"></param>
        public static void ChangeMapping(MappingMode mode) {
            switch (mode) {
                case MappingMode.Android:
                    SetupCurrentMapping(_mappingForAndroidStudioTemplate);
                    break;
                case MappingMode.VisualStudio:
                    SetupCurrentMapping(_mappingForVisualStudioTemplate);
                    break;
                default:
                    return;
            }
        }
        #endregion

        #region Privater Method
        /// <summary>
        /// hook
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="hookData"></param>
        /// <returns></returns>
        private static IntPtr HookProcedure(int code, uint msg, ref KBDLLHOOKSTRUCT hookData) {
            ushort scanCode = (ushort)hookData.scanCode;

            if (Const.Action != code || (IntPtr)ExtraInfo.SendKey == hookData.dwExtraInfo) {
                goto ExitProc;
            }
            // 例えば←とテンキーの4はスキャンコードが同一。テンキーは拡張フラグがオフなので、
            // それ前提の判断とする。
            if ((hookData.flags & ExtraInfo.LLKHF_EXTENDED) == ExtraInfo.LLKHF_EXTENDED) {
                goto ExitProc;
            }

            if (_currentMapping.ContainsKey(scanCode)) {
                if (KeyStroke.KeyDown == msg) {
                    System.Diagnostics.Debug.Print("dwExtraInfo:" + hookData.dwExtraInfo);
                    System.Diagnostics.Debug.Print("flags:" + hookData.flags);
                    System.Diagnostics.Debug.Print("flags:" + hookData.vkCode);


                    SendKey(_currentMapping[scanCode]);
                }
                return (IntPtr)1;
            } else if (scanCode == KeySet.TenKeyNum0[KeySetIndex.ScanCode]) {
                if (KeyStroke.KeyUp == msg) {
                    var refrence = new KeyMappingReference();
                    refrence.ShowDialog();
                }
                return (IntPtr)1;
            }
            ExitProc:
            return NativeMethods.CallNextHookEx(_keyEventHandle, code, msg, ref hookData);
        }

        /// <summary>
        /// set up key mapping list
        /// </summary>
        /// <param name="template"></param>
        private static void SetupCurrentMapping(Dictionary<ushort, List<KeyData>> template) {
            _currentMapping = new Dictionary<ushort, List<KeyData>>(template);

            var keys = new List<ushort>(_currentMapping.Keys);
            foreach (var key in keys) {
                var keyList = _currentMapping[key];
                if (keyList[0].Flag != Flags.None) {
                    continue;
                }
                foreach (var keyset in keyList) {
                    keyset.Flag = Flags.KeyDown;
                }
                for (int i = keyList.Count - 1; 0 <= i; i--) {
                    keyList.Add(new KeyData(keyList[i].KeySet, Flags.KeyUp));
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyList"></param>
        private static void SendKey(List<KeyData> keyList) {
            foreach (var key in keyList) {
                NativeMethods.keybd_event(
                    key.KeySet[KeySetIndex.VirtualKey], key.KeySet[KeySetIndex.ScanCode], key.Flag, (UIntPtr)ExtraInfo.SendKey);
            }
        }
        #endregion
    }
}
