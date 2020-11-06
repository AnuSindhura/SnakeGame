using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        public Form1()
        {
            InitializeComponent();

            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += updateScreen;
            gameTimer.Start();

            startGame();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            // the key down event will trigger the change state from the Input class
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // the key down event will trigger the change state from the Input class
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            //In this we will see snake and its parts moving 
            Graphics canvas = e.Graphics;

            if(Settings.GameOver==false)
            {
                Brush snakeColour;
                
                //loop to check snake parts
                for(int i =0; i < Snake.Count; i++)
                {
                    if(i==0)
                    {
                        snakeColour = Brushes.Black;//snake head color is black
                    }
                    else
                    {
                        snakeColour = Brushes.Green; //snake body color is green
                    }
                    // draw snake body and head
                    canvas.FillEllipse(snakeColour,
                                       new Rectangle(
                                       Snake[i].X * Settings.Width,
                                       Snake[i].Y * Settings.Height,
                                       Settings.Width, Settings.Height));
                    //draw food
                    canvas.FillEllipse(Brushes.Red,
                                       new Rectangle(
                                           food.X * Settings.Width,
                                           food.Y * Settings.Height,
                                           Settings.Width, Settings.Height));
                }
            }
            else
            {
                //this part shows game over text and the three labels on the screen 
                string gameOver = "Game Over \n " + "Final Score is " + Settings.Score + "\n Press enter to Restart \n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }

        private void startGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            label2.Text = Settings.Score.ToString();

            generateFood();
        }

        private void movePlayer()
        {
            //main loop for snake head and parts
            for(int i = Snake.Count -1; i >= 0; i--)
            {
                //if snake head is active
                if( i == 0)
                {
                    //moving the rest of the body according to the snake head movement
                    switch (Settings.direction)
                    {
                        case Directions.Right: Snake[i].X++;
                            break;
                        case Directions.Left: Snake[i].X--;
                            break;
                        case Directions.Up: Snake[i].Y--;
                            break;
                        case Directions.Down: Snake[i].Y++;
                            break;
                    }
                    //restricting the snake from leaving canvas
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if(Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxXpos || Snake[i].Y > maxYpos)
                    {
                        //end the game if snake either reaches the edge of the canvas
                        die();
                    }
                    //this loop checks if there is any collision with the other body 
                    for(int j =1; j<Snake.Count; j++)
                    {
                        if(Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            die();
                        }
                    }

                    //detect collission between snake head and food
                    if(Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        eat();
                    }
                }
                else
                {
                    //if there are no collisions then continue moving snake parts
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }
        private void generateFood()
        {
            //this will generate the food icon on a random location in the game area 
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;
            Random rnd = new Random();
            food = new Circle { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
        }
        private void eat()
        {
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);
            Settings.Score += Settings.Points; // increses score for the game
            label2.Text = Settings.Score.ToString();
            generateFood();
        }
        private void die()
        {
            Settings.GameOver = true;
        }

        private void updateScreen(object sender, EventArgs e)
        {
            //time update screen function where each tick will run this function
            if(Settings.GameOver==true)
            {
                //if game is over then player press enter and runs start game function
                if(Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                //if game is not over these commands will be executed accordingly
                if(Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if(Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if(Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if(Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }
                movePlayer();
            }
            pbCanvas.Invalidate(); //refresh the graphic and update graphic
        }
    }
}
