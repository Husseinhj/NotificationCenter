using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NotificationCenter.Test.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Action(object o)
        {
            if (o != null)
            {
                if (o is List<object>)
                {
                    foreach (var o1 in (List<object>)o)
                    {
                        if (int.TryParse(o1.ToString(), out var counter))
                        {
                            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                            {
                                LblMessage.Text = $"New message '{counter}' was received.";
                            });
                        }
                    }
                }
                else
                {
                    if (int.TryParse(o.ToString(), out var counter))
                    {
                        await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                        {
                            LblMessage.Text = $"New message '{counter}' was received.";
                        });
                    }
                }
            }
        }

        private void BtnSubscribe_OnClick(object sender, RoutedEventArgs e)
        {
            NotificationCenter.Subscribe("newMessage", Action);
        }

        private void BtnSubscribeActionTwo_OnClick(object sender, RoutedEventArgs e)
        {
            NotificationCenter.Subscribe("newMessage", Action2);
        }

        private void Action2(object o)
        {
            if (o != null)
            {
                Debug.WriteLine("Action two run with data {0}", o);
            }
        }
    }
}
