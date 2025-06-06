/******************************************************************************
** PROGRAMME  Bullet.cs                                                      **
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
** Cette classe crée une balle, essentielle pour le jeu, permettant au joueur**
** et aux ennemis de tirer, ajoutant ainsi du challenge.                     **
******************************************************************************/
using System;
using System.Threading;

namespace ShootMeUp
{
    public class Bullet
    {
        private string _bullet = "▄";              // Forme visuelle de la balle
        private int _bulletPosX;                   // Position X de la balle
        private int _bulletPosY;                   // Position Y de la balle
        private static int moveInterval = 30;      // Intervalle de mouvement de la balle (en ms)
        private DateTime lastMoveTime;             // Dernier moment où la balle s'est déplacée

        // Constructeur de la balle, initialisant sa position
        public Bullet(int bulletPosX, int bulletPosY)
        {
            _bulletPosX = bulletPosX;
            _bulletPosY = bulletPosY;
        }

        // Propriétés pour obtenir ou définir la position X de la balle
        public int bulletPosX
        {
            get { return _bulletPosX; }
            set { _bulletPosX = value; }
        }

        // Propriétés pour obtenir ou définir la position Y de la balle
        public int bulletPosY
        {
            get { return _bulletPosY; }
            set { _bulletPosY = value; }
        }

        /// <summary>
        /// Affiche la balle à sa position actuelle
        /// </summary>
        public void Show()
        {
            Console.SetCursorPosition(_bulletPosX, _bulletPosY); // Positionne le curseur
            Console.Write(_bullet); // Affiche la balle
        }

        /// <summary>
        /// Cache la balle en remplaçant son symbole par un espace
        /// </summary>
        public void Hide()
        {
            Console.SetCursorPosition(_bulletPosX, _bulletPosY);
            Console.Write(" "); // Remplace la balle par un espace
        }

        /// <summary>
        /// Met à jour la position de la balle en la déplaçant vers le haut
        /// </summary>
        /// <returns>True si la balle est toujours visible, False si elle est sortie du champ de jeu</returns>
        public bool UpdateBullet()
        {
            Hide();

            if (_bulletPosY > 0) // Si la balle est toujours dans la zone jouable
            {
                _bulletPosY--; // Déplace la balle vers le haut
                Show(); // Affiche la balle à la nouvelle position
                Thread.Sleep(30); // Pause de 30 ms
                return true; // La balle existe toujours
            }
            else
            {
                return false; // La balle a atteint le bord et "meurt"
            }
        }

        /// <summary>
        /// Déplace la balle vers le bas si le délai est écoulé depuis le dernier mouvement
        /// </summary>
        /// <returns>True si la balle est encore visible dans la console, False si elle dépasse le bas de l'écran</returns>
        public bool MoveDown()
        {
            if ((DateTime.Now - lastMoveTime).TotalMilliseconds >= moveInterval)
            {
                Hide();
                _bulletPosY++; // Incrémente la position Y
                lastMoveTime = DateTime.Now; // Met à jour le dernier mouvement

                if (_bulletPosY < Console.WindowHeight) // Vérifie si la balle est toujours dans la fenêtre
                {
                    Show(); // Affiche la balle à la nouvelle position
                    return true;
                }
                else
                {
                    return false; // La balle a dépassé le bas de la console
                }
            }
            return true; // Retourne true si le délai est écoulé
        }
    }
}
