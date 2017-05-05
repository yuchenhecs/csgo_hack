using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace csgo
{
    class Program
    {
        /*
         * 00AB06EC
         * 04AD3A64
         * 04F1D16C
         * 04F68D68
         */
        public const string process = "csgo"; // process name
        public static int aLocalPlayer = 0xAB26E8; // address to player address
        public const int oJump = 0xFC + 0x1;
        public const int oFlags = 0x100; // flags offset
        public const int aJump = 0x4F6C854; // address of jump value
        public static int Client;

        public const int rEnemy = 1;
        public const int gEnemy = 0;
        public const int bEnemy = 0;
        public const int aEnemy = 1;
        public const bool rwoEnemy = true;
        public const bool rwuoEnemy = false;

        public const int rTeam = 0;
        public const int gTeam = 0;
        public const int bTeam = 1;
        public const int aTeam = 1;
        public const bool rwoTeam = true;
        public const bool rwuoTeam = false;

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);

        static void Main(string[] args)
        {

            //BHop();
            WallHack();
        }

        static void WallHack()
        {
            VAMemory vam = new VAMemory(process);

            int address;
            int i = 1;
            int Player;
            int MyTeam;
            int EntityList;
            int HisTeam;
            int GlowIndex;
            int GlowObject;
            int calculation;
            int current;

            if (GetModuleAddy())
            {
                while (true)
                {

                    i = 1;

                    do
                    {
                        address = Client + Offsets.oLocalPlayer;
                        Player = vam.ReadInt32((IntPtr)address);

                        address = Player + Offsets.oTeam;
                        MyTeam = vam.ReadInt32((IntPtr)address);
                        //int MyTeam = 2;

                        address = Client + Offsets.oEntityList + (i - 1) * 0x10;
                        EntityList = vam.ReadInt32((IntPtr)address);

                        address = EntityList + Offsets.oTeam;
                        HisTeam = vam.ReadInt32((IntPtr)address);

                        address = EntityList + Offsets.oDormat;
                        if (!vam.ReadBoolean((IntPtr)address))
                        {
                            address = EntityList + Offsets.oGlowIndex;

                            GlowIndex = vam.ReadInt32((IntPtr)address);

                            if (MyTeam == HisTeam)
                            {
                                address = Client + Offsets.oGlowObject;
                                GlowObject = vam.ReadInt32((IntPtr)address);

                                calculation = GlowIndex * 0x38 + 0x4;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, rTeam);

                                calculation = GlowIndex * 0x38 + 0x8;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, gTeam);

                                calculation = GlowIndex * 0x38 + 0xC;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, bTeam);

                                calculation = GlowIndex * 0x38 + 0x10;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, aTeam);

                                calculation = GlowIndex * 0x38 + 0x24;
                                current = GlowObject + calculation;
                                vam.WriteBoolean((IntPtr)current, rwoTeam);

                                calculation = GlowIndex * 0x38 + 0x25;
                                current = GlowObject + calculation;
                                vam.WriteBoolean((IntPtr)current, rwuoTeam);
                            }
                            else
                            {
                                address = Client + Offsets.oGlowObject;
                                GlowObject = vam.ReadInt32((IntPtr)address);

                                calculation = GlowIndex * 0x38 + 0x4;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, rEnemy);

                                calculation = GlowIndex * 0x38 + 0x8;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, gEnemy);

                                calculation = GlowIndex * 0x38 + 0xC;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, bEnemy);

                                calculation = GlowIndex * 0x38 + 0x10;
                                current = GlowObject + calculation;
                                vam.WriteFloat((IntPtr)current, aEnemy);

                                calculation = GlowIndex * 0x38 + 0x24;
                                current = GlowObject + calculation;
                                vam.WriteBoolean((IntPtr)current, rwoEnemy);

                                calculation = GlowIndex * 0x38 + 0x25;
                                current = GlowObject + calculation;
                                vam.WriteBoolean((IntPtr)current, rwuoEnemy);
                            }
                        }
                        i++;

                    } while (i < 21);

                    Thread.Sleep(30);
                }
            }

        }

        static void BHop()
        {
            VAMemory vam = new VAMemory(process);

            if (GetModuleAddy())
            {
                int fJump = Client + aJump;
                aLocalPlayer = Client + aLocalPlayer;
                int LocalPlayer = vam.ReadInt32((IntPtr)aLocalPlayer);

                int aFlags = LocalPlayer + oFlags;
                int i = 0;
                while (true)
                {


                    while (GetAsyncKeyState(32) != 0)
                    {


                        int Flags = vam.ReadInt32((IntPtr)aFlags);
                        if (Flags == 257)
                        {
                            vam.WriteInt32((IntPtr)fJump, 5);

                            Thread.Sleep(10);
                            vam.WriteInt32((IntPtr)fJump, 4);
                            Console.Clear();
                            Console.WriteLine("Jumping", Console.ForegroundColor = ConsoleColor.Green);

                            // TODO: CONSOLE
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Standing", Console.ForegroundColor = ConsoleColor.Yellow);

                    Thread.Sleep(10);
                }
            }
        }

        static bool GetModuleAddy()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(process);

                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if (m.ModuleName == "client.dll")
                        {
                            Client = (int)m.BaseAddress;
                            return true;

                        }

                    }

                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public struct GlowStruct
        {
            public float r;
            public float g;
            public float b;
            public float a;
            public bool rwo;
            public bool rwuo;
        }

        public class Offsets
        {
            public static int oLocalPlayer = 0xAB26E8;
            public static int oTeam = 0xF0;
            public static int oEntityList = 0x4AD5C24;
            public static int oDormat = 0xE9;
            public static int oGlowIndex = 0xA320;
            public static int oGlowObject = 0x4FF077C;
        }
    }
}