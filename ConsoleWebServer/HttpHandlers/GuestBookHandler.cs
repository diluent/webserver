using System;
using ConsoleWebServer.DataModels;
using ConsoleWebServer.Repository;
using ConsoleWebServer.Utils;
using WebServerLib;

namespace ConsoleWebServer.HttpHandlers
{
    class GuestBookHandler
    {
        private static IGuestBookRepositoryFactory _repositoryFactory = new GuestBookRepositoryFactory(new WebConfigSettings());

        public IGuestBookRepositoryFactory RepositoryFactory
        {
            set
            {
                if (value != null)
                    _repositoryFactory = value;
            }
        }

        public static Action<RequestContext> Index = context => {
            context.RespondHtmlTemplate("GuestBook");
        };

        public static Action<RequestContext> Get = context =>
        {
            var rep = _repositoryFactory.GetRepository();

            var guestbook = rep.Select();

            context.RespondJson(guestbook.UserMessages);
        };

        public static Action<RequestContext> Add = context => {
            var model = new Item {
                User = context.PostParam("user").ToString(),
                Message = context.PostParam("message").ToString()
            };
            var rep = _repositoryFactory.GetRepository();

            rep.Insert(model);
            context.RespondHtmlTemplate("GuestBook");
        };
    }
}
