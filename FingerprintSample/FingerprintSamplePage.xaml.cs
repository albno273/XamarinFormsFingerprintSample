﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Forms;

namespace FingerprintSample
{
    public partial class FingerprintSamplePage : ContentPage
    {
        public FingerprintSamplePage()
        {
            Title = "FingerprintSamplePage";
            InitializeComponent();
        }

        private CancellationTokenSource _cancel;

        private async void OnAuthenticate(object sender, EventArgs e)
        {
            _cancel = swAutoCancel.IsToggled ? new CancellationTokenSource(TimeSpan.FromSeconds(10)) : new CancellationTokenSource();
            lblStatus.Text = "";

            if (await Plugin.Fingerprint.CrossFingerprint.Current.IsAvailableAsync())
            {
                var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync("指を置いてね！", _cancel.Token);
                await SetResultAsync(result);
            }
            else
            {
                await DisplayAlert("注意", "指紋認証機能は使用できません！", "OK");
            }
        }

        private async Task SetResultAsync(FingerprintAuthenticationResult result)
        {
            if (result.Authenticated)
            {
                await Navigation.PushAsync(new FingerprintAuthenticatedPage());
            }
            else
            {
                lblStatus.Text = $"{result.Status}: {result.ErrorMessage}";
            }
        }
    }
}
