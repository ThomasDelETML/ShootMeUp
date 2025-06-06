/******************************************************************************
** PROGRAMME  Player.cs                                                      **
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
** Cette classe crée une instance du joueur avec tous les attributs et       **
** méthodes nécessaires pour son fonctionnement.                             **
******************************************************************************/
using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;
using ShootMeUp;

namespace ShootMeUp
{
    public class Player
    {
        private string _Player = "<▄8▄>";         // Forme visuelle du joueur
        private int _hpPlayer;                    // Points de vie du joueur
        private int _PlayerPosX;                  // Position X du joueur
        private int _PlayerPosY;                  // Position Y du joueur
        private bool _shootState = false;         // État de tir du joueur

        private Bullet _bullet;                   // Objet Bullet pour les tirs du joueur

        // Propriété pour accéder à la balle du joueur
        public Bullet Bullet => _bullet;

        // Propriété pour la forme du joueur
        public string player => _Player;

        // Propriété pour accéder/modifier les points de vie du joueur
        public int hpPlayer
        {
            get { return _hpPlayer; }
            set { _hpPlayer = value; }
        }

        // Propriété pour accéder/modifier la position X du joueur
        public int PosX
        {
            get { return _PlayerPosX; }
            set { _PlayerPosX = value; }
        }

        // Propriété pour accéder/modifier la position Y du joueur
        public int PosY
        {
            get { return _PlayerPosY; }
            set { _PlayerPosY = value; }
        }

        // Propriété pour accéder/modifier l'état de tir du joueur
        public bool ShootState
        {
            get { return _shootState; }
            set { _shootState = value; }
        }

        // Constructeur du joueur avec ses attributs de base
        public Player(int vie, int positionX, int positionY)
        {
            _hpPlayer = vie;
            _PlayerPosX = positionX;
            _PlayerPosY = positionY;
        }

        /// <summary>
        /// Affiche le joueur à sa position actuelle
        /// </summary>
        public void Show()
        {
            Console.SetCursorPosition(_PlayerPosX, _PlayerPosY);
            Console.Write(_Player);
        }

        /// <summary>
        /// Efface le joueur de la position actuelle
        /// </summary>
        public void Hide()
        {
            Console.SetCursorPosition(_PlayerPosX, _PlayerPosY);
            Console.Write("     ");
        }

        /// <summary>
        /// Met à jour l'état du joueur, incluant les tirs
        /// </summary>
        public void Update()
        {
            if (_hpPlayer > 0)
            {
                if (_bullet != null)
                {
                    if (!_bullet.UpdateBullet())
                    {
                        _bullet = null;
                    }
                }
                this.Show();
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// Réinitialise la balle du joueur
        /// </summary>
        public void ResetBullet()
        {
            _bullet = null;
            _shootState = false;
        }

        /// <summary>
        /// Met à jour la position du joueur selon la touche pressée
        /// </summary>
        public void UpdatePosition(ConsoleKeyInfo key)
        {
            int playerLength = _Player.Length;

            if (key.Key == ConsoleKey.LeftArrow)
            {
                Hide();
                _PlayerPosX--;
            }

            if (key.Key == ConsoleKey.RightArrow)
            {
                if (_PlayerPosX + playerLength < Console.WindowWidth)
                {
                    Hide();
                    _PlayerPosX++;
                }
            }

            if (key.Key == ConsoleKey.Spacebar)
            {
                _shootState = true;
                if (_bullet == null)
                {
                    _bullet = new Bullet(PosX + 2, PosY - 1);
                }
            }
        }
    }
}

