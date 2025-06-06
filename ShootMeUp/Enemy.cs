/******************************************************************************
** PROGRAMME  Enemy.cs                                                       **
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
** Cette classe initialise les ennemis pour qu'ils apparaissent dans le jeu, **
** se déplacent et attaquent.                                                **
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShootMeUp;
using static ShootMeUp.Game;

namespace ShootMeUp
{
    public class Enemy
    {
        // Attributs de l'ennemi
        private string _enemyForm;                      // Forme visuelle de l'ennemi
        private int _hpEnemy;                           // Points de vie de l'ennemi
        private int _enemyPosX;                         // Position X de l'ennemi
        private int _enemyPosY;                         // Position Y de l'ennemi
        private int _nbScore;                           // Valeur en points que l'ennemi vaut
        private bool _enemyShootState = false;          // État de tir de l'ennemi
        private static Random random = new Random();    // Générateur aléatoire pour les tirs
        private int shootCooldown = 300;                // Délai de tir en millisecondes
        private int cooldownCounter = 0;                // Compteur pour le délai de tir

        public Bullet _enemyBullet; // Instance d'une balle pour les tirs de l'ennemi

        // Propriétés pour accéder aux attributs de l'ennemi
        public string enemyForm
        {
            get { return _enemyForm; }
            set { _enemyForm = value; }
        }

        public int hpEnemy
        {
            get { return _hpEnemy; }
            set { _hpEnemy = value; }
        }

        public int enemyPosX
        {
            get { return _enemyPosX; }
            set { _enemyPosX = value; }
        }

        public int enemyPosY
        {
            get { return _enemyPosY; }
            set { _enemyPosY = value; }
        }

        public int nbScore
        {
            get { return _nbScore; }
            set { _nbScore = value; }
        }

        // Constructeur de l'ennemi, initialisant ses attributs
        public Enemy(string enemyForm, int hpEnemy, int enemyPosX, int enemyPosY, int nbScore)
        {
            _enemyForm = enemyForm;
            _hpEnemy = hpEnemy;
            _enemyPosX = enemyPosX;
            _enemyPosY = enemyPosY;
            _nbScore = nbScore;
        }

        /// <summary>
        /// Affiche l'ennemi à sa position actuelle
        /// </summary>
        public void ShowEnemy()
        {
            Console.SetCursorPosition(_enemyPosX, _enemyPosY);
            Console.WriteLine(_enemyForm);
        }

        /// <summary>
        /// Cache l'ennemi en effaçant sa forme de l'affichage
        /// </summary>
        public void HideEnemy()
        {
            Console.SetCursorPosition(_enemyPosX, _enemyPosY);
            Console.Write(new string(' ', _enemyForm.Length));
        }

        /// <summary>
        /// Déplace l'ennemi dans la direction spécifiée
        /// </summary>
        /// <param name="direction">0 pour droite, 1 pour bas, 2 pour gauche</param>
        public void EnemyMove(int direction)
        {
            HideEnemy();

            if (direction == 0) _enemyPosX++;           // Bouge à droite
            else if (direction == 1) _enemyPosY++;      // Bouge en bas
            else if (direction == 2) _enemyPosX--;      // Bouge à gauche

            ShowEnemy();
        }

        /// <summary>
        /// Crée une balle pour l'ennemi si aucune n'existe
        /// </summary>
        public void EnemyShoot()
        {
            if (_enemyBullet == null)
            {
                _enemyShootState = true;
                _enemyBullet = new Bullet(_enemyPosX + 2, _enemyPosY + 1);
            }
        }

        /// <summary>
        /// Met à jour la position de la balle tirée par l'ennemi
        /// </summary>
        /// <returns>True si la balle est encore dans la zone de jeu, False si elle est sortie</returns>
        public bool UpdateBullet()
        {
            _enemyBullet.Hide();

            if (_enemyBullet.bulletPosY > 0)
            {
                _enemyBullet.bulletPosY++;
                _enemyBullet.Show();
                Thread.Sleep(30);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gère l'état de la balle de l'ennemi, la détruit si elle sort de la zone
        /// </summary>
        public void UpdateEnemyBullet()
        {
            if (_enemyBullet != null)
            {
                bool isInBounds = _enemyBullet.MoveDown();

                if (!isInBounds)
                {
                    _enemyBullet = null;
                    _enemyShootState = false;
                }
            }
        }

        /// <summary>
        /// Met à jour l'ennemi, gère le tir automatique avec un délai de cooldown
        /// </summary>
        public void Update()
        {
            if (cooldownCounter >= shootCooldown)
            {
                int shootChance = random.Next(0, 450);

                if (shootChance < 1 && _enemyBullet == null)
                {
                    EnemyShoot();
                }

                cooldownCounter = 0;
            }
            else
            {
                cooldownCounter++;
            }

            UpdateEnemyBullet();
        }

        /// <summary>
        /// Réinitialise la balle de l'ennemi
        /// </summary>
        public void ResetEnemyBullet()
        {
            _enemyBullet = null;
            _enemyShootState = false;
        }
    }
}

