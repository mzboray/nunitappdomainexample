using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

namespace ScreenshotLib
{
    public class ScreenshotBrowser : ChromiumWebBrowser
    {
        public ScreenshotBrowser()
        {
            ConsoleMessage += BrowserConsoleMessage;
        }

        public async Task WaitForInitiializedAsync()
        {
            if (IsBrowserInitialized)
                return;

            var initFinished = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            void OnBrowserInitialized(object sender, EventArgs args)
            {
                initFinished.TrySetResult(null);
            }

            try
            {
                BrowserInitialized += OnBrowserInitialized;
                if (IsBrowserInitialized)
                    return;

                await initFinished.Task;
            }
            finally
            {
                BrowserInitialized -= OnBrowserInitialized;
            }
        }

        public async Task LoadAsync(string url)
        {
            await WaitForInitiializedAsync();

            var loadingFinished = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
            {
                if (!args.IsLoading)
                {
                    loadingFinished.TrySetResult(null);
                }
            }

            try
            {
                LoadingStateChanged += OnLoadingStateChanged;
                Load(url);
                await loadingFinished.Task;
            }
            finally
            {
                LoadingStateChanged -= OnLoadingStateChanged;
            }
        }

        public async Task<Bitmap> LoadAndTakeScreenshot(string url, int delay = 0)
        {
            await LoadAsync(url);
            if (delay > 0)
                await Task.Delay(delay);
            return await ScreenshotAsync();
        }

        private void BrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine($"browser console: [{e.Level}] {e.Message}");
        }
    }
}
