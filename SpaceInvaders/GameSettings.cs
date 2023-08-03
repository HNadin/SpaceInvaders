using System.Text;

namespace SpaceInvaders
{
    internal class GameSettings
    {
        public GameSettings()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false; // прибираємо видимість курсора
        }

        public int StartCoordinate { get; } = 0; // початок координат

        public int ConsoleWidth { get; } = 80; // ширина поля

        public int ConsoleHeight { get; } = 20; // висота поля

        public int InvadersStartPositionLeft { get; } = 10; // координати початку нападающих кораблів по ширині

        public int InvadersStartPositionTop { get; } = 1;  // координати початку нападающих кораблів по висоті

        public int InvadersCount { get; } = 60; // кількість кораблів в одному ряді

        public int InvadersLines { get; } = 2; // кількість нападающих рядів

        public char InvadersShip { get; } = '\u00A5'; 

        public int EarthStartPositionLeft { get; } = 0; // координати початку зкмлі по ширині

        public int EarthStartPositionTop { get; } = 19; // координати початку землі по висоті

        public int EarthCount { get; } = 80; // кільсть символів землі

        public int MinimumEarth { get; } = 20; // кількість землі по бокам

        public char Earth { get; } = '\u2550'; 

        public int PlayerShipStartPositionLeft { get; } = 39; //координати створення корабля користувача по ширині

        public int PlayerShipStartPositionTop { get; } = 18; //координати створення корабля користувача по висоті

        public char PlayerShip { get; } = '\u028C'; 

        public char PlayerMissile { get; } = '\u0164';

        public char InvaderMissile { get; } = '\u013C';

        public int GameSpeed { get; } = 100; // кожні 100 мілісекунд відбуватметься затримка та зміна кадру

        public int InvadersSpeed { get; } = 40;

        public int MissileSpeed { get; } = 3;

        public int InvaderShotSpeed { get; } = 3;

        public int ScorePositionTop { get; } = 22; // координати рахунку по висоті

        public int ScorePositionLeft { get; } = 2; // координати рахунку по ширині

    }
}
