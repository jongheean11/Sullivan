using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sample2 : Page
    {
        public Sample2()
        {
            this.InitializeComponent();
        }

        private void btnBefore_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = new Frame();
            string[] para = { "aaa", "bbb" };
            frame.Navigate(typeof(Storage), para);
            Window.Current.Content = frame;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = new Frame();
            string[] para = { "aaa", "bbb" };
            frame.Navigate(typeof(Sample1), para);
            Window.Current.Content = frame;
        }

        private void LeftOut_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            med_W.Play();
        }

        private void LeftOut_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            med_W.Play();
        }

        private void LeftOut_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            med_W.Play();
        }

        private void LeftOut_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            med_W.Stop();
        }

        private void RightOut_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            med_E.Play();
        }

        private void RightOut_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            med_E.Play();
        }

        private void RightOut_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            med_E.Play();
        }

        private void RightOut_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            med_E.Stop();
        }

        private void A2_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            med_A.Play();
        }

        private void A2_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            med_A.Play();
        }

        private void A2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            med_A.Play();
        }

        private void A2_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            med_A.Stop();
        }

        private void I2_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            med_I.Play();
        }

        private void I2_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            med_I.Play();
        }

        private void I2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            med_I.Play();
        }

        private void I2_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            med_I.Stop();
        }

        private void InPath_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            med_Africa.Play();
        }

        private void InPath_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            med_Africa.Play();
        }

        private void InPath_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            med_Africa.Play();
        }

        private void InPath_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            med_Africa.Stop();
        }

        private void OutPath_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            med_Do.Play();
        }

        private void OutPath_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            med_Do.Play();
        }

        private void OutPath_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            med_Do.Play();
        }

        private void OutPath_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            med_Do.Stop();
        }

        private void OutPath_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void OutPath_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void OutPath_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void OutPath_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void InPath_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void InPath_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void InPath_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void InPath_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {

        }

        private void M2_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            med_M.Play();
        }

        private void M2_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            med_M.Stop();
        }

        private void M2_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            med_M.Play();
        }

        private void M2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            med_M.Play();
        }

      }
}