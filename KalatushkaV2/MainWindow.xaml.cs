using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KalatushkaV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int ind = 0;
        public static bool replay = false; /// flse - for non repeat, true - for repeat
        bool randomPlay = false;
        public static List<string> src = new List<string>();
        private Random random = new Random();
        private static bool PlayOrPause = true; /// true - for play, false - for pause
        public MainWindow()
        {
            InitializeComponent();
        }

        private void trf1()
        {
            bool gang = true;
            while (gang)
            {

                Dispatcher.Invoke(() =>
                {
                    slider.Value = Convert.ToDouble(media.Position.Ticks);
                    strt.Text = new TimeSpan(Convert.ToInt64(slider.Value)).ToString(@"mm\:ss");
                    if (Convert.ToDouble(media.Position.Ticks) == slider.Maximum && replay == true)
                    {
                        manipulations(oper.PlayBtn, ind);
                    }
                    else if (Convert.ToDouble(media.Position.Ticks) == slider.Maximum && randomPlay == true)
                    {
                        int col = random.Next(0, src.Count);
                        manipulations(oper.PlayBtn, col);
                    }
                    else if (Convert.ToDouble(media.Position.Ticks) == slider.Maximum)
                    {
                        manipulations(oper.Nxt, ind);
                    }
                });
                Thread.Sleep(100);
            }
        }

        private void file_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var res = dialog.ShowDialog();
            if (res == CommonFileDialogResult.Ok)
            {
                List<string> sort = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(dialog.FileName);
                FileInfo[] Files = dir.GetFiles("*.*"); //Getting Text files
                foreach (FileInfo fil in Files)
                {
                    if (fil.Name.Contains(".mp3") || fil.Name.Contains(".m4a") || fil.Name.Contains(".vav"))
                    {
                        string str = "";
                        str = str + fil.Name;
                        sort.Add(str);
                        src.Add(fil.FullName);
                    }
                }
                spisok.ItemsSource = sort;
                manipulations(oper.defaultValue);


                Thread potok = new Thread(trf1);
                potok.Start();

            }
        }
        private void spisok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ind = spisok.SelectedIndex;
            manipulations(oper.PlayBtn, ind);

        }
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider.Maximum = media.NaturalDuration.TimeSpan.Ticks;
            end.Text = new TimeSpan(Convert.ToInt64(slider.Maximum)).ToString(@"mm\:ss");
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                media.Position = new TimeSpan(Convert.ToInt64(slider.Value));

            }
            catch
            {
                ///kostilus
            }
        }
        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                media.Volume = Math.Round(Convert.ToDouble(volume.Value))/100;
                MessageBox.Show(media.Volume.ToString());
            }
            catch
            {

            }
        }

        private void playPause_Click(object sender, RoutedEventArgs e)
        {
            if (PlayOrPause == false)
            {
                var biba = media.Position.Ticks;
                manipulations(oper.PlayBtn, ind, biba);
            }
            else if (PlayOrPause == true)
            {
                manipulations(oper.Pause);
            }
        }

        private void rep_Click(object sender, RoutedEventArgs e)
        {
            if (replay == false)
            {
                replay = true;
            }
            else if (replay == true)
            {
                replay = false;
            }
        }
        private void rnd_Click(object sender, RoutedEventArgs e)
        {
            if (randomPlay == false)
            {
                int col = random.Next(0, src.Count);
                manipulations(oper.PlayBtn, col);
                randomPlay = true;
            }
            else if (randomPlay == true)
            {
                randomPlay = false;
            }


        }
        private void next_Click(object sender, RoutedEventArgs e)
        {
            manipulations(oper.Nxt, ind);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            manipulations(oper.Lst, ind);
        }

        private void manipulations(oper param, int trekPos = 0, long pos = 0)
        {
            switch (param)
            {
                case oper.defaultValue: /// default 1st sound play when folder is selected
                    media.Source = new Uri(src[0]);
                    media.Play();
                    media.Volume = 1;

                    ind = 0;
                    PlayOrPause = true;
                    break;
                case oper.PlayBtn: /// play when btn click or spidokitem selected
                    media.Source = new Uri(src[trekPos]);
                    media.Play();
                    if (pos != 0)
                    {
                        media.Position = new TimeSpan(pos);
                    }
                    media.Volume = 1;

                    PlayOrPause = true;
                    break;
                case oper.Pause: /// pause
                    media.Pause();

                    PlayOrPause = false;
                    break;
                case oper.Nxt:/// next song
                    media.Source = new Uri(src[trekPos + 1]);
                    media.Play();
                    media.Volume = 1;
                    ind++;
                    PlayOrPause = false;
                    break;
                case oper.Lst:/// back song
                    media.Source = new Uri(src[trekPos - 1]);
                    media.Play();
                    media.Volume = 1;
                    ind--;
                    PlayOrPause = false;
                    break;
            }
        }

    }
}
