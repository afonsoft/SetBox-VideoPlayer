﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Afonsoft.SetBox.Web.Areas.App.Models.Layout;
using Afonsoft.SetBox.Web.Session;
using Afonsoft.SetBox.Web.Views;

namespace Afonsoft.SetBox.Web.Areas.App.Views.Shared.Components.AppTheme2Footer
{
    public class AppTheme2FooterViewComponent : SetBoxViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme2FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
