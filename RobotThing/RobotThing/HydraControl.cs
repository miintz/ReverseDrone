using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotThing
{
    internal class HydraControl
    {
        public static bool leftDocked;
        public static bool rightDocked;

        [StructLayout(LayoutKind.Sequential)]
        public struct sixenseControllerData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] pos;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public float[] rot_mat;
            public float joystick_x;
            public float joystick_y;
            public float trigger;
            public uint buttons;
            public byte sequence_number;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] rot_quat;
            public ushort firmware_revision;
            public ushort hardware_revision;
            public ushort packet_type;
            public ushort magnetic_frequency;
            public int enabled;
            public int controller_index;
            public byte is_docked;
            public byte which_hand;
            public byte hemi_tracking_enabled;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct sixenseAllControllerData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public sixenseControllerData[] controllers;
        }

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseInit")]
        public static extern int sixenseInit();

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseExit")]
        public static extern int sixenseExit();

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetMaxBases")]
        public static extern int sixenseGetMaxBases();

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseSetActiveBase")]
        public static extern int sixenseSetActiveBase(int i);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseIsBaseConnected")]
        public static extern int sixenseIsBaseConnected(int i);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetMaxControllers")]
        public static extern int sixenseGetMaxControllers();

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseIsControllerEnabled")]
        public static extern int sixenseIsControllerEnabled(int which);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetNumActiveControllers")]
        public static extern int sixenseGetNumActiveControllers();

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetHistorySize")]
        public static extern int sixenseGetHistorySize();

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetData")]
        public static extern int sixenseGetData(int which, int index_back, ref sixenseControllerData cd);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetAllData")]
        public static extern int sixenseGetAllData(int index_back, ref sixenseAllControllerData acd);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetNewestData")]
        public static extern int sixenseGetNewestData(int which, ref sixenseControllerData cd);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetAllNewestData")]
        public static extern int sixenseGetAllNewestData(ref sixenseAllControllerData acd);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseSetHemisphereTrackingMode")]
        public static extern int sixenseSetHemisphereTrackingMode(int which_controller, int state);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetHemisphereTrackingMode")]
        public static extern int sixenseGetHemisphereTrackingMode(int which_controller, ref int state);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseAutoEnableHemisphereTracking")]
        public static extern int sixenseAutoEnableHemisphereTracking(int which_controller);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseSetHighPriorityBindingEnabled")]
        public static extern int sixenseSetHighPriorityBindingEnabled(int on_or_off);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetHighPriorityBindingEnabled")]
        public static extern int sixenseGetHighPriorityBindingEnabled(ref int on_or_off);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseTriggerVibration")]
        public static extern int sixenseTriggerVibration(int controller_id, int duration_100ms, int pattern_id);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseSetFilterEnabled")]
        public static extern int sixenseSetFilterEnabled(int on_or_off);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetFilterEnabled")]
        public static extern int sixenseGetFilterEnabled(ref int on_or_off);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseSetFilterParams")]
        public static extern int sixenseSetFilterParams(float near_range, float near_val, float far_range, float far_val);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetFilterParams")]
        public static extern int sixenseGetFilterParams(ref float near_range, ref float near_val, ref float far_range, ref float far_val);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseSetBaseColor")]
        public static extern int sixenseSetBaseColor(byte red, byte green, byte blue);

        [DllImport("lib/sixense.dll", EntryPoint = "sixenseGetBaseColor")]
        public static extern int sixenseGetBaseColor(ref byte red, ref byte green, ref byte blue);
    }
}
