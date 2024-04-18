using System;
using System.Windows;
using LangLang.DAO;
using LangLang.DAO.JsonDao;
using LangLang.HostBuilders;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.EntityServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UserServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.View;
using LangLang.View.Factories;
using LangLang.ViewModel;
using LangLang.ViewModel.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang
{
    public partial class App : Application
    {
        private readonly IHost _host = CreateHostBuilder().Build();

        private static IHostBuilder CreateHostBuilder(string[]? args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddDao()
                .AddServices()
                .AddStores()
                .AddViewModels()
                .AddWindows();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            Window window = _host.Services.GetRequiredService<LoginWindow>();
            window.Show();
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
            base.OnExit(e);
        }
    }
}
