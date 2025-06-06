using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootMeUp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu(); //instancier un menu

            Console.CursorVisible = false;  //enlever le curseur

            menu.GameMenu();    //afficher le menu

            Console.ReadLine(); //pour que le programme s'affiche
        }
    }
}
