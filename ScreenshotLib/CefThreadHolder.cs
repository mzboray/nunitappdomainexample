using System;
using System.Threading;
using CefSharp;
using CefSharp.OffScreen;

namespace ScreenshotLib
{
    public sealed class CefThreadHolder
    {
        private readonly ManualResetEventSlim _initEvent, _shutdownEvent;

        private readonly Thread _thread;

        private CefThreadHolder()
        {
            _initEvent = new ManualResetEventSlim(false);
            _shutdownEvent = new ManualResetEventSlim(false);
            _thread = new Thread(RunCef)
            {
                Name = "CefThread",
                IsBackground = true
            };
        }

        public static CefThreadHolder Instance { get; } = new CefThreadHolder();

        public bool IsInitialized
        {
            get => _initEvent.IsSet;
        }

        public void Start()
        {
            if ((_thread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted)
            {
                _thread.Start(null);
                _initEvent.Wait();
            }
        }

        public void Stop(bool waitForExit = false)
        {
            _shutdownEvent.Set();
            if (waitForExit)
            {
                _thread.Join();
            }
        }

        private void RunCef(object state)
        {
            try
            {
                CefInitializeWrapper();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to initialize Cef:{e}");

                if (!_initEvent.IsSet)
                    _initEvent.Set();
            }
        }

        private void CefInitializeWrapper()
        {
            try
            {
                var settings = new CefSettings()
                {
                    LogSeverity = LogSeverity.Disable
                };

                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            }
            finally
            {
                _initEvent.Set();
            }

            _shutdownEvent.Wait();
            Cef.Shutdown();
        }
    }
}
