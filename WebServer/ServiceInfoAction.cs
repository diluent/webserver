using System;

namespace WebServerLib
{

    public enum Method
    {
        ANY,
        GET,
        POST
    }

    public class RouteCollection
    {


        public void MapRoute(string route, Action<RequestContext> handler, Method method = Method.ANY)
        {
            
        }

        public void ErrorRoute(Action<RequestContext> handler)
        {
            
        }
    }



    public interface IRouteAction
    {
        bool RouteValidate(RequestContext request);

        void Action(RequestContext request);
    }

    public class RouteAction : IRouteAction
    {
        private readonly string _route;
        private readonly Action<RequestContext> _handler;
        private readonly Method _method;

        public RouteAction(string route, Action<RequestContext> handler, Method method = Method.ANY)
        {
            _route = route;
            _handler = handler;
            _method = method;
        }

        public bool RouteValidate(RequestContext request)
        {
            return request.Request.Path.Equals(_route) && 
                (_method == Method.ANY || request.Request.Method.ToLower().Equals(_method.ToString().ToLower()));
        }

        public void Action(RequestContext request)
        {
            _handler(request);
        }
    }

    public class RouteError : IRouteAction// : RouteAction
    {
        private readonly Action<RequestContext> _handler;

        public RouteError(Action<RequestContext> handler)
            //: base(null, handler, Method.ANY) 
        {
            _handler = handler;
        }

        public bool RouteValidate(RequestContext request) {
            return true;
        }

        public new void Action(RequestContext request) {
            //if (_route == null)
            _handler(request);
            //base.Action(request);
        }
    }

}
