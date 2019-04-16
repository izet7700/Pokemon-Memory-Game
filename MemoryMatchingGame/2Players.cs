using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryMatchingGame
{
    public partial class _2Players : Form
    {
        GameWindow source;
        int okcount = 1;
        string player1 = "";
        string player2 = "";
        public _2Players(GameWindow TwoPlayerButton)
        {
            source = TwoPlayerButton;
            InitializeComponent();
        }

        public void _2Players_Load(object sender, EventArgs e)
        {

        }

        public void OKbutton_Click(object sender, EventArgs e)
        {
            if (okcount == 1)
            {
                if (playersTextBox.Text != "")
                {
                    player1 = playersTextBox.Text;
                    source.ScoreLabel.Text = player1;
                    source.ScoreLabel.Visible = true;
                    source.ScoreCounter.Visible = true;
                    playersTextBox.Text = "";
                    okcount++;
                }
            }
            else if (okcount == 2)
            {
                if (playersTextBox.Text != "")
                {
                    player2 = playersTextBox.Text;
                    source.Score2label.Text = player2;
                    source.Score2label.Visible = true;
                    source.Score2counter.Visible = true;
                    playersTextBox.Text = "";
                    this.Close();
                }
            }
        }
    }
}
