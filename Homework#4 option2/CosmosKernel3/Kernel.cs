using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.Hardware;

namespace CosmosKernel3
{
    public class Kernel : Sys.Kernel
    {
        public static uint old_x = 0;
        public static uint old_y = 0;
        public static VGAScreen VScreen = new VGAScreen();
        public static Mouse m = new Cosmos.Hardware.Mouse();
        protected override void BeforeRun()
        {



            Console.WriteLine("Vga Driver Booting");
            VScreen.SetGraphicsMode(VGAScreen.ScreenSize.Size320x200, VGAScreen.ColorDepth.BitDepth8);
            VScreen.Clear(0);
            Console.WriteLine("Vga Driver Booted");

            m.Initialize(320, 200);
        }

        public static void DrawMouse()
        {

            if (old_x == 0 && old_y == 0)
            {
                Do_DrawMouse();
            }

            if (old_x != m.X || old_y != m.Y)
            {
                //Clear the old mouse draw location.
                VScreen.Clear(0);
                //Draw a new one
                Do_DrawMouse();
                //Updtae the last draw mouse position to the new position
                old_x = (uint)m.X;
                old_y = (uint)m.Y;
            }

        }

        public static void Do_DrawMouse()
        {
            VScreen.SetPixel320x200x8((uint)m.X, (uint)m.Y, 63);
            VScreen.SetPixel320x200x8((uint)m.X + 1, (uint)m.Y, 63);
            VScreen.SetPixel320x200x8((uint)m.X + 2, (uint)m.Y, 63);
            VScreen.SetPixel320x200x8((uint)m.X, (uint)m.Y + 1, 63);
            VScreen.SetPixel320x200x8((uint)m.X, (uint)m.Y + 2, 63);
            VScreen.SetPixel320x200x8((uint)m.X + 1, (uint)m.Y + 1, 63);
            VScreen.SetPixel320x200x8((uint)m.X + 2, (uint)m.Y + 2, 63);
            VScreen.SetPixel320x200x8((uint)m.X + 3, (uint)m.Y + 3, 63);
        }


        protected override void Run()
        {
            DrawMouse();
        }
    }
}