using System;
using System.Net;
using System.Reactive.Linq;

namespace WebServerLib
{
    internal class HttpServer : IObservable<RequestContext>, IDisposable
    {
        private readonly HttpListener _listener;
        private readonly IObservable<RequestContext> _stream;

        public HttpServer(string url)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            _listener.Start();
            _stream = ObservableHttpContext();
        }

        private IObservable<RequestContext> ObservableHttpContext()
        {
            return Observable.Create<RequestContext>(obs =>
                      Observable.FromAsyncPattern<HttpListenerContext>(_listener.BeginGetContext,
                                               _listener.EndGetContext)()
                           .Select(c => new RequestContext(c.Request, c.Response))
                           .Subscribe(obs))
                     .Repeat()
                     .Retry()
                     .Publish()
                     .RefCount();
        }

        public void Dispose()
        {
            _listener.Stop();
        }

        public IDisposable Subscribe(IObserver<RequestContext> observer)
        {
            return _stream.Subscribe(observer);
        }
    }
}
