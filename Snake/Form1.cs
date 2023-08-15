using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    namespace Snake
    {
        public partial class Form1 : Form
        {
            private readonly List<SnakePart> snake = new List<SnakePart>();
            private const int tileWidth = 16;
            private const int tileHeight = 16;
            private bool gameover = false;
            private int score = 0;
            private int direction = 0; // Down = 0, Left = 1, Right = 2, Up = 3
            private SnakePart foodPiece = new SnakePart();

            public Form1()
            {
                InitializeComponent();
                gameTimer.Interval = 1000 / 4;
                gameTimer.Tick += Update;
                gameTimer.Start();
                StartGame();
            }

            private void StartGame()
            {
                gameover = false;
                score = 0;
                direction = 0;
                snake.Clear();
                snake.Add(new SnakePart { X = 10, Y = 5 });
                GenerateFood();
            }

            private void GenerateFood()
            {
                int maxTileWidth = pbCanvas.Size.Width / tileWidth;
                int maxTileHeight = pbCanvas.Size.Height / tileHeight;
                Random random = new Random();
                foodPiece = new SnakePart { X = random.Next(0, maxTileWidth), Y = random.Next(0, maxTileHeight) };
            }

            private void Update(object sender, EventArgs e)
            {
                if (gameover)
                {
                    if (Input.Pressed(Keys.Enter))
                        StartGame();
                }
                else
                {
                    if (Input.Pressed(Keys.Right) && (snake.Count < 2 || snake[0].X == snake[1].X))
                        direction = 2;
                    else if (Input.Pressed(Keys.Left) && (snake.Count < 2 || snake[0].X == snake[1].X))
                        direction = 1;
                    else if (Input.Pressed(Keys.Up) && (snake.Count < 2 || snake[0].Y == snake[1].Y))
                        direction = 3;
                    else if (Input.Pressed(Keys.Down) && (snake.Count < 2 || snake[0].Y == snake[1].Y))
                        direction = 0;

                    UpdateSnake();
                }
                pbCanvas.Invalidate();
            }

            private void UpdateSnake()
            {
                for (int i = snake.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        switch (direction)
                        {
                            case 0: // Down
                                snake[i].Y++;
                                break;
                            case 1: // Left
                                snake[i].X--;
                                break;
                            case 2: // Right
                                snake[i].X++;
                                break;
                            case 3: // Up
                                snake[i].Y--;
                                break;
                        }

                        int maxTileWidth = pbCanvas.Width / tileWidth;
                        int maxTileHeight = pbCanvas.Height / tileHeight;

                        if (snake[i].X < 0 || snake[i].X >= maxTileWidth || snake[i].Y < 0 || snake[i].Y >= maxTileHeight)
                            gameover = true;

                        for (int j = 1; j < snake.Count; j++)
                        {
                            if (snake[i].X == snake[j].X && snake[i].Y == snake[j].Y)
                            {
                                gameover = true;
                                break;
                            }
                        }

                        if (snake[i].X == foodPiece.X && snake[i].Y == foodPiece.Y)
                        {
                            SnakePart part = new SnakePart { X = snake[snake.Count - 1].X, Y = snake[snake.Count - 1].Y };
                            snake.Add(part);
                            GenerateFood();
                            score++;
                        }
                    }
                    else
                    {
                        snake[i].X = snake[i - 1].X;
                        snake[i].Y = snake[i - 1].Y;
                    }
                }
            }

            private void Form1_KeyDown(object sender, KeyEventArgs e)
            {
                Input.ChangeState(e.KeyCode, true);
            }

            private void Form1_KeyUp(object sender, KeyEventArgs e)
            {
                Input.ChangeState(e.KeyCode, false);
            }
        }
    }






    private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (gameover)
            {
                Font font = this.Font;
                string gameover_msg = "Gameover";
                string score_msg = "Score: " + score.ToString();
                string newgame_msg = "Press Enter to Start Over";
                int center_width = pbCanvas.Width / 2;
                SizeF msg_size = canvas.MeasureString(gameover_msg, font);
                PointF msg_point = new PointF(center_width - msg_size.Width / 2, 16);
                canvas.DrawString(gameover_msg, font, Brushes.Black, msg_point);
                msg_size = canvas.MeasureString(score_msg, font);
                msg_point = new PointF(center_width - msg_size.Width / 2, 32);
                canvas.DrawString(score_msg, font, Brushes.Black, msg_point);
                msg_size = canvas.MeasureString(newgame_msg, font);
                msg_point = new PointF(center_width - msg_size.Width / 2, 48);
                canvas.DrawString(newgame_msg, font, Brushes.Black, msg_point);
            }
            else
            {
                for (int i = 0; i < snake.Count; i++)
                {
                    Brush snake_color = i == 0 ? Brushes.Green : Brushes.Green;
                    canvas.FillRectangle(snake_color, new Rectangle(snake[i].X * tile_width, snake[i].Y * tile_height, tile_width, tile_height));
                }
                canvas.FillRectangle(Brushes.Red, new Rectangle(food_piece.X * tile_width, food_piece.Y * tile_height, tile_width, tile_height));
                canvas.DrawString("Score: " + score.ToString(), this.Font, Brushes.White, new PointF(4, 4));
            }
        }
    }
}
