using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderPattern
{
    public class ComputerFactory
    {
        public Computer Create(string cpu, string ram)
        {
            Computer computer = new Computer()
            {
                CPU = cpu,
                RAM = ram
            };
            computer.HardDrive = "SSD, 128 GB";
            return computer;
        }

        public Computer Create(string cpu, string ram, string hardware)
        {
            Computer computer = new Computer()
            {
                CPU = cpu,
                RAM = ram
            };
            computer.HardDrive = hardware;
            return computer;
        }
    }
}
