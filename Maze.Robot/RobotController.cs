using Maze.Library;
using System.Collections.Generic;
using System.Drawing;

//HÜ
namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
        }


        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        private bool reachedEnd = false;
        public HashSet<Point> alreadyChecked { get; private set; }
        public void MoveRobotToExit()
        {
            Point startPoint = new Point(0, 0);
            alreadyChecked = new HashSet<Point>();

            robot.ReachedExit += (_, __) => reachedEnd = true;

            CheckPoint(startPoint);

            if (reachedEnd == false)
                robot.HaltAndCatchFire();
        }
        public void CheckPoint(Point currentPoint)
        {
            if (alreadyChecked.Contains(currentPoint) == false && reachedEnd == false)
            {
                alreadyChecked.Add(currentPoint);
                if (robot.TryMove(Direction.Left) == true)
                {
                    Point newtestpoint = new Point(currentPoint.X - 1, currentPoint.Y);
                    CheckPoint(newtestpoint);
                    if (reachedEnd == false) { robot.Move(Direction.Right); }
                }

                if (reachedEnd == false && robot.TryMove(Direction.Right) == true)
                {
                    Point newtestpoint = new Point(currentPoint.X + 1, currentPoint.Y);
                    CheckPoint(newtestpoint);
                    if (reachedEnd == false) { robot.Move(Direction.Left); }
                }

                if (reachedEnd == false && robot.TryMove(Direction.Down) == true)
                {
                    Point newtestpoint = new Point(currentPoint.X, currentPoint.Y + 1);
                    CheckPoint(newtestpoint);
                    if (reachedEnd == false) { robot.Move(Direction.Up); }
                }

                if (reachedEnd == false && robot.TryMove(Direction.Up) == true)
                {
                    Point newtestpoint = new Point(currentPoint.X, currentPoint.Y - 1);
                    CheckPoint(newtestpoint);
                    if (reachedEnd == false) { robot.Move(Direction.Down); }
                }
            }
        }
    }
}
