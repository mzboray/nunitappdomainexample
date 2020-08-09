using System;
using System.Drawing;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;

namespace ScreenshotLib
{
    public class ScreenshotTaker : IDisposable
    {
        private ChromiumWebBrowser _browser;

        public ScreenshotTaker()
        {
            _browser = new ChromiumWebBrowser();
            _browser.ConsoleMessage += BrowserConsoleMessage;
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
                _browser.LoadingStateChanged += OnLoadingStateChanged;
                _browser.Load(url);
                await loadingFinished.Task;
            }
            finally
            {
                _browser.LoadingStateChanged -= OnLoadingStateChanged;
            }
        }

        public async Task<Bitmap> LoadAndTakeScreenshot(string url, int delay = 0)
        {
            await LoadAsync(url);

            // hide scrollbars
            if (_browser.CanExecuteJavascriptInMainFrame)
                await _browser.EvaluateScriptAsync("document.body.style.overflow = 'hidden'");

            if (delay > 0)
                await Task.Delay(delay);
            return await _browser.ScreenshotAsync();
        }

        public void Dispose()
        {
            _browser.Dispose();
        }

        private async Task WaitForInitiializedAsync()
        {
            if (_browser.IsBrowserInitialized)
                return;

            var initFinished = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            void OnBrowserInitialized(object sender, EventArgs args)
            {
                initFinished.TrySetResult(null);
            }

            try
            {
                _browser.BrowserInitialized += OnBrowserInitialized;
                if (_browser.IsBrowserInitialized)
                    return;

                await initFinished.Task;
            }
            finally
            {
                _browser.BrowserInitialized -= OnBrowserInitialized;
            }
        }

        private void BrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine($"browser console: [{e.Level}] {e.Message}");
        }
    }
}
