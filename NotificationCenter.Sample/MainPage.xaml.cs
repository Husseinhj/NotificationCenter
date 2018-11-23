using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotificationCenter.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        #region Event method

        void Subscribe_Clicked(object sender, System.EventArgs e)
        {
            String actionName = txtSubscribeAction.Text;
            if (!String.IsNullOrEmpty(actionName))
            {
                NotificationCenter.Subscribe(actionName, (obj) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        txtLog.Text = txtLog.Text + $"\nAction {actionName} notified. Object is {obj.ToString()}";
                    });
                });
            } 
            else 
            {
                DisplayAlert("Error", "Subscribe action name is null or empty. Please, provide an action name", "OK");
            }
        }

        void Unsubscribe_Clicked(object sender, System.EventArgs e)
        {
            String actionName = txtUnsubscribeAction.Text;
            if (!String.IsNullOrEmpty(actionName))
            {
                NotificationCenter.Unsubscribe(actionName);
                txtLog.Text = txtLog.Text + $"\nUnsubscribe {actionName}";
            }
            else
            {
                DisplayAlert("Error", "Unsubscribe action name is null or empty. Please, provide an action name", "OK");
            }
        }

        async void Notify_Clicked(object sender, System.EventArgs e)
        {
            String actionName = txtNotifyAction.Text;
            if (!String.IsNullOrEmpty(actionName))
            {
                var obj = new {action = actionName};

                await NotificationCenter.Notify(actionName, obj);
                txtLog.Text = txtLog.Text + $"\nNotify action {actionName}";
            }
            else
            {
                await DisplayAlert("Error", "Subscribe action name is null or empty. Please, provide an action name", "OK");
            }
        }

        #endregion
    }
}
