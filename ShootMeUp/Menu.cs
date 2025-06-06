/******************************************************************************
** PROGRAMME  Menu.cs                                                        **
**                                                                           **
** Lieu      : ETML - section informatique                                   **
** Auteur    : Thomas Moreira                                                **
** Date      : 02.11.24                                                      **
**                                                                           **
** Modifications                                                             **
**   Auteur  :                                                               **
**   Version :                                                               **
**   Date    :                                                               **
**   Raisons :                                                               **
**                                                                           **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Cette classe crée un menu permettant de choisir entre plusieurs options : **
** démarrer le jeu, voir les scores, accéder aux options ou aux informations **
** à propos du jeu.                                                          **
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShootMeUp
{
    public class Menu
    {
        // Instance de la classe Game pour gérer le jeu
        private Game game;

        // Titre ASCII du menu principal
        private readonly string _Titre = @"
                         ____ ____ ____ ____ ____ _________ ____ ____ ____ ____ ____ ____ ____ 
                        ||S |||p |||a |||c |||e |||       |||I |||n |||v |||a |||d |||e |||r ||
                        ||__|||__|||__|||__|||__|||_______|||__|||__|||__|||__|||__|||__|||__||
                        |/__\|/__\|/__\|/__\|/__\|/_______\|/__\|/__\|/__\|/__\|/__\|/__\|/__\|";

        private int _selectedOption; // Option actuellement sélectionnée dans le menu

        // Liste des options du menu
        private string[] _Options = new string[] { "Jouer", "Options", "A propos", "Highscores", "Exit" };

        // Constructeur par défaut du menu
        public Menu() { }

        /// <summary>
        /// Affiche le titre du jeu en couleur
        /// </summary>
        public void ShowTitle()
        {
            Console.WriteLine(_Titre, Console.ForegroundColor = ConsoleColor.DarkRed);
            Console.ResetColor();
        }

        /// <summary>
        /// Affiche les options du menu et permet de naviguer entre elles
        /// </summary>
        public void menuOptions()
        {
            string chosenOption;
            string leftSide;
            string rightSide;
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
            int PositionY = Console.CursorTop;

            // Affichage de chaque option, avec un indicateur pour celle sélectionnée
            for (int i = 0; i < _Options.Length; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth / 2) - (_Options.Length + 3), PositionY++);
                chosenOption = _Options[i];
                if (_selectedOption == i)
                {
                    leftSide = " >> ";
                    rightSide = " << ";
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                else
                {
                    leftSide = " ";
                    rightSide = "         ";
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine($"{leftSide}{chosenOption}{rightSide}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Permet de naviguer entre les options avec les flèches du clavier
        /// </summary>
        /// <returns>L'index de l'option choisie</returns>
        public int Select()
        {
            ConsoleKeyInfo pressedKey;

            do
            {
                pressedKey = Console.ReadKey(true);

                if (pressedKey.Key == ConsoleKey.DownArrow)
                {
                    _selectedOption++;
                    if (_selectedOption == _Options.Length)
                    {
                        _selectedOption = 0;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.UpArrow)
                {
                    _selectedOption--;
                    if (_selectedOption < 0)
                    {
                        _selectedOption = _Options.Length - 1;
                    }
                }
                menuOptions();
            }
            while (pressedKey.Key != ConsoleKey.Enter);

            return _selectedOption;
        }

        /// <summary>
        /// Menu principal du jeu
        /// </summary>
        public void GameMenu()
        {
            int selectOption;

            ShowTitle();
            menuOptions();

            selectOption = Select();

            switch (_selectedOption)
            {
                case 0:
                    Play();
                    break;
                case 1:
                    Options();
                    break;
                case 2:
                    Apropos();
                    break;
                case 3:
                    Highscores();
                    break;
                case 4:
                    Exit();
                    break;
            }
        }

        /// <summary>
        /// Démarre une nouvelle partie
        /// </summary>
        public void Play()
        {
            Console.Clear();
            Game game = new Game();
            game.GameStart();
        }

        /// <summary>
        /// Affiche la page des options
        /// </summary>
        public void Options()
        {
            Console.Clear();
            Console.WriteLine("Pas d'options pour le moment.\n");
            Console.WriteLine("\nAppuyer sur une touche pour retourner au menu");
            Console.ReadKey();
            Console.Clear();
            GameMenu();
        }

        /// <summary>
        /// Affiche la page À propos
        /// </summary>
        public void Apropos()
        {
            Console.Clear();
            ShowTitle();
            Console.WriteLine("Replica d'un 'space invader', crée pour le P_OO.");
            Console.WriteLine("\nAppuyer sur une touche pour retourner au menu");
            Console.ReadKey();
            Console.Clear();
            GameMenu();
        }

        /// <summary>
        /// Affiche la page des highscores
        /// </summary>
        public void Highscores()
        {
            Console.Clear();

            if (game == null)
            {
                game = new Game();
            }
            game.LoadHighscore();

            Console.WriteLine("=== Highscores ===");
            Console.WriteLine();
            Console.WriteLine($"Highscore: {game.highscore}");
            Console.WriteLine();
            Console.WriteLine("Appuyer sur une touche pour retourner au menu");
            Console.ReadKey();
            Console.Clear();
            GameMenu();
        }

        /// <summary>
        /// Quitte le jeu avec un message d'adieu
        /// </summary>
        public void Exit()
        {
            string goodbyeLogo = @"    
                                 ____ ____ ____ _________ ____ ____ ____ ____ ____ ____ 
                                ||B |||y |||e |||       |||P |||l |||a |||y |||e |||r ||
                                ||__|||__|||__|||_______|||__|||__|||__|||__|||__|||__||
                                |/__\|/__\|/__\|/_______\|/__\|/__\|/__\|/__\|/__\|/__\|";

            string goodbyeText = "Au revoir !";

            Console.Clear();
            Console.WriteLine(goodbyeLogo, Console.ForegroundColor = ConsoleColor.DarkRed);
            Console.ResetColor();
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2 - (_Options.Length - 2));
            Console.WriteLine(goodbyeText);
        }
    }
}
