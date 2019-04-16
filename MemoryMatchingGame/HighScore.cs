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
    public partial class HighScore : Form
    {
        //prikaz scorova iz baze
        public HighScore()
        {
            InitializeComponent();
        }
       //u data grid izpiše za jednog playera scorove
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Visible = true;
            SQLiteConnection conn = new SQLiteConnection("data source=highscore.db");
            conn.Open();
            SQLiteCommand comm = new SQLiteCommand("SELECT * FROM hsone ORDER BY score ASC limit 10;", conn);
            using (SQLiteDataReader read = comm.ExecuteReader())
            {
                while (read.Read())
                {
                    dataGridView1.Rows.Add(new object[] {
                    read.GetValue(0),
                    read.GetValue(read.GetOrdinal("score")),
            });
                }
            }

        }
        //za dva playera prikaz scorova
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Visible = true;
            SQLiteConnection conn = new SQLiteConnection("data source=highscore.db");
            conn.Open();
            SQLiteCommand comm = new SQLiteCommand("SELECT * FROM hstwo ORDER BY score DESC limit 10;", conn);
            using (SQLiteDataReader read = comm.ExecuteReader())
            {
                while (read.Read())
                {
                    dataGridView1.Rows.Add(new object[] {
                    read.GetValue(0),
                    read.GetValue(read.GetOrdinal("score")),
            });
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
