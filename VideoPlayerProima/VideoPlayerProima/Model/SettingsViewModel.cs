﻿using System;
using System.Collections.Generic;
using System.Text;
using VideoPlayerProima.Helpers;

namespace VideoPlayerProima.Model
{
    public class SettingsViewModel :  BaseViewModel
    {
        private string _License ;
        private string _PathFiles;
        private bool _ShowVideo ;
        private bool _ShowPhoto = false;
        private bool _ShowWebImage = false;
        private bool _ShowWebVideo = false;
        private bool _EnableTransactionTime = false;
        private int _TransactionTime;

        private string _Company = "Art Vision Indoor";
        private string _Telephone = "(13) 9817-76786";
        private string _Email = "artvisionindoor@gmail.com";

        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                SetProperty(ref _Email, value);
            }
        }

        public string Company
        {
            get
            {
                return this._Company;
            }
            set
            {
                SetProperty(ref _Company, value);
            }
        }

        public string Telephone
        {
            get
            {
                return this._Telephone;
            }
            set
            {
                SetProperty(ref _Telephone, value);
            }
        }


        public SettingsViewModel()
        {
            _License = PlayerSettings.License;
            _PathFiles = PlayerSettings.PathFiles;
            _ShowVideo = PlayerSettings.ShowVideo;
            _ShowPhoto = PlayerSettings.ShowPhoto;
            _ShowWebImage = PlayerSettings.ShowWebImage;
            _ShowWebVideo = PlayerSettings.ShowWebVideo;
            _EnableTransactionTime = PlayerSettings.EnableTransactionTime;
            _TransactionTime = PlayerSettings.TransactionTime;

        }

        public string License
        {
            get
            {
                return this._License;
            }
            set
            {
                SetProperty(ref _License, value);
            }
        }
        public string PathFiles
        {
            get
            {
                return this._PathFiles;
            }
            set
            {
                SetProperty(ref _PathFiles, value);
            }
        }
        public bool ShowVideo
        {
            get
            {
                return this._ShowVideo;
            }
            set
            {
                SetProperty(ref _ShowVideo, value);
            }
        }
        public bool ShowPhoto
        {
            get
            {
                return this._ShowPhoto;
            }
            set
            {
                SetProperty(ref _ShowPhoto, value);
            }
        }
        public bool ShowWebImage
        {
            get
            {
                return this._ShowWebImage;
            }
            set
            {
                SetProperty(ref _ShowWebImage, value);
            }
        }
        public bool ShowWebVideo
        {
            get
            {
                return this._ShowWebVideo;
            }
            set
            {
                SetProperty(ref _ShowWebVideo, value);
            }
        }
        public bool EnableTransactionTime
        {
            get
            {
                return this._EnableTransactionTime;
            }
            set
            {
                SetProperty(ref _EnableTransactionTime, value);
            }
        }
        public int TransactionTime
        {
            get
            {
                return this._TransactionTime;
            }
            set
            {
                SetProperty(ref _TransactionTime, value);
            }
        }
    }
}