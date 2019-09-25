﻿using System;
using System.Threading.Tasks;
using VideoPlayerProima.Helpers;
using VideoPlayerProima.Interface;
using VideoPlayerProima.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayerProima
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private IDevicePicker devicePicker;
        private IDirectoyPicker directoyPicker;
        private readonly ILogger log;
        private SettingsViewModel model;
        string deviceIdentifier;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = model = new SettingsViewModel();

            log = DependencyService.Get<ILogger>();
            if (log != null)
            {
                IDevicePicker device = DependencyService.Get<IDevicePicker>();
                log.DeviceIdentifier = device?.GetIdentifier();
                log.Platform = DevicePicker.GetPlatform().ToString();
                log.Version = $"{DevicePicker.GetVersion().Major}.{DevicePicker.GetVersion().Minor}.{DevicePicker.GetVersion().Revision}.{DevicePicker.GetVersion().Build}";
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            directoyPicker = DependencyService.Get<IDirectoyPicker>();
            devicePicker = DependencyService.Get<IDevicePicker>();
            deviceIdentifier = devicePicker?.GetIdentifier();
            LicenseID.Detail = "ID: " + deviceIdentifier;

            try
            {
                var api = new API.SetBoxApi(deviceIdentifier, PlayerSettings.License, PlayerSettings.Url);
                var config = await api.GetSupport();
                if (config != null)
                {
                    model.Company = config.company;
                    model.Telephone = config.telephone;
                    model.Email = config.email;
                }
            }
            catch (Exception ex)
            {
                log?.Error("Erro para atualizar o suporte", ex);
            }
        }

        public async void OnButtonSelectClicked(object sender, EventArgs e)
        {
            try
            {
                string path = await directoyPicker.OpenSelectFolderAsync();
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Substring(0, path.LastIndexOf('/') + 1);
                    PlayerSettings.PathFiles = path;
                    model.PathFiles = PlayerSettings.PathFiles;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public async Task ShowMessage(string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }

        public async void OnButtonSalvarClicked(object sender, EventArgs e)
        {
            PlayerSettings.License = model.License;
            PlayerSettings.PathFiles = model.PathFiles;
            PlayerSettings.ShowVideo = model.ShowVideo;
            PlayerSettings.ShowPhoto = model.ShowPhoto;
            PlayerSettings.ShowWebImage = model.ShowWebImage;
            PlayerSettings.ShowWebVideo = model.ShowWebVideo;
            PlayerSettings.EnableTransactionTime = model.EnableTransactionTime;
            PlayerSettings.TransactionTime = model.TransactionTime;

            try
            {
                var api = new API.SetBoxApi(deviceIdentifier, model.License, PlayerSettings.Url);
                log?.Info("Salvando as Configurações no Servidor");
                await api.SetConfig(new ConfigModel()
                {
                    enablePhoto = model.ShowPhoto,
                    enableVideo = model.ShowVideo,
                    enableTransaction = model.EnableTransactionTime,
                    enableWebVideo = model.ShowWebVideo,
                    transactionTime = model.TransactionTime
                });
            }
            catch (Exception ex)
            {
                log?.Error(ex);
            }

            await ShowMessage("Dados Salvos com sucesso!", "Salvar", "OK",
                () => { Application.Current.MainPage = new MainPage(); });
        }

        private void LicenseID_Tapped(object sender, EventArgs e)
        {
            DependencyService.Get<IClipboardService>().SendTextToClipboard(LicenseID.Detail.Replace("ID: ", ""));
            DependencyService.Get<IMessage>().Alert("Licença Copiada para o Clipboard");
        }
    }
}