using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderPattern
{
    public class ComputerBuilder
    {
        #region attributes
        public string CPU { get; private set; }

        public string RAM { get; private set; }

        public string HardDrive { get; private set; }

        public string Motherboard { get; private set; }

        public string GraphicsCard { get; private set; }

        public string Speaker { get; private set; }

        public string Monitor { get; private set; }

        public string Mouse { get; private set; }

        public string Keyboard { get; private set; }
        #endregion

        public ComputerBuilder (string cpu, string ram)
        {
            CPU = cpu;
            RAM = ram;
        }

        public Computer Build()
        {
            return new Computer(this);
        }

        public ComputerBuilder SetHardware(string hardware)
        {
            HardDrive = hardware;
            return this;
        }

        public ComputerBuilder SetMouse(string mouse)
        {
            Mouse = mouse;
            return this;
        }

        public ComputerBuilder SetDisplay(string display)
        {
            if (string.IsNullOrEmpty(display)) return this;
            Monitor = display;
            return this;
        }
    }
}
