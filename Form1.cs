using System;
using System.Drawing;
using System.Windows.Forms;

namespace LittleTetris
{  
    public partial class TetrisForm : Form
    {
        Graphics graphics;
        private Bitmap gameField;       
        private bool pause = false;
              
        public TetrisForm()
        {
            InitializeComponent();        
            gameField = new Bitmap(Constants.CELL_PIXEL_SIZE * (Constants.CELL_COUNT_X + 1), Constants.CELL_PIXEL_SIZE * (Constants.CELL_COUNT_Y + 1));
            graphics = Graphics.FromImage(gameField);
            SoundMaster.ContinuePlaying();
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            if (!pause)
            {
                LineChecker.IsTooHigh();
                LineChecker.FindFilledLines();          
                GameModel.figure.MoveDown();
                UpdateData();
                FillField();          
            }
        }

        private void FillField()
        {            
            graphics.Clear(Color.Black);
            for (int i = 0; i < Constants.CELL_COUNT_X; i++)
                for (int j = 0; j < Constants.CELL_COUNT_Y; j++)
                    if (GameModel.field[i, j] == true)
                        graphics.FillRectangle(
                            Brushes.Green,
                            i * Constants.CELL_PIXEL_SIZE,
                            j * Constants.CELL_PIXEL_SIZE,
                            Constants.CELL_PIXEL_SIZE - 1,
                            Constants.CELL_PIXEL_SIZE - 1);

            for (int i = 0; i < 4; i++)
            {
                Point cell = GameModel.figure.CellsCoordinates[i];
                graphics.FillRectangle(
                    Brushes.Red,
                    cell.X * Constants.CELL_PIXEL_SIZE,
                    cell.Y * Constants.CELL_PIXEL_SIZE,
                    Constants.CELL_PIXEL_SIZE - 1,
                    Constants.CELL_PIXEL_SIZE - 1);
            }

            GameField.Image = gameField;
        }

        private void UpdateData()
        {
            Score.Text = GameModel.gameScore.ToString();
            Lines.Text = GameModel.destroyedLines.ToString();
        }
      
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.S: TickTimer.Interval = 50; break;
                case Keys.A: GameModel.figure.MoveSide(-1); FillField(); break;
                case Keys.D: GameModel.figure.MoveSide(1); FillField(); break;
                case Keys.W: GameModel.figure.Rotate();  break;
                case Keys.P:
                    pause = !pause;
                    PauseLabel.Visible = pause;
                    if (pause)
                        SoundMaster.MakePause();
                    else
                        SoundMaster.ContinuePlaying();
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e) => TickTimer.Interval = 250;             
    }
}