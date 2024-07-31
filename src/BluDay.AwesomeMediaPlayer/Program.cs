﻿/*
Awesome Media Player

https://github.com/BluDay/awesome-media-player-winui3

MIT License

Copyright (c) 2024 BluDay

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Services
    .AddScoped(_ => DispatcherQueue.GetForCurrentThread())
    .AddScoped<ShellViewModel>()
    .AddScoped<MainViewModel>()
    .AddScoped<SettingsViewModel>();

builder.Services
    .AddSingleton<App>()
    .AddSingleton<ResourceLoader>()
    .AddSingleton(WeakReferenceMessenger.Default);

builder.Services
    .AddTransient<Shell>()
    .AddTransient<MainView>()
    .AddTransient<SettingsView>();

IHost host = builder.Build();

host.Start();
host.CreateWinui3App<App>();