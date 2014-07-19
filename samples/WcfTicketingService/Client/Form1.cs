using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
namespace Client
{
    public partial class Form1 : Form
    {
        int ROWS = 10;
        int COLUMNS = 10;
        const int SEAT_WIDTH = 45;
        const int SEAT_HEIGHT = 25;
        const int START_X = 10;
        const int START_Y = 40;
        static Button[,] seatsArray;
        private ServiceReference1.TicketingServiceClient _client;
        private Guid _guid = Guid.NewGuid();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InstanceContext context =
                new InstanceContext(new SeatStatusCallback());
            _client = new
                ServiceReference1.TicketingServiceClient(context);
            _client.RegisterClient(_guid);

            //---display the seats---
            seatsArray = new Button[COLUMNS, ROWS];
            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < ROWS; c++)
                {
                    Button btn = new Button();
                    btn.Location = new Point(
                        START_X + (SEAT_WIDTH * c),
                        START_Y + (SEAT_HEIGHT * r));

                    btn.Size = new Size(SEAT_WIDTH, SEAT_HEIGHT);
                    btn.Text = (c + 1).ToString() + "-" + (r +
                       1).ToString();
                    btn.BackColor = Color.White;
                    seatsArray[c, r] = btn;
                    btn.Click += new EventHandler(btn_Click);
                    this.Controls.Add(btn);
                }
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.White)
            {
                ((Button)sender).BackColor = Color.Yellow;
            }
            else if (((Button)sender).BackColor == Color.Yellow)
            {
                ((Button)sender).BackColor = Color.White;
            }
        }

        //---set all occupied seats in red---
        public static void SeatsOccupied(string strSeatsOccupied)
        {
            string[] seats = strSeatsOccupied.Split(',');
            for (int i = 0; i < seats.Length - 1; i++)
            {
                string[] xy = seats[i].Split('-');
                Button btn = seatsArray[int.Parse(xy[0]) - 1,
                    int.Parse(xy[1]) - 1];
                btn.BackColor = Color.Red;
            }
        }

        public class SeatStatusCallback :
            ServiceReference1.TicketingServiceCallback
        {
            public void SeatStatus(string message)
            {
                Form1.SeatsOccupied(message);
            }
        }

        private void btnBookSeats_Click(object sender, EventArgs e)
        {
            string seatsToBook = string.Empty;
            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < ROWS; c++)
                {
                    if (seatsArray[c, r].BackColor == Color.Yellow)
                    {
                        seatsToBook += seatsArray[c, r].Text + ",";
                    }
                }
            }

            //---send to WCF service---
            _client.SetSeatStatus(seatsToBook);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _client.UnRegisterClient(_guid);
        }
    }
}
