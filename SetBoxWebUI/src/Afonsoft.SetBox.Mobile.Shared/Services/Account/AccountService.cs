﻿using System.Threading.Tasks;
using Abp.Dependency;
using Acr.UserDialogs;
using Afonsoft.SetBox.ApiClient;
using Afonsoft.SetBox.ApiClient.Models;
using Afonsoft.SetBox.Core.Threading;
using Afonsoft.SetBox.Localization;
using Afonsoft.SetBox.Services.Navigation;
using Afonsoft.SetBox.Services.Storage;
using Afonsoft.SetBox.Sessions;
using Afonsoft.SetBox.Sessions.Dto;
using Afonsoft.SetBox.ViewModels.Base;
using Afonsoft.SetBox.Views;

namespace Afonsoft.SetBox.Services.Account
{
    public class AccountService : IAccountService, ISingletonDependency
    {
        private readonly IApplicationContext _applicationContext;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly INavigationService _navigationService;
        private readonly IDataStorageService _dataStorageService;

        public AccountService(
            IApplicationContext applicationContext,
            ISessionAppService sessionAppService,
            IAccessTokenManager accessTokenManager,
            INavigationService navigationService,
            AbpAuthenticateModel abpAuthenticateModel,
            IDataStorageService dataStorageService)
        {
            _applicationContext = applicationContext;
            _sessionAppService = sessionAppService;
            _accessTokenManager = accessTokenManager;
            _navigationService = navigationService;
            _dataStorageService = dataStorageService;
            AbpAuthenticateModel = abpAuthenticateModel;
        }

        public AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        public AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        public async Task LoginUserAsync()
        {
            await WebRequestExecuter.Execute(_accessTokenManager.LoginAsync, AuthenticateSucceed, ex => Task.CompletedTask);
        }

        public async Task LogoutAsync()
        {
            _accessTokenManager.Logout();
            _applicationContext.ClearLoginInfo();
            _dataStorageService.ClearSessionPersistance();
            await GoToLoginPageAsync();
        }

        private async Task GoToLoginPageAsync()
        {
            await _navigationService.SetDetailPageAsync(typeof(LoginView));
        }

        private async Task AuthenticateSucceed(AbpAuthenticateResultModel result)
        {
            AuthenticateResultModel = result;

            if (AuthenticateResultModel.ShouldResetPassword)
            {
                await UserDialogs.Instance.AlertAsync(L.Localize("ChangePasswordToLogin"), L.Localize("LoginFailed"), L.Localize("Ok"));
                return;
            }

            if (AuthenticateResultModel.RequiresTwoFactorVerification)
            {
                await _navigationService.SetMainPage<SendTwoFactorCodeView>(AuthenticateResultModel);
                return;
            }

            if (!AbpAuthenticateModel.IsTwoFactorVerification)
            {
                await _dataStorageService.StoreAuthenticateResultAsync(AuthenticateResultModel);
            }

            AbpAuthenticateModel.Password = null;
            await SetCurrentUserInfoAsync();
            await UserConfigurationManager.GetAsync();
            await _navigationService.SetMainPage<MainView>(clearNavigationHistory: true);
        }

        private async Task SetCurrentUserInfoAsync()
        {
            await WebRequestExecuter.Execute(async () =>
                await _sessionAppService.GetCurrentLoginInformations(), GetCurrentUserInfoExecuted);
        }

        private async Task GetCurrentUserInfoExecuted(GetCurrentLoginInformationsOutput result)
        {
            _applicationContext.SetLoginInfo(result);

            await _dataStorageService.StoreLoginInformationAsync(_applicationContext.LoginInfo);
        }
    }
}