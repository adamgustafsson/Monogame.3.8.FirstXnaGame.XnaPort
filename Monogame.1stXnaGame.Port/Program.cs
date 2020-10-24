using System;

namespace Lab2
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Controller.MasterController game = new Controller.MasterController())
            {
                game.Run();
            }
        }
    }

}

