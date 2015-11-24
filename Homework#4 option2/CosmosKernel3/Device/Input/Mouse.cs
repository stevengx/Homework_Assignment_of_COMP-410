using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.Core;
using Cosmos.Hardware.Drivers.PCI.Video;
using Cosmos.Hardware;

namespace CosmosKernel1.Devices.Input
{
    public class Mouse
    {
        //Underlying mouse driver instance - PS2 mice only but VMWare simulates PS2 for us.
        private Cosmos.Hardware.Mouse mouse;

        //Mouse X coord.
        public ushort X
        {
            get
            {
                return (ushort)mouse.X;
            }
        }
        //Mouse Y coord.
        public ushort Y
        {
            get
            {
                return (ushort)mouse.Y;
            }
        }

        //Screen width - sets max X coord.
        public uint ScreenWidth
        {
            get
            {
                return mouse.ScreenWidth;
            }
            set
            {
                mouse.ScreenWidth = value;
            }
        }
        //Screen height - sets max Y coord.
        public uint ScreenHeight
        {
            get
            {
                return mouse.ScreenHeight;
            }
            set
            {
                mouse.ScreenHeight = value;
            }
        }

        public Mouse(uint screenWidth, uint screenHeight)
        {
            //Create and initialise the underlying driver
            mouse = new Cosmos.Hardware.Mouse();
            mouse.Initialize(screenWidth, screenHeight);
        }

        //Store the last drwan position and pixel colours of the mouse so they can be reset when the mouse moves (causes max 1 frame lag in colours)
        private int lastDrawX = 0;
        private int lastDrawY = 0;
        public int LastDrawX
        {
            get
            {
                return lastDrawX;
            }
        }
        public int LastDrawY
        {
            get
            {
                return lastDrawY;
            }
        }
        private uint[][] lastDrawPosCols = new uint[20][]
            {
                new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],
                new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],new uint[20],
            };
        //Sets the colours for the mouse - 0 is black and can be used as transparent in draw method.
        //20 x 20 pixels allowed for mouse design.
        private uint[][] mouseCols = new uint[20][]
        {
            new uint[20] { 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x00FFFFFFFF, 0x00FFFFFFFF, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 },
            new uint[20] { 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000011, 0x0000000011, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000, 0x0000000000 }
        };
        //Draws the mouse
        public void Draw(VMWareSVGAII vgaDriver)
        {
            //If the mouse hasn't moved, don't reset.
            if (lastDrawX != X || lastDrawY != Y)
            {
                //Clear the old mouse draw location.
                DoDraw(lastDrawX, lastDrawY, vgaDriver, lastDrawPosCols, true);

                //Store the existing colours where the mouse will now be.
                for (ushort x = 0; x < 20; x++)
                {
                    for (ushort y = 0; y < 20; y++)
                    {
                        lastDrawPosCols[y][x] = vgaDriver.GetPixel((ushort)(X + x), (ushort)(Y + y));
                    }
                }
                
                //Updtae the last draw mouse position to the new position
                lastDrawX = X;
                lastDrawY = Y;
            }
            
            //Draw the mouse in it's current position - ensure it's always on top.
            DoDraw(X, Y, vgaDriver, mouseCols, false);
        }
        //Actually does the draw.
        private void DoDraw(int X, int Y, VMWareSVGAII vgaDriver, uint[][] mouseCol, bool allowBlack)
        {
            for (ushort x = 0; x < 20; x++)
            {
                for (ushort y = 0; y < 20; y++)
                {
                    if (allowBlack || mouseCol[y][x] != 0)
                    {
                        vgaDriver.SetPixel((ushort)(X + x), (ushort)(Y + y), mouseCol[y][x]);
                    }
                }
            }
        }
    }
}
