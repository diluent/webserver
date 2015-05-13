using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace WebServerLib {

    public interface IWebServer
    {
        void AddRouteAction(IRouteAction routeAction);
        void Start();
        void Stop();
    }

    public sealed class WebServer : IWebServer, IDisposable
    {
        private HttpServer _server;

        private readonly string _url;

        private readonly HashSet<IRouteAction> _routeActions = new HashSet<IRouteAction>();

        private readonly HashSet<IDisposable> _handlers = new HashSet<IDisposable>();

        private readonly ILogWriter _logWriter;

        public WebServer(string url, ILogWriter logWriter) {
            _url = url;
            _logWriter = logWriter;
        }

        public void AddRouteAction(IRouteAction routeAction) {
            _routeActions.Add(routeAction);
        }

        public void Start() {
            _server = new HttpServer(_url);
            foreach (var routeAction in _routeActions) {
                Subscribe(routeAction.RouteValidate, routeAction.Action);
            }
            Subscribe(ErrorValidate, ErrorAction);
            _logWriter.Info("Web server is running");
        }

        private void Subscribe(Func<RequestContext, bool> validate, Action<RequestContext> routeAction)
        {
            _handlers.Add(_server.Where(validate)
                    .Subscribe(ctx =>
                    {
                        try
                        {
                            _logWriter.Info(string.Format(@"Query [path:'{1}'; method:'{0}']", ctx.Request.Method, ctx.Request.Path));
                            routeAction(ctx);
                            ctx.RespondNotFound("Error 404");
                        }
                        catch (Exception e)
                        {
                            _logWriter.Error(e);
                        }
                    }));
        }

        private bool ErrorValidate(RequestContext context)
        {
            return !_routeActions.Any(x => x.RouteValidate(context));
        }

        private void ErrorAction(RequestContext context)
        {
            context.RespondNotFound("Error 404");
        }

        public void Stop() {
            if (_server == null) return;
            foreach (var handler in _handlers.Where(handler => handler != null))
                handler.Dispose();
            _handlers.Clear();
            _server.Dispose();
            _logWriter.Info("Web server stoped");
        }


        public void Dispose() {
            Stop();
        }
    }
}
