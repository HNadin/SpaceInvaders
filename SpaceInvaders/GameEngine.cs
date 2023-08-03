using SpaceInvaders.Objects;

namespace SpaceInvaders
{
    internal class GameEngine
    {
        private static GameEngine instance;

        private readonly GameSettings gameSettings;

        private readonly FrameRender frameRender;

        private readonly Frame frame;

        private bool isGameOver = false;

        private bool isPause = false;

        private readonly ManualResetEvent manualResetEvent;

        private GameEngine(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
            frame = Frame.GetFrame(gameSettings);
            frameRender = new FrameRender(gameSettings);
            manualResetEvent = new ManualResetEvent(true);
            Initialize();
        }

        public void Initialize()
        {
            frame.Initialize();
            isGameOver = false;
            isPause = false;
        }

        public static GameEngine GetGameEngine(GameSettings gameSettings)
        {
            if (instance == null)
            {
                instance = new GameEngine(gameSettings);
            }
            return instance;
        }

        public void Run()
        {
            int invaderMoveCounter = 0;
            int missileCounter = 0;

            do
            {
                frameRender.Render(frame); // малювання

                manualResetEvent.WaitOne();
                Thread.Sleep(gameSettings.GameSpeed); // частота виведення на екран

                frameRender.ClearFrame();

                if (invaderMoveCounter == gameSettings.InvadersSpeed) // часто перерахунку ворожих кораблів
                {
                    InvadersMove(); // переміщення ворожих кораблів
                    invaderMoveCounter = -1;
                }

                invaderMoveCounter++;

                if (missileCounter == gameSettings.MissileSpeed)
                {
                    MissilesMove();
                    InvaderShipShot();
                    missileCounter = -1;
                }

                missileCounter++;

            } while (!isGameOver);

            frameRender.RenderGameOver();

        }

        public void EndGame() // закінчення гри
        {
            isGameOver = true;
        }

        public void PauseGame() // пауза гри
        {
            if (isPause)
            {
                isPause = false;
                manualResetEvent.Set();
            }
            else
            {
                isPause = true;
                manualResetEvent.Reset();
            }
        }

        public void InvaderShipShot()
        {
            frame.AddInvaderMissile();
        }

        public void PlayerShipShot()
        {
            frame.AddPlayerMissile(); // добавляє ракету, випущену гравцем
            Console.Beep(1000, 200); // звуковий ефект
        }

        public void PlayerShipMoveLeft() // пересування корабля користувача вліво
        {
            if (frame.PlayerShip.Left > gameSettings.StartCoordinate)
            {
                frame.PlayerShip.Left--;
            }
        }

        public void PlayerShipMoveRight() // пересування корабля користувача вправо
        {
            if (frame.PlayerShip.Left < gameSettings.ConsoleWidth - 1)
            {
                frame.PlayerShip.Left++;
            }
        }

        public void MissilesMove() // переміщення всіх ракет
        {
            for (int i = 0; i < frame.PlayerMissiles.Count; i++) // ракеті гравця
            {
                Point playerMissile = frame.PlayerMissiles[i]; // поточна ракета
                if (playerMissile.Top == gameSettings.StartCoordinate) // дійшла до вершини
                {
                    frame.PlayerMissiles.RemoveAt(i);
                }
                else
                {
                    playerMissile.Top--; // зменшуємо координату ракети(зверху вниз)

                    if (ObjectsHitByMissile(playerMissile, frame.InvaderMissiles, out int invaderRocketIndex)) // координати ракети гравця та ракети ворога співпали
                    {
                        frame.InvaderMissiles.RemoveAt(invaderRocketIndex);
                        frame.PlayerMissiles.RemoveAt(i);
                    }
                    else if (ObjectsHitByMissile(playerMissile, frame.Invaders, out int invaderIndex)) // координати ракети гравця співпали з координатами корабля ворога
                    {
                        frame.Invaders.RemoveAt(invaderIndex);
                        frame.PlayerMissiles.RemoveAt(i);
                    }
                }
            }

            for (int i = 0; i < frame.InvaderMissiles.Count; i++)  // ракети ворога
            {
                Point invaderMissile = frame.InvaderMissiles[i];
                if (invaderMissile.Top == gameSettings.ConsoleHeight - 1) // ракета дійшла до низу
                {
                    frame.InvaderMissiles.RemoveAt(i);
                }
                else
                {
                    invaderMissile.Top++;
                    if (ObjectsHitByMissile(invaderMissile, frame.PlayerMissiles, out int playerRocketIndex)) // ракета зіткнулась з ракетою гравця
                    {
                        frame.InvaderMissiles.RemoveAt(i);
                        frame.PlayerMissiles.RemoveAt(playerRocketIndex);
                    }
                    else if (ObjectHitByMissile(invaderMissile, frame.PlayerShip))
                    {
                        isGameOver = true;
                    }
                    else if (ObjectsHitByMissile(invaderMissile, frame.Earths, out int earthsIndex)) // ракета зіткнулась з землею
                    {
                        frame.Earths.RemoveAt(earthsIndex);
                        frame.InvaderMissiles.RemoveAt(i);
                    }
                }
            }

            if (frame.Invaders.Count == 0 || frame.Earths.Count <= gameSettings.MinimumEarth)
            {
                isGameOver = true;
            }
        }



        public void InvadersMove() // пересування ворожих кораблі вниз
        {
            for (int i = 0; i < frame.Invaders.Count; i++)
            {
                frame.Invaders[i].Top++;

                if (frame.Invaders[i].Top == frame.PlayerShip.Top) // якщо ворожі кораблі дойшли до корабля користувача
                {
                    isGameOver = true;
                    break;
                }
            }
        }

        private bool ObjectsHitByMissile(Point missile, List<Point> list, out int objectIndex)
        {
            objectIndex = -1;
            for (int j = 0; j < list.Count; j++)
            {
                if (ObjectHitByMissile(missile, list[j]))
                {
                    objectIndex = j;
                    return true;
                }
            }
            return false;
        }

        private bool ObjectHitByMissile(Point missile, Point obj)
        {
            return missile.Compare(obj);
        }
    }
}
