/******************************************************************************
** PROGRAMME  Game.cs                                                        **
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
** Cette classe initialise un jeu et contient la boucle de jeu principale,   **
** gérant les collisions et toutes les méthodes nécessaires au bon           **
** fonctionnement du jeu.                                                    **
******************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows.Input;
using ShootMeUp;

namespace ShootMeUp
{
    public class Game
    {
        // Texte de fin de jeu
        private string _gameOverText = @"   
                             ____ ____ ____ ____ _________ ____ ____ ____ ____ ____ 
                            ||G |||a |||m |||e |||       |||o |||v |||e |||r |||! ||
                            ||__|||__|||__|||__|||_______|||__|||__|||__|||__|||__||
                            |/__\|/__\|/__\|/__\|/_______\|/__\|/__\|/__\|/__\|/__\|";

        private const int _ENEMY_CHAR = 5;            // Longueur de la forme de l'ennemi
        private const int NB_MAX_ENEMIES = 10;        // Nombre maximum d'ennemis

        // Listes des éléments du jeu
        private List<Enemy> _enemyList = new List<Enemy>();       // Liste des ennemis
        private List<Enemy> _enemiesToRemove = new List<Enemy>(); // Ennemis à supprimer
        private List<Enemy> _deadEnemies = new List<Enemy>();     // Ennemis morts
        private List<Obstacles> _obstaclesList = new List<Obstacles>(); // Liste des obstacles

        // Autres variables de jeu
        private int _direction = 0;                             // Direction du mouvement des ennemis
        private readonly string highscoreFilePath = "highscore.txt"; // Chemin du fichier highscore
        private int _highscore = 0;                             // Highscore actuel
        private int _currentScore = 0;                          // Score actuel

        public Player player; // Instance du joueur

        // Propriété du highscore
        public int highscore
        {
            get { return _highscore; }
            set { _highscore = value; }
        }

        // Largeur de la forme du joueur
        private int width => player.player.Length;

        /// <summary>
        /// Démarre le jeu, en initialisant les éléments nécessaires
        /// </summary>
        public void GameStart()
        {
            LoadHighscore();
            player = new Player(3, (Console.WindowWidth - 1) / 2, 27);
            player.Show();
            EnemyGenerator();
            ObstacleGenerator();

            DateTime lastEnemyMoveTime = DateTime.Now;
            int enemyMoveDiff = 200;

            while (true)
            {
                player.Show();
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    player.UpdatePosition(key);
                }
                player.Update();

                if ((DateTime.Now - lastEnemyMoveTime).TotalMilliseconds >= enemyMoveDiff)
                {
                    EnemyUpdate();
                    lastEnemyMoveTime = DateTime.Now;
                }

                if (_enemyList.Count == 0)
                {
                    RespawnEnemies();
                    enemyMoveDiff = 100;
                }

                foreach (var enemy in _enemyList)
                {
                    enemy.Update();
                }

                EnemyCollisions();
                CheckEnemyBulletCollision();
                CheckEnemyPlayerCollision();
                ShowPlayerHealth();
                UpdateObstacles();
                CurrentScore();
            }
        }

        /// <summary>
        /// Génère les ennemis en les espaçant
        /// </summary>
        public void EnemyGenerator()
        {
            int spaceBetweenEnemies = (Console.WindowWidth - 1) / NB_MAX_ENEMIES;

            for (int i = 0; i < NB_MAX_ENEMIES; i++)
            {
                int posX = (i + 1) * spaceBetweenEnemies;

                var enemyType1 = new Enemy("╠═╬═╣", 1, posX, 3, 30);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                _enemyList.Add(enemyType1);
                enemyType1.ShowEnemy();

                var enemyType2 = new Enemy("╔═╦═╗", 2, posX, 5, 20);
                Console.ForegroundColor = ConsoleColor.Blue;
                _enemyList.Add(enemyType2);
                enemyType2.ShowEnemy();

                var enemyType3 = new Enemy("└┬─┬┘", 3, posX, 7, 10);
                Console.ForegroundColor = ConsoleColor.Yellow;
                _enemyList.Add(enemyType3);
                enemyType3.ShowEnemy();
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Génère les obstacles et les place
        /// </summary>
        public void ObstacleGenerator()
        {
            int obstaclePosY = player.PosY - 2;
            int obstacleSpacing = 30;

            for (int i = -1; i <= 1; i++)
            {
                int posX = player.PosX + (i * obstacleSpacing);
                Obstacles obstacle = new Obstacles(posX, obstaclePosY);
                _obstaclesList.Add(obstacle);
                obstacle.ShowObstacle();
            }
        }

        /// <summary>
        /// Met à jour l'affichage des obstacles
        /// </summary>
        public void UpdateObstacles()
        {
            foreach (var obstacle in _obstaclesList)
            {
                if (obstacle.health > 0)
                {
                    obstacle.ShowObstacle();
                }
            }
        }

        /// <summary>
        /// Régénère les ennemis
        /// </summary>
        public void RespawnEnemies()
        {
            _enemyList.Clear();
            EnemyGenerator();
        }

        /// <summary>
        /// Gère les collisions entre les balles du joueur et les ennemis
        /// </summary>
        public void EnemyCollisions()
        {
            var bullet = player.Bullet;
            if (bullet == null) return;

            foreach (Enemy enemy in _enemyList)
            {
                if (bullet.bulletPosX >= enemy.enemyPosX &&
                    bullet.bulletPosX <= (enemy.enemyPosX + enemy.enemyForm.Length - 1) &&
                    (bullet.bulletPosY - 1) == enemy.enemyPosY)
                {
                    bullet.Hide();
                    player.ResetBullet();
                    enemy.HideEnemy();
                    _enemiesToRemove.Add(enemy);
                    _deadEnemies.Add(enemy);
                    break;
                }
            }
            foreach (var enemy in _enemiesToRemove)
            {
                _enemyList.Remove(enemy);
            }
        }

        /// <summary>
        /// Vérifie les collisions des balles d'ennemis avec les obstacles ou le joueur
        /// </summary>
        public void CheckEnemyBulletCollision()
        {
            foreach (var enemy in _enemyList)
            {
                var bullet = enemy._enemyBullet;
                if (bullet != null)
                {
                    bool bulletHit = false;

                    foreach (var obstacle in _obstaclesList.ToList())
                    {
                        if (bullet.bulletPosX >= obstacle.PosX &&
                            bullet.bulletPosX < obstacle.PosX + obstacle.obstacleForm.Length &&
                            bullet.bulletPosY == obstacle.PosY)
                        {
                            bullet.Hide();
                            obstacle.TakeDamage();
                            bulletHit = true;

                            if (obstacle.health <= 0)
                            {
                                obstacle.HideObstacle();
                                _obstaclesList.Remove(obstacle);
                            }

                            enemy._enemyBullet = null;
                            break;
                        }
                    }

                    if (!bulletHit &&
                        bullet.bulletPosX >= player.PosX &&
                        bullet.bulletPosX < player.PosX + width &&
                        bullet.bulletPosY == player.PosY)
                    {
                        bullet.Hide();
                        player.hpPlayer--;
                        enemy._enemyBullet = null;

                        if (player.hpPlayer <= 0)
                        {
                            GameOver();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Vérifie si les ennemis entrent en collision avec le joueur
        /// </summary>
        public void CheckEnemyPlayerCollision()
        {
            foreach (var enemy in _enemyList)
            {
                if (enemy.enemyPosX >= player.PosX &&
                    enemy.enemyPosX < player.PosX + width &&
                    enemy.enemyPosY == player.PosY)
                {
                    GameOver();
                    break;
                }
            }
        }

        /// <summary>
        /// Affiche l'écran de fin de jeu et sauvegarde le highscore si nécessaire
        /// </summary>
        public void GameOver()
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 10) / 2, Console.WindowHeight / 2);
            Console.ForegroundColor = ConsoleColor.Red;

            if (_currentScore > _highscore)
            {
                Console.WriteLine(_gameOverText);
                Console.WriteLine("New Highscore!");
                SaveHighscore();
            }
            else
            {
                Console.WriteLine(_gameOverText);
            }

            Console.WriteLine($"Your Score: {_currentScore}");
            Console.WriteLine($"Highscore: {_highscore}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        /// <summary>
        /// Affiche la vie du joueur
        /// </summary>
        public void ShowPlayerHealth()
        {
            string healthDisplay = $"Vie: {player.hpPlayer}";
            Console.SetCursorPosition(0, 0);
            Console.Write(healthDisplay);
        }

        /// <summary>
        /// Affiche le score actuel et le highscore
        /// </summary>
        public void CurrentScore()
        {
            int score = 0;
            foreach (var enemy in _deadEnemies)
            {
                score += enemy.nbScore;
            }

            _currentScore = score;
            string highscoreDisplay = $"Highscore: {_highscore}";
            Console.SetCursorPosition(Console.WindowWidth - highscoreDisplay.Length, 0);
            Console.WriteLine(highscoreDisplay);

            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"Score: {_currentScore}");
        }

        /// <summary>
        /// Met à jour la position des ennemis
        /// </summary>
        public void EnemyUpdate()
        {
            _direction = ChangeDirection(_direction);
            foreach (Enemy enemy in _enemyList)
            {
                enemy.EnemyMove(_direction);
            }
        }

        /// <summary>
        /// Vérifie la position pour empêcher les ennemis de sortir de l'écran
        /// </summary>
        public int CheckWidth(bool isRight)
        {
            int positionX = isRight ? 0 : Console.WindowWidth;

            foreach (Enemy enemy in _enemyList)
            {
                if (isRight && enemy.enemyPosX > positionX)
                {
                    positionX = enemy.enemyPosX;
                }
                else if (!isRight && enemy.enemyPosX < positionX)
                {
                    positionX = enemy.enemyPosX;
                }
            }
            return positionX;
        }

        /// <summary>
        /// Change la direction des ennemis si nécessaire
        /// </summary>
        public int ChangeDirection(int direction)
        {
            switch (direction)
            {
                case 0:
                    if (CheckWidth(true) < Console.WindowWidth - _ENEMY_CHAR) return 0;
                    return 1;
                case 1:
                    if (CheckWidth(true) == Console.WindowWidth - _ENEMY_CHAR) return 2;
                    return 0;
                case 2:
                    if (CheckWidth(false) > 0) return 2;
                    return 1;
                default:
                    return direction;
            }
        }

        /// <summary>
        /// Charge le highscore depuis un fichier
        /// </summary>
        public void LoadHighscore()
        {
            if (File.Exists(highscoreFilePath))
            {
                string highscoreText = File.ReadAllText(highscoreFilePath);
                if (int.TryParse(highscoreText, out int highscore))
                {
                    _highscore = highscore;
                }
            }
        }

        /// <summary>
        /// Sauvegarde le highscore actuel dans un fichier
        /// </summary>
        public void SaveHighscore()
        {
            if (_currentScore > _highscore)
            {
                _highscore = _currentScore;
                File.WriteAllText(highscoreFilePath, _highscore.ToString());
            }
        }

        public List<Enemy> EnemyList => _enemyList;
    }
}
