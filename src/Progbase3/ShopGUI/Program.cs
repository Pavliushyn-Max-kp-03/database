using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal.Gui;

namespace ShopGUI
{
    class Program
    {
        static ProcessGUI gui = new ProcessGUI();
        static void Main(string[] args)
        {
            gui.LogIn();
        }
    }
}