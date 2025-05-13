using Owin;
using System.Web.Http;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.FileSystems;
using System.Net;
using System.Security.Principal;
using System.Text;
using System;
using System.Reflection;
using Microsoft.Owin;

namespace WPELibrary.Lib
{
    public class Socket_Web
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                #region//HTTP Basic Authentication

                app.Use(async (context, next) =>
                {
                    var authHeader = context.Request.Headers["Authorization"];

                    if (authHeader != null && authHeader.StartsWith("Basic"))
                    {
                        var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

                        var encoding = Encoding.GetEncoding("iso-8859-1");
                        var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                        var username = usernamePassword.Split(':')[0];
                        var password = usernamePassword.Split(':')[1];

                        if (Socket_Cache.ProxyAccount.IsValidAdmin(username, password))
                        {
                            var principal = new GenericPrincipal(new GenericIdentity(username), null);
                            context.Request.User = principal;

                            await next.Invoke();

                            return;
                        }
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Headers.Add("WWW-Authenticate", new[] { "Basic realm=\"MyApp\"" });
                });

                #endregion

                #region//Web API

                var config = new HttpConfiguration();

                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                app.UseWebApi(config);

                #endregion

                #region//默认文档

                var defaultFileOptions = new DefaultFilesOptions
                {
                    DefaultFileNames = new[] { "index.html" }
                };

                app.UseDefaultFiles(defaultFileOptions);

                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == new PathString("/"))
                    {
                        context.Response.Redirect("/index.html");
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });

                #endregion

                #region//静态文件

                var staticFileOptions = new StaticFileOptions
                {
                    FileSystem = new PhysicalFileSystem(@".\Web")
                };

                app.UseStaticFiles(staticFileOptions);

                #endregion
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
