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
    public partial class OnePlayer : Form
    {
        GameWindow source;
        string playername = "";
        public OnePlayer(GameWindow OnePlayerButton)
        {
            source = OnePlayerButton;
            InitializeComponent();
        }

        public void okButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                playername = textBox1.Text;
                source.ScoreLabel.Text = playername;
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
