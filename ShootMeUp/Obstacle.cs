/******************************************************************************
** PROGRAMME  Obstacle.cs                                                    **
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
** Cette classe initialise des obstacles qui servent de boucliers pour       **
** protéger le joueur des tirs ennemis.                                      **
******************************************************************************/
using System;

namespace ShootMeUp
{
    public class Obstacles
    {
        private string _obstacleForm = "▄▄▄"; // Forme visuelle de l'obstacle
        private int _posX;                   // Position X de l'obstacle
        private int _posY;                   // Position Y de l'obstacle
        private int _health;                 // Points de vie de l'obstacle

        // Constructeur de l'obstacle avec position et vie initiale
        public Obstacles(int posX, int posY, int health = 5)
        {
            _posX = posX;
            _posY = posY;
            _health = health;
        }

        // Propriétés pour accéder et modifier les attributs de l'obstacle
        public string obstacleForm
        {
            get { return _obstacleForm; }
            set { _obstacleForm = value; }
        }

        public int health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int PosX
        {
            get { return _posX; }
            set { _posX = value; }
        }

        public int PosY
        {
            get { return _posY; }
            set { _posY = value; }
        }

        /// <summary>
        /// Affiche l'obstacle à sa position actuelle dans la console
        /// </summary>
        public void ShowObstacle()
        {
            Console.SetCursorPosition(_posX, _posY);
            Console.Write(_obstacleForm);
        }

        /// <summary>
        /// Efface l'obstacle de l'affichage en console
        /// </summary>
        public void HideObstacle()
        {
            Console.SetCursorPosition(_posX, _posY);
            Console.Write(new string(' ', _obstacleForm.Length));
        }

        /// <summary>
        /// Gère les dégâts reçus par l'obstacle et le détruit si la vie atteint 0
        /// </summary>
        public void TakeDamage()
        {
            _health--;

            if (_health <= 0)
            {
                DestroyObstacle();
            }
        }

        /// <summary>
        /// Supprime l'obstacle de l'affichage lorsqu'il est détruit
        /// </summary>
        private void DestroyObstacle()
        {
            HideObstacle();
        }
    }
}
