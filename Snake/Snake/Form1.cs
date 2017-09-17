using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
	public partial class Form1 : Form
	{
		readonly List<Circle> snake = new List<Circle>();
		Circle food;

		public Form1()
		{
			InitializeComponent();

			StartGame();
			timer.Interval = 1000 / Settings.Speed;
			timer.Start();
		}

		private void Canvas_Paint(object sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			if (!Settings.GameOver)
			{
				for (var i = 0; i < snake.Count; i++)
				{
					var snakeColor = i == 0 ? Brushes.Black : Brushes.Green;
					g.FillEllipse(snakeColor, new Rectangle(snake[i].X * Settings.Width, snake[i].Y * Settings.Height, Settings.Width, Settings.Height));
				}
				g.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height));
			}
		}

		private void UpdateScreen(object sender, EventArgs e)
		{
			if (Settings.GameOver)
			{
				StartGame();
			}
			else
			{
				MovePlayer();
			}

			canvas.Invalidate();
		}

		void MovePlayer()
		{
			var maxXPos = canvas.Size.Width / Settings.Width;
			var maxYPos = canvas.Size.Height / Settings.Height;

			for (var i = snake.Count - 1; i >= 0; i--)
			{
				if (i == 0)
				{
					switch (Settings.Direction)
					{
						case Direction.Up:
							snake[i].Y = snake[i].Y - 1 < 0 ? maxYPos : snake[i].Y - 1;
							break;
						case Direction.Down:
							snake[i].Y = snake[i].Y + 1 > maxYPos ? 0 : snake[i].Y + 1;
							break;
						case Direction.Right:
							snake[i].X = snake[i].X + 1 > maxXPos ? 0 : snake[i].X + 1;
							break;
						case Direction.Left:
							snake[i].X = snake[i].X - 1 < 0 ? maxXPos : snake[i].X - 1;
							break;
					}

					var snakeHead = snake[i];

					if (snakeHead.X == food.X && snake[0].Y == food.Y)
					{
						Eat();
					}
				}
				else
				{
					snake[i].X = snake[i - 1].X;
					snake[i].Y = snake[i - 1].Y;
				}
			}
		}

		void Eat()
		{
			snake.Add(new Circle(snake[snake.Count - 1].X, snake[snake.Count - 1].Y));
			food = GenerateFood();
		}

		void StartGame()
		{
			snake.Clear();
			snake.Add(new Circle(10, 5));
			food = GenerateFood();
		}

		Circle GenerateFood()
		{
			var maxXPos = canvas.Size.Width/Settings.Width ;
			var maxYPos = canvas.Size.Height /Settings.Height ;

			return new Circle(random.Next(0, maxXPos), random.Next(0, maxYPos));
		}


		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Up:
					if (Settings.Direction != Direction.Down)
					{
						Settings.Direction = Direction.Up;
					}
					break;
				case Keys.Down:
					if (Settings.Direction != Direction.Up)
					{
						Settings.Direction = Direction.Down;
					}
					break;
				case Keys.Left:
					if (Settings.Direction != Direction.Right)
					{
						Settings.Direction = Direction.Left;
					}
					break;
				case Keys.Right:
					if (Settings.Direction != Direction.Left)
					{
						Settings.Direction = Direction.Right;
					}
					break;
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
		}

		readonly Random random = new Random();
	}
}
