using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace UntitledProject1
{
	public partial class Window1
	{
		String songText = "";
		int count = 0;
		DispatcherTimer tmr = new DispatcherTimer();
		Boolean isPlaying = false;

		public Window1()
		{
			this.InitializeComponent();
			tmr.Interval = TimeSpan.FromMilliseconds(200);
			tmr.Tick += texter;
			tmr.Start();
			this.Drop += new DragEventHandler(Window1_Drop);
			this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(Window1_MouseDown);
			CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
			me.Loaded += new RoutedEventHandler(me_MediaOpened);
			me.Volume = 100;
			me.LoadedBehavior = MediaState.Manual;
			muteLine.Opacity = 0;
		}

		void me_MediaOpened(object sender, RoutedEventArgs e)
		{
			isPlaying = true;
			me.Play();
		}

		void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			if (me.NaturalDuration.HasTimeSpan)
			{
				double total = me.NaturalDuration.TimeSpan.TotalMilliseconds;
				double pos = me.Position.TotalMilliseconds / total;
				progress.Width = 133 * pos;
			}
		}

		void Window1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		void Window1_Drop(object sender, DragEventArgs e)
		{
			string[] filePathInfo = (string[])e.Data.GetData("FileNameW", true);
			me.Source = new Uri(filePathInfo[0]);
			count = 0;
			songText = "                                                      " + filePathInfo[0];
			if (songText.IndexOf(".mp3") == -1 && songText.IndexOf(".wav") == -1)
			{
				cover.Opacity = 1;
			}
			else cover.Opacity = 0;
		}

		void texter(object sender, EventArgs e)
		{
			if (songText.Length > 3)
			{
				try
				{
					songName.Text = songText.Substring(count);
				}
				catch (ArgumentOutOfRangeException ex)
				{
					MessageBox.Show(ex.Message);
				}
				if (count == songText.Length - 1) count = 0;
				else count++;
			}
		}

		void closer(Object sender, EventArgs e)
		{
			this.Close();
		}

		void playPause(Object sender, EventArgs e)
		{
			if (isPlaying)
			{
				me.Pause();
				isPlaying = false;
			}
			else
			{
				me.Play();
				isPlaying = true;
			}
		}

		void restarter(Object sender, EventArgs e)
		{
			me.Position = new TimeSpan(0, 0, 0);
		}

		void muter(Object sender, EventArgs e)
		{
			if (me.Volume == 0)
			{
				me.Volume = 100;
				muteLine.Opacity = 0;
			}
			else
			{
				me.Volume = 0;
				muteLine.Opacity = 1;
			}
		}

	}
}