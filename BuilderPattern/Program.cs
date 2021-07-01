using System;

namespace BuilderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Method1();
            Method2();
            Method3();
        }

        /// <summary>
        /// vanilla way to create object by using constructor
        /// overload constructors
        /// </summary>
        private static void Method1()
        {
            Computer computer = new Computer() { CPU = "Intel Core 7", RAM = "8G" };
            Computer computer1 = new Computer("Intel Core 5", "4G");
        }

        /// <summary>
        /// factory design pattern
        /// </summary>
        private static void Method2()
        {
            ComputerFactory factory = new ComputerFactory();
            Computer computer = factory.Create("AMD", "16G");
            Computer computer1 = factory.Create("Apple", "8G", "SSD 128G");
        }

        /// <summary>
        /// builder design pattern
        /// </summary>
        private static void Method3()
        {
            ComputerBuilder builder = new ComputerBuilder("Intel", "4G");
            builder.SetMouse("Logitech Wireless Mouse")
                .SetHardware("SSD 64G")
                .SetDisplay("Double Dell Monitors");
            Computer computer = builder.Build();
        }

    }
}
