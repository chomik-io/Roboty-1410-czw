using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NKH.MindSqualls;
using NKH.MindSqualls.HiTechnic;

namespace app
{
    public class Program
    {
        public static char[,] map = loadMap();

        public static double[,] startDistanceMap = new double[map.GetLength(0), map.GetLength(1)];

        public static double[,] goalDistanceMap = new double[map.GetLength(0), map.GetLength(1)];

        public static int startPosX = 0;
        public static int startPosY = 0;
        public static int goalPosX = 0;
        public static int goalPosY = 0;

        public static int NORTHDIR = 0;
        public static int EASTDIR = 0;
        public static int SOUTHDIR = 0;
        public static int WESTDIR = 0;

        public const int NORTH = 1;
        public const int EAST = 2;
        public const int SOUTH = 3;
        public const int WEST = 4;

        public static NxtBrick brick = null;
        public static HiTechnicCompassSensor compass = null;

        public const int SLEEPTIME = 1000;

        public static void printMap()
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.Write("\n");
            }
        }

        public static void Main(string[] args)
        {
            //wspolrzedne startu i celu
            getPositions();

            startDistanceMap = createDistanceMap(startDistanceMap, startPosX, startPosY);

            removeStars();

            goalDistanceMap = createDistanceMap(goalDistanceMap, goalPosX, goalPosY);

            removeStars();

            //for (int i = 0; i < startDistanceMap.GetLength(0); i++)
            //{
            //    for (int j = 0; j < startDistanceMap.GetLength(1); j++)
            //    {
            //        Console.Write(startDistanceMap[i, j] + "\t");
            //    }
            //    Console.Write("\n");
            //}

            //Console.WriteLine("-----");

            //for (int i = 0; i < goalDistanceMap.GetLength(0); i++)
            //{
            //    for (int j = 0; j < goalDistanceMap.GetLength(1); j++)
            //    {
            //        Console.Write(goalDistanceMap[i, j] + "\t");
            //    }
            //    Console.Write("\n");
            //}

            //remove stars

            double[,] path = createPath();

            //for (int i = 0; i < path.GetLength(0); i++)
            //{
            //    for (int j = 0; j < path.GetLength(1); j++)
            //    {
            //        Console.Write(path[i, j] + "\t");
            //    }
            //    Console.Write("\n");
            //}

            //remove stars

            removeStars();

            //uruchomienie robota

            //brick = new NxtBrick(NxtCommLinkType.USB, 0);

            //polaczenie bt
            brick = new NxtBrick(NxtCommLinkType.Bluetooth, 18);

            brick.MotorB = new NxtMotor();
            brick.MotorC = new NxtMotor();

            compass = new NKH.MindSqualls.HiTechnic.HiTechnicCompassSensor();
            brick.Sensor4 = compass;

            compass.PollInterval = 50;

            compass.OnPolled += new Polled(compassSensor_OnPolled);

            brick.Connect();
            Console.WriteLine(brick.Name);

            Console.WriteLine("WCISNIJ ENTER ABY POBRAC KIERUNEK POLNOCNY");

            Console.ReadLine();

            compass.Poll();
            NORTHDIR = compass.Heading.Value;

            EASTDIR = (NORTHDIR + 90) % 360;
            SOUTHDIR = (NORTHDIR + 180) % 360;
            WESTDIR = (NORTHDIR-90+360)%360;

            //

            int oldX = startPosX;
            int oldY = startPosY;
            int direction = NORTH;

            do
            {
                
                // map[oldX, oldY] = '*';
                map[oldX, oldY] = ' ';

                int newDir = (int)path[oldX, oldY];

                if (direction == NORTH)
                {
                    if (newDir == NORTH)
                    {
                        Console.WriteLine("\nJedź prosto");
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == EAST)
                    {
                        Console.WriteLine("\nSkręć w prawo");
                        turnRight(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == SOUTH)
                    {
                        Console.Write("\nZawróć");
                        turnRight(direction, newDir, 180);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == WEST)
                    {
                        Console.WriteLine("\nSkręć w lewo");
                        turnLeft(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                }
                if (direction == EAST)
                {
                    if (newDir == NORTH)
                    {
                        Console.WriteLine("\nSkręć w lewo");
                        turnLeft(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == EAST)
                    {
                        Console.WriteLine("\nJedź prosto");
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == SOUTH)
                    {
                        Console.WriteLine("\nSkręć w prawo");
                        turnRight(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == WEST)
                    {
                        Console.Write("\nZawróć");
                        turnRight(direction, newDir, 180);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                }
                if (direction == SOUTH)
                {
                    if (newDir == NORTH)
                    {
                        Console.Write("\nZawróć");
                        turnRight(direction, newDir, 180);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == EAST)
                    {
                        Console.WriteLine("\nSkręć w lewo");
                        turnLeft(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == SOUTH)
                    {
                        Console.WriteLine("\nJedź prosto");
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == WEST)
                    {
                        Console.WriteLine("\nSkręć w prawo");
                        turnRight(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                }
                if (direction == WEST)
                {
                    if (newDir == NORTH)
                    {
                        Console.WriteLine("\nSkręć w prawo");
                        turnRight(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == EAST)
                    {
                        Console.Write("\nZawróć");
                        turnRight(direction, newDir, 180);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == SOUTH)
                    {
                        Console.WriteLine("\nSkręć w lewo");
                        turnLeft(direction, newDir, 90);
                        
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                    if (newDir == WEST)
                    {
                        Console.WriteLine("\nJedź prosto");
                        goForward();
                        System.Threading.Thread.Sleep(SLEEPTIME);
                    }
                }

                direction = newDir;

                //System.Threading.Thread.Sleep(SLEEPTIME);

                switch (newDir)
                {
                    case 1:
                        oldX = oldX - 1;
                        break;
                    case 2:
                        oldY = oldY + 1;
                        break;
                    case 3:
                        oldX = oldX + 1;
                        break;
                    case 4:
                        oldY = oldY - 1;
                        break;
                    default:
                        break;
                }
                map[oldX, oldY] = '*';

                printMap();
            } while (path[oldX, oldY] > 0);

            Console.WriteLine("\nJesteś u celu! Prowadził Cię Krzysztof Hołowczyc.");

            brick.Disconnect();

            Console.ReadLine();
        }

        public static void goForward()
        {
            brick.MotorB.Run(25, 500);
            brick.MotorC.Run(25, 500);
        }

        public static void turnRight(int lastDir, int newDir, int degrees)
        {
            int goalDir = 0;

            switch (newDir)
            {
                case NORTH:
                    goalDir = NORTHDIR;
                    break;
                case EAST:
                    goalDir = EASTDIR;
                    break;
                case SOUTH:
                    goalDir = SOUTHDIR;
                    break;
                case WEST:
                    goalDir = WESTDIR;
                    break;
                default:
                    break;
            }

            int heading = 0;

            int lastDirection = 0;

            switch (lastDir)
            {
                case NORTH:
                    lastDirection = NORTHDIR;
                    break;
                case EAST:
                    lastDirection = EASTDIR;
                    break;
                case SOUTH:
                    lastDirection = SOUTHDIR;
                    break;
                case WEST:
                    lastDirection = WESTDIR;
                    break;
                default:
                    break;
            }

            int goalDirection = ((lastDirection + degrees) + 360) % 360; //90 lub 180

            if (goalDirection > 360) //przechodzi przez 0
            {
                bool add360degrees = false;
                int lastHeading = 0;
                int trueHeading = 0;

                compass.Poll();

                lastHeading = compass.Heading.Value;

                do
                {
                    brick.MotorB.Run(-30, 20);
                    brick.MotorC.Run(30, 20);

                    System.Threading.Thread.Sleep(SLEEPTIME);
                    compass.Poll();

                    heading = compass.Heading.Value;

                    if (lastHeading <= 360 && heading >= 0)
                    {
                        add360degrees = true;
                    }

                    if (add360degrees)
                    {
                        trueHeading = heading + 360;
                    }
                    else
                    {
                        trueHeading = heading;
                    }

                    lastHeading = heading;

                } while (trueHeading < goalDirection);
            }
            else //nie przechodzi przez 0
            {
                do
                {
                    brick.MotorB.Run(-30, 50);
                    brick.MotorC.Run(30, 50);

                    System.Threading.Thread.Sleep(SLEEPTIME);
                    compass.Poll();

                    heading = compass.Heading.Value;
                } while (!(heading < (goalDir+10) && heading > (goalDir-10)));
            }

            System.Threading.Thread.Sleep(SLEEPTIME);
        }

        public static void turnLeft(int lastDir, int newDir, int degrees)
        {
            int goalDir = 0;

            switch (newDir)
            {
                case NORTH:
                    goalDir = NORTHDIR;
                    break;
                case EAST:
                    goalDir = EASTDIR;
                    break;
                case SOUTH:
                    goalDir = SOUTHDIR;
                    break;
                case WEST:
                    goalDir = WESTDIR;
                    break;
                default:
                    break;
            }

            int heading = 0;

            int lastDirection = 0;

            switch (lastDir)
            {
                case NORTH:
                    lastDirection = NORTHDIR;
                    break;
                case EAST:
                    lastDirection = EASTDIR;
                    break;
                case SOUTH:
                    lastDirection = SOUTHDIR;
                    break;
                case WEST:
                    lastDirection = WESTDIR;
                    break;
                default:
                    break;
            }

            int goalDirection = ((lastDirection - degrees)+360)%360; //90 lub 180

            if (goalDirection < 0) //przechodzi przez 0
            {
                bool minus360degrees = false;
                int lastHeading = 0;
                int trueHeading = 0;

                compass.Poll();

                lastHeading = compass.Heading.Value;

                do
                {
                    brick.MotorB.Run(30, 20);
                    brick.MotorC.Run(-30, 20);

                    System.Threading.Thread.Sleep(SLEEPTIME);
                    compass.Poll();
                    
                    heading = compass.Heading.Value;

                    if (lastHeading >= 360 && heading <= 0)
                    {
                        minus360degrees = true;
                    }

                    if (minus360degrees)
                    {
                        trueHeading = heading - 360;
                    }
                    else
                    {
                        trueHeading = heading;
                    }

                    lastHeading = heading;

                } while (trueHeading < (goalDirection-10) && trueHeading > (goalDirection+10));
            }
            else //nie przechodzi przez 0
            {
                do
                {
                    brick.MotorB.Run(30, 50);
                    brick.MotorC.Run(-30, 50);

                    System.Threading.Thread.Sleep(SLEEPTIME);
                    compass.Poll();

                    heading = compass.Heading.Value;

                } while (!(heading < (goalDir+10) && heading > (goalDir-10)));
            }

            System.Threading.Thread.Sleep(SLEEPTIME);
        }

        public static void removeStars()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j].ToString() == "*")
                    {
                        map[i, j] = ' ';
                    }
                }
            }
        }

        public static void compassSensor_OnPolled(NxtPollable polledItem)
        {
            //ushort heading = compassSensor.Heading.Value;

            // Console.WriteLine(heading);
            // turnRatio = (sbyte)((heading > 180) ? 100 : -100);

            //sbyte power = (sbyte)Math.Min(100, 180 - Math.Abs(heading - 180));

            //motorPair.Run(power, 0, turnRatio);
        }

        public static char[,] loadMap()
        {
            //wczytywanie pliku
            string plik = System.IO.File.ReadAllText(@"map.txt");

            //odczyt ilosci wierszy
            string[] wiersze = plik.Split("\n".ToCharArray());

            //ilosc kolumn
            int colsCount = wiersze[0].Length;
            int rowsCount = wiersze.Count();

            char[,] map = new char[rowsCount, colsCount - 1];

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount - 1; j++)
                {
                    map[i, j] = wiersze[i][j];
                }
            }

            return map;
        }

        public static void getPositions()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i, j].ToString())
                    {
                        case "S":
                            startPosX = i;
                            startPosY = j;
                            break;
                        case "G":
                            goalPosX = i;
                            goalPosY = j;
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        public static double[,] createDistanceMap(double[,] clearMap, int posX, int posY)
        {
            double[,] newMap = new double[clearMap.GetLength(0), clearMap.GetLength(1)];

            int dist = 1;

            //start
            newMap[startPosX, startPosY] = -1;

            //cel
            newMap[goalPosX, goalPosY] = -2;

            List<Krotka> tempList = new List<Krotka>();

            //SKAD ZACZYNAMY
            tempList = createNList(posX, posY, dist);

            int y = tempList.Count();

            for (int i = 0; i < y; )
            {
                dist = tempList[i].distance;
                newMap[tempList[i].posX, tempList[i].posY] = dist;
                map[tempList[i].posX, tempList[i].posY] = '*';

                //printMap();

                List<Krotka> temp = new List<Krotka>();
                temp = createNList(tempList[i].posX, tempList[i].posY, tempList[i].distance + 1);

                foreach (var item in temp)
                {
                    if (!tempList.Any(x => x.posX == item.posX && x.posY == item.posY))
                    {
                        tempList.Add(item);
                    }
                }
                tempList.Remove(tempList[i]);
                y = tempList.Count();
            }

            for (int i = 0; i < newMap.GetLength(0); i++)
            {
                for (int j = 0; j < newMap.GetLength(1); j++)
                {
                    if (newMap[i, j] == 0)
                    {
                        newMap[i, j] = 777;
                    }
                }
            }

            return newMap;
        }

        public static List<Krotka> createNList(int posX, int posY, int dist)
        {

            List<Krotka> toReturn = new List<Krotka>();

            try
            {
                if (map[posX - 1, posY].ToString() == " ")
                {
                    Krotka n = new Krotka()
                    {
                        posX = posX - 1,
                        posY = posY,
                        value = map[posX - 1, posY],
                        distance = dist
                    };
                    toReturn.Add(n);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                //throw;
            }

            try
            {
                if (map[posX, posY + 1].ToString() == " ")
                {
                    Krotka e = new Krotka()
                    {
                        posX = posX,
                        posY = posY + 1,
                        value = map[posX, posY + 1],
                        distance = dist
                    };
                    toReturn.Add(e);
                }
            }
            catch (IndexOutOfRangeException ex)
            {

                //throw;
            }

            try
            {
                if (map[posX, posY - 1].ToString() == " ")
                {
                    Krotka w = new Krotka()
                    {
                        posX = posX,
                        posY = posY - 1,
                        value = map[posX, posY - 1],
                        distance = dist
                    };
                    toReturn.Add(w);
                }
            }
            catch (IndexOutOfRangeException ex)
            {

                //throw;
            }

            try
            {
                if (map[posX + 1, posY].ToString() == " ")
                {
                    Krotka s = new Krotka()
                    {
                        posX = posX + 1,
                        posY = posY,
                        value = map[posX + 1, posY],
                        distance = dist
                    };
                    toReturn.Add(s);
                }
            }
            catch (IndexOutOfRangeException ex)
            {

                //throw;
            }

            return toReturn;
        }

        public static double[,] createPath()
        {
            double[,] newMap = new double[map.GetLength(0), map.GetLength(1)];

            //start
            newMap[startPosX, startPosY] = 7;

            //cel
            newMap[goalPosX, goalPosY] = -2;

            map[goalPosX, goalPosY] = ' ';

            List<Krotka> newList = createAreaN(startPosX, startPosY);

            int size = newList.Count();

            int lastPosX = startPosX;
            int lastPosY = startPosY;

            int direction = 1;
            //1-north
            //2-east
            //3-south
            //4-west

            List<Krotka> path = new List<Krotka>();

            Krotka start = new Krotka()
            {
                posX = startPosX,
                posY = startPosY
            };

            path.Add(start);

            for (int i = 0; i < size; )
            {
                Krotka glowna = newList.Last();

                for (int k = newList.Count - 1; k >= 0; k--)
                {
                    if (newList[k].xFunction < glowna.xFunction)
                    {
                        glowna = newList[k];
                    }
                }


                //Krotka glowna = newList.OrderBy(x=>x.xFunction).First();

                //newMap[glowna.posX, glowna.posY] = direction;

                map[glowna.posX, glowna.posY] = '*';

                List<Krotka> tempList = newList.ToList();

                newList = newList.Concat(createAreaN(glowna.posX, glowna.posY)).ToList();

                lastPosX = glowna.posX;
                lastPosY = glowna.posY;

                newList.Remove(glowna);
                path.Add(glowna);
                size = newList.Count();
            }

            Krotka goal = new Krotka();

            goal = path.Where(x => x.posX == goalPosX && x.posY == goalPosY).First();

            path.Add(goal);

            size = path.Count();

            Krotka first = path.Last();

            for (int i = 0; i < size; )
            {

                if (first.xFunction == 0)
                {
                    break;
                }
                Krotka second = path.Where(x => x.posX == first.rootPosX && x.posY == first.rootPosY).First();


                //Krotka first = path.First();
                //Krotka second = path.Skip(1).First();

                if ((second.posX - first.posX) < 0 && second.posY == first.posY)
                {
                    direction = 3;
                }
                if (second.posX == first.posX && (second.posY - first.posY) > 0)
                {
                    direction = 4;
                }
                if ((second.posX - first.posX) > 0 && second.posY == first.posY)
                {
                    direction = 1;
                }
                if (second.posX == first.posX && (second.posY - first.posY) < 0)
                {
                    direction = 2;
                }

                newMap[second.posX, second.posY] = direction;
                path.Remove(first);
                size = path.Count();
                first = second;
            }

            return newMap;
        }

        public static List<Krotka> createAreaN(int posX, int posY)
        {
            List<Krotka> toReturn = new List<Krotka>();

            try
            {
                if (map[posX - 1, posY].ToString() == " ")
                {
                    Krotka n = new Krotka()
                    {
                        posX = posX - 1,
                        posY = posY,
                        value = map[posX - 1, posY],
                        xFunction = startDistanceMap[posX - 1, posY] + goalDistanceMap[posX - 1, posY],
                        rootPosX = posX,
                        rootPosY = posY
                        //xFunction = startDistanceMap[posX - 1, posY] + Math.Sqrt(Math.Pow(((posX-1)-posY),2)+Math.Pow((goalPosX-goalPosY),2))
                    };
                    toReturn.Add(n);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                //throw;
            }

            try
            {
                if (map[posX, posY + 1].ToString() == " ")
                {
                    Krotka e = new Krotka()
                    {
                        posX = posX,
                        posY = posY + 1,
                        value = map[posX, posY + 1],
                        xFunction = startDistanceMap[posX, posY + 1] + goalDistanceMap[posX, posY + 1],
                        rootPosX = posX,
                        rootPosY = posY
                        //xFunction = startDistanceMap[posX, posY+1] + Math.Sqrt(Math.Pow((posX - (posY+1)), 2) + Math.Pow((goalPosX - goalPosY), 2))
                    };
                    toReturn.Add(e);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                //throw;
            }

            try
            {
                if (map[posX + 1, posY].ToString() == " ")
                {
                    Krotka s = new Krotka()
                    {
                        posX = posX + 1,
                        posY = posY,
                        value = map[posX + 1, posY],
                        xFunction = startDistanceMap[posX + 1, posY] + goalDistanceMap[posX + 1, posY],
                        rootPosX = posX,
                        rootPosY = posY
                        //xFunction = startDistanceMap[posX + 1, posY] + Math.Sqrt(Math.Pow(((posX + 1) - posY), 2) + Math.Pow((goalPosX - goalPosY), 2))
                    };
                    toReturn.Add(s);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                //throw;
            }

            try
            {
                if (map[posX, posY - 1].ToString() == " ")
                {
                    Krotka w = new Krotka()
                    {
                        posX = posX,
                        posY = posY - 1,
                        value = map[posX, posY - 1],
                        xFunction = startDistanceMap[posX, posY - 1] + goalDistanceMap[posX, posY - 1],
                        rootPosX = posX,
                        rootPosY = posY
                        //xFunction = startDistanceMap[posX, posY - 1] + Math.Sqrt(Math.Pow((posX - (posY - 1)), 2) + Math.Pow((goalPosX - goalPosY), 2))
                    };
                    toReturn.Add(w);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                //throw;
            }

            return toReturn;
        }

        public class Krotka
        {
            public int posX { get; set; }
            public int posY { get; set; }
            public char value { get; set; }
            public double xFunction { get; set; }
            public int distance { get; set; }
            public int rootPosX { get; set; }
            public int rootPosY { get; set; }
        }
    }
}