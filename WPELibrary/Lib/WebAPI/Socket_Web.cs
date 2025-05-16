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
using System.IO;

namespace WPELibrary.Lib.WebAPI
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
                    context.Response.Headers.Add("WWW-Authenticate", new[] { "Basic realm=\"WPE x64\"" });
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

                #region//静态文件

                var staticFileOptions = new StaticFileOptions
                {
                    FileSystem = new PhysicalFileSystem(@".\Web"),
                    ServeUnknownFileTypes = true
                };

                app.UseStaticFiles(staticFileOptions);

                #endregion

                #region//默认文档

                var defaultFileOptions = new DefaultFilesOptions
                {
                    DefaultFileNames = new[] { "index.html" }
                };

                app.UseDefaultFiles(defaultFileOptions);

                #endregion

                #region//处理默认路径

                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == new PathString("/"))
                    {
                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web", "index.html");

                        if (File.Exists(filePath))
                        {
                            context.Response.ContentType = "text/html";
                            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                await fileStream.CopyToAsync(context.Response.Body);
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });

                #endregion

                #region//处理 ProxyAccount 路径

                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == new PathString("/ProxyAccount"))
                    {
                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web", "ProxyAccount.html");

                        if (File.Exists(filePath))
                        {
                            context.Response.ContentType = "text/html";
                            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                await fileStream.CopyToAsync(context.Response.Body);
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });

                #endregion                

                #region//处理 SystemLog 路径

                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == new PathString("/SystemLog"))
                    {
                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web", "SystemLog.html");

                        if (File.Exists(filePath))
                        {
                            context.Response.ContentType = "text/html";
                            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                await fileStream.CopyToAsync(context.Response.Body);
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });

                #endregion                                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
