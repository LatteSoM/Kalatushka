using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KalatushkaV2
{
    internal class sldr: MainWindow
    {
        Thread potok1 = new Thread(trf1);

        private static void trf1()
        {

        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider.Maximum = media.NaturalDuration.TimeSpan.Ticks;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(slider.Value));
        }

    }
}
