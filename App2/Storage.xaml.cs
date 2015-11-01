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
    public sealed partial class Storage : Page
    {
        public Storage()
        {
            this.InitializeComponent();
        }

        private void SampleTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            Frame frame = new Frame();
            frame.Navigate(typeof(Sample2));
            Window.Current.Content = frame;
        }

        
        
            

        private void FantasticBaby(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
         	// TODO: Add event handler implementation here.
            Frame frame = new Frame();
            frame.Navigate(typeof(DrawingPage));
            Window.Current.Content = frame;
        }
            
        
        
    }
}
