using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderPattern
{
    public class Computer
    {
        #region required
        public string CPU;
        public string RAM;
        #endregion

        #region optional
        public string HardDrive;
        public string Motherboard;
        public string GraphicsCard;
        public string Speaker;
        public string Monitor;
        public string Mouse;
        public string Keyboard;
        #endregion

        #region constructors
        public Computer() { }

        public Computer(string cpu, string ram)
        {
            CPU = cpu;
            RAM = ram;
        }

        public Computer(string cpu, string ram, string hardDrive)
        {
            CPU = cpu;
            RAM = ram;
            HardDrive = hardDrive;
        }

        public Computer(ComputerBuilder builder)
        {
            CPU = builder.CPU;
            RAM = builder.RAM;
            HardDrive = builder.HardDrive;
            Motherboard = builder.Motherboard;
            Monitor = builder.Monitor;
            // ...
        }
        #endregion
    }
}
