using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace MemoryMatchingGame
{
    public partial class GameWindow : Form
    {
        //Variables
        int score1 = 0;
        int score2 = 0;
        string player1 = "";
        string player2 = "";
        Random location = new Random(); 
        List<Point> points = new List<Point>();
        PictureBox PendingImage1; 
        PictureBox PendingImage2;
        bool twoplayer = false;
        int playersturn = 1;
        int playersturns = 0;
        static void addToBase1(string player1,int score1)
        { 
            //povezivanje na bazu
            using (SQLiteConnection myConnection = new SQLiteConnection("data source = highscore.db"))
            {
                myConnection.Open();

                using (SQLiteCommand myCommand = new SQLiteCommand(myConnection))
                {
                    //dodavanje u bazu
                    myCommand.CommandText = "INSERT INTO hsone (ime,score) " +
                        "VALUES ('" + player1 + "' , '" + score1 + "'); ";
                    myCommand.ExecuteNonQuery();

                    myCommand.Dispose();
                }
                myConnection.Close();
            }
        }
        static void addToBase2(string player1,string player2,int score1,int score2)
        {
            //dodavanje u bazu, ali za dva igrača
            if (score1 > score2)
            {
                //dodavanje za player1
                using (SQLiteConnection myConnection = new SQLiteConnection("data source = highscore.db"))
                {
                    myConnection.Open();

                    using (SQLiteCommand myCommand = new SQLiteCommand(myConnection))
                    {
                        myCommand.CommandText = "INSERT INTO hstwo (ime,score) " +
                            "VALUES ('" + player1 + "' , '" + score1 + "'); ";
                        myCommand.ExecuteNonQuery();

                        myCommand.Dispose();
                    }
                    myConnection.Close();
                }
            }
            else
            {
                //dodavanje za player 2 
                using (SQLiteConnection myConnection = new SQLiteConnection("data source = highscore.db"))
                {
                    myConnection.Open();

                    using (SQLiteCommand myCommand = new SQLiteCommand(myConnection))
                    {
                        myCommand.CommandText = "INSERT INTO hstwo (ime,score) " +
                            "VALUES ('" + player2 + "' , '" + score2 + "'); ";
                        myCommand.ExecuteNonQuery();

                        myCommand.Dispose();
                    }
                    myConnection.Close();
                }
            }
        }
        public GameWindow()
        {
            InitializeComponent();
        }

        private void GameWindow_Load(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //za privremeni prikaza slika
            timer1.Stop();
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                picture.Enabled = true;
                picture.Cursor = Cursors.Hand;
                picture.Image = Properties.Resources.Cover;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //dužinu prikaza
            int timer = Convert.ToInt32(label1.Text);
            timer = timer-1;
            label1.Text = Convert.ToString(timer);
            if (timer == 0)
            {
                timer2.Stop();
                label1.Visible = false;
            }
        }


        #region Poklapanje slika(da li je pravilan izbor)
        private void Card1_Click(object sender, EventArgs e)
        { 
            Card1.Image = Properties.Resources.Card1;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card1;
                Card1.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card1;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                //provjerava da li se poklapaju
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    //vraća ih na nulu
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card1.Enabled = false;
                    DupCard1.Enabled = false;
                    //dodavanje poena u bazu
                    if (twoplayer == true) {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1,player2,score1,score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1,player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                //provjerava koji je igrač na potezu, ako nisu stisnute dvije slike
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }
        //ponavlja se za sve
        private void DupCard1_Click(object sender, EventArgs e)
        {
            DupCard1.Image = Properties.Resources.Card1;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard1;
                DupCard1.Enabled = false; 
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard1;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card1.Enabled = false;
                    DupCard1.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card2_Click(object sender, EventArgs e)
        {
            Card2.Image = Properties.Resources.Card2;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card2;
                Card2.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card2;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card2.Enabled = false;
                    DupCard2.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);

                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard2_Click(object sender, EventArgs e)
        {
            DupCard2.Image = Properties.Resources.Card2;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard2;
                DupCard2.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard2;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card2.Enabled = false;
                    DupCard2.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card3_Click(object sender, EventArgs e)
        {
            Card3.Image = Properties.Resources.Card3;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card3;
                Card3.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card3;            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card3.Enabled = false;
                    DupCard3.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard3_Click(object sender, EventArgs e)
        {
            DupCard3.Image = Properties.Resources.Card3;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard3;
                DupCard3.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard3;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card3.Enabled = false;
                    DupCard3.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card4_Click(object sender, EventArgs e)
        {
            Card4.Image = Properties.Resources.Card4;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card4;
                Card4.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card4;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card4.Enabled = false;
                    DupCard4.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard4_Click(object sender, EventArgs e)
        {
            DupCard4.Image = Properties.Resources.Card4;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard4;
                DupCard4.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard4;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card4.Enabled = false;
                    DupCard4.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card5_Click(object sender, EventArgs e)
        {
            Card5.Image = Properties.Resources.Card5;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card5;
                Card5.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card5;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card5.Enabled = false;
                    DupCard5.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard5_Click(object sender, EventArgs e)
        {
            DupCard5.Image = Properties.Resources.Card5;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard5;
                DupCard5.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard5;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card5.Enabled = false;
                    DupCard5.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card6_Click(object sender, EventArgs e)
        {
            Card6.Image = Properties.Resources.Card6;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card6;
                Card6.Enabled = false; 
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card6;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card6.Enabled = false;
                    DupCard6.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard6_Click(object sender, EventArgs e)
        {
            DupCard6.Image = Properties.Resources.Card6;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard6;
                DupCard6.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard6;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card6.Enabled = false;
                    DupCard6.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card7_Click(object sender, EventArgs e)
        {
            Card7.Image = Properties.Resources.Card7;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card7;
                Card7.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card7;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card7.Enabled = false;
                    DupCard7.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard7_Click(object sender, EventArgs e)
        {
            DupCard7.Image = Properties.Resources.Card7;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard7;
                DupCard7.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard7;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card7.Enabled = false;
                    DupCard7.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card8_Click(object sender, EventArgs e)
        {
            Card8.Image = Properties.Resources.Card8;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card8;
                Card8.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card8;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card8.Enabled = false;
                    DupCard8.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard8_Click(object sender, EventArgs e)
        {
            DupCard8.Image = Properties.Resources.Card8;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard8;
                DupCard8.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard8;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card8.Enabled = false;
                    DupCard8.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card9_Click(object sender, EventArgs e)
        {
            Card9.Image = Properties.Resources.Card9;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card9;
                Card9.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card9;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card9.Enabled = false;
                    DupCard9.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard9_Click(object sender, EventArgs e)
        {
            DupCard9.Image = Properties.Resources.Card9;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard9;
                DupCard9.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard9;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card9.Enabled = false;
                    DupCard9.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card10_Click(object sender, EventArgs e)
        {
            Card10.Image = Properties.Resources.Card10;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card10;
                Card10.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card10;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card10.Enabled = false;
                    DupCard10.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard10_Click(object sender, EventArgs e)
        {
            DupCard10.Image = Properties.Resources.Card10;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard10;
                DupCard10.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard10;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card10.Enabled = false;
                    DupCard10.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }
        
        private void Card11_Click(object sender, EventArgs e)
        {
            Card11.Image = Properties.Resources.Card11;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card11;
                Card11.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card11;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card11.Enabled = false;
                    DupCard11.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard11_Click(object sender, EventArgs e)
        {
            DupCard11.Image = Properties.Resources.Card11;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard11;
                DupCard11.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard11;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card11.Enabled = false;
                    DupCard11.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void Card12_Click(object sender, EventArgs e)
        {
            Card12.Image = Properties.Resources.Card12;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card12;
                Card12.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = Card12;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card12.Enabled = false;
                    DupCard12.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }

        private void DupCard12_Click(object sender, EventArgs e)
        {
            DupCard12.Image = Properties.Resources.Card12;
            if (PendingImage1 == null)
            {
                PendingImage1 = DupCard12;
                DupCard12.Enabled = false;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                Card1.Enabled = true;
                DupCard1.Enabled = true;
                Card2.Enabled = true;
                DupCard2.Enabled = true;
                Card3.Enabled = true;
                DupCard3.Enabled = true;
                Card4.Enabled = true;
                DupCard4.Enabled = true;
                Card5.Enabled = true;
                DupCard5.Enabled = true;
                Card6.Enabled = true;
                DupCard6.Enabled = true;
                Card7.Enabled = true;
                DupCard7.Enabled = true;
                Card8.Enabled = true;
                DupCard8.Enabled = true;
                Card9.Enabled = true;
                DupCard9.Enabled = true;
                Card10.Enabled = true;
                DupCard10.Enabled = true;
                Card11.Enabled = true;
                DupCard11.Enabled = true;
                Card12.Enabled = true;
                DupCard12.Enabled = true;
                PendingImage2 = DupCard12;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Card12.Enabled = false;
                    DupCard12.Enabled = false;
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                        else
                        {
                            Score2counter.Text = Convert.ToString(Convert.ToInt32(Score2counter.Text) + 1);
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            score2 = Convert.ToInt32(Score2counter.Text);
                            player1 = ScoreLabel.Text;
                            player2 = Score2label.Text;
                            if ((score1 + score2) == 12)
                            {
                                addToBase2(player1, player2, score1, score2);
                            }
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        playersturns++;
                        if (playersturns == 12)
                        {
                            score1 = Convert.ToInt32(ScoreCounter.Text);
                            player1 = ScoreLabel.Text;
                            addToBase1(player1, score1);
                        }
                    }
                }
                else
                {
                    if (twoplayer == true)
                    {
                        if (playersturn == 1)
                        {
                            playersturn = 2;
                            timer3.Start();
                        }
                        else
                        {
                            playersturn = 1;
                            timer3.Start();
                        }
                    }
                    else
                    {
                        ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 1);
                        timer3.Start();
                    }
                }
            }
        }
        #endregion
        //za cover.png, koliko dugo će bit prikazan
        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            PendingImage1.Image = Properties.Resources.Cover;
            PendingImage2.Image = Properties.Resources.Cover;
            PendingImage1 = null;
            PendingImage2 = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void ScoreLabel_Click(object sender, EventArgs e)
        {

        }
        //dva playera
        public void button2_Click(object sender, EventArgs e)
        {
            IntroLabel.Visible = false;
            abLabel.Visible = false;
            _2Players _2players= new _2Players(this);
            _2players.Show();
            ScoreCounter.Text = "0";
            Score2counter.Text = "0";
            twoplayer = true;
            label1.Text = "3";
            NGameButton.Visible = true;
            TwoPlayerButton.Visible = false;
            OnePlayerButton.Visible = false;
            hsButton.Visible = false;
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                picture.Enabled = false;
                points.Add(picture.Location);
            }
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                int next = location.Next(points.Count);
                Point p = points[next];
                picture.Location = p;
                points.Remove(p);
            }

            timer2.Start();
            timer1.Start();
            //daje vrijednosti za PictureBox-ove
            Card1.Image = Properties.Resources.Card1;
            DupCard1.Image = Properties.Resources.Card1;
            Card2.Image = Properties.Resources.Card2;
            DupCard2.Image = Properties.Resources.Card2;
            Card3.Image = Properties.Resources.Card3;
            DupCard3.Image = Properties.Resources.Card3;
            Card4.Image = Properties.Resources.Card4;
            DupCard4.Image = Properties.Resources.Card4;
            Card5.Image = Properties.Resources.Card5;
            DupCard5.Image = Properties.Resources.Card5;
            Card6.Image = Properties.Resources.Card6;
            DupCard6.Image = Properties.Resources.Card6;
            Card7.Image = Properties.Resources.Card7;
            DupCard7.Image = Properties.Resources.Card7;
            Card8.Image = Properties.Resources.Card8;
            DupCard8.Image = Properties.Resources.Card8;
            Card9.Image = Properties.Resources.Card9;
            DupCard9.Image = Properties.Resources.Card9;
            Card10.Image = Properties.Resources.Card10;
            DupCard10.Image = Properties.Resources.Card10;
            Card11.Image = Properties.Resources.Card11;
            DupCard11.Image = Properties.Resources.Card11;
            Card12.Image = Properties.Resources.Card12;
            DupCard12.Image = Properties.Resources.Card12;
        }
        //za jednog igrača
        private void OnePlayerButton_Click(object sender, EventArgs e)
        {
            IntroLabel.Enabled = false;
            abLabel.Enabled = false;
            IntroLabel.Visible = false;
            abLabel.Visible = false;
            OnePlayer OnePlayer = new OnePlayer(this);
            OnePlayer.Show();
            ScoreCounter.Text = "0";
            label1.Text = "3";
            ScoreLabel.Visible = true;
            ScoreCounter.Visible = true;
            NGameButton.Visible = true;
            TwoPlayerButton.Visible = false;
            OnePlayerButton.Visible = false;
            hsButton.Visible = false;
            //disable cover
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                picture.Enabled = false;
                points.Add(picture.Location);
            }
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                int next = location.Next(points.Count);
                Point p = points[next];
                picture.Location = p;
                points.Remove(p);
            }

            timer2.Start();
            timer1.Start();
            Card1.Image = Properties.Resources.Card1;
            DupCard1.Image = Properties.Resources.Card1;
            Card2.Image = Properties.Resources.Card2;
            DupCard2.Image = Properties.Resources.Card2;
            Card3.Image = Properties.Resources.Card3;
            DupCard3.Image = Properties.Resources.Card3;
            Card4.Image = Properties.Resources.Card4;
            DupCard4.Image = Properties.Resources.Card4;
            Card5.Image = Properties.Resources.Card5;
            DupCard5.Image = Properties.Resources.Card5;
            Card6.Image = Properties.Resources.Card6;
            DupCard6.Image = Properties.Resources.Card6;
            Card7.Image = Properties.Resources.Card7;
            DupCard7.Image = Properties.Resources.Card7;
            Card8.Image = Properties.Resources.Card8;
            DupCard8.Image = Properties.Resources.Card8;
            Card9.Image = Properties.Resources.Card9;
            DupCard9.Image = Properties.Resources.Card9;
            Card10.Image = Properties.Resources.Card10;
            DupCard10.Image = Properties.Resources.Card10;
            Card11.Image = Properties.Resources.Card11;
            DupCard11.Image = Properties.Resources.Card11;
            Card12.Image = Properties.Resources.Card12;
            DupCard12.Image = Properties.Resources.Card12;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About About = new About();
            About.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help Help = new Help();
            Help.Show();
        }
        //za highscore
        private void hsButton_Click(object sender, EventArgs e)
        {
            HighScore HighScore = new HighScore();
            HighScore.Show();
        }

        private void IntroLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
