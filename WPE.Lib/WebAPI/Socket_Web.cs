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

namespace WPE.Lib.WebAPI
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

                        if (Operate.ProxyConfig.ProxyAccount.IsValidAdmin(username, password))
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

                #region//设置 Web API 路由

                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();               

                app.UseWebApi(config);

                #endregion

                #region//静态文件

                var staticFileOptions = new StaticFileOptions
                {
                    FileSystem = new PhysicalFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web")),
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

                #region//处理 Account 路径

                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == new PathString("/account"))
                    {
                        if (context.Request.Method == "GET")
                        {
                            string sReturn = WebAPI.CCProxy_Controller.QueryUserAll();

                            if (!string.IsNullOrEmpty(sReturn))
                            {
                                context.Response.ContentType = "text/html; charset=utf-8";
                                await context.Response.WriteAsync(sReturn);
                            }
                        }
                        else if (context.Request.Method == "POST")
                        {
                            context.Response.StatusCode = 200;
                            context.Response.ContentType = "application/json";

                            var body = await context.Request.ReadFormAsync();

                            bool IsAdd = false;
                            if (body["add"] != null)
                            {
                                IsAdd = Socket_Operation.StringToBool(body["add"].ToString());
                            }

                            bool IsDel = false;
                            if (body["delete"] != null)
                            {
                                IsDel = Socket_Operation.StringToBool(body["delete"].ToString());
                            }

                            bool IsEdit = false;
                            if (body["edit"] != null)
                            {
                                IsEdit = Socket_Operation.StringToBool(body["edit"].ToString());
                            }

                            Proxy_AccountInfo pai = new Proxy_AccountInfo();

                            if (body["enable"] != null)
                            {
                                pai.IsEnable = Socket_Operation.StringToBool(body["enable"].ToString());
                            }

                            if (body["username"] != null)
                            {
                                pai.UserName = body["username"].ToString();
                            }

                            if (body["password"] != null)
                            {
                                pai.PassWord = body["password"].ToString();
                            }

                            if (body["autodisable"] != null)
                            {
                                pai.IsExpiry = Socket_Operation.StringToBool(body["autodisable"].ToString());
                            }

                            if (body["disabledate"] != null && body["disabletime"] != null)
                            {
                                pai.ExpiryTime = Socket_Operation.StringToDateTime(body["disabledate"].ToString(), body["disabletime"].ToString());
                            }

                            if (IsAdd)
                            {
                                if (WebAPI.CCProxy_Controller.AddUser(pai))
                                {
                                    await context.Response.WriteAsync("1");
                                }
                            }

                            if (IsDel)
                            {
                                if (body["userid"] != null)
                                {
                                    string UserName = body["userid"].ToString();
                                    if (WebAPI.CCProxy_Controller.DelUser(UserName))
                                    {
                                        await context.Response.WriteAsync("1");
                                    }
                                }
                            }

                            if (IsEdit)
                            {
                                if (WebAPI.CCProxy_Controller.UserUpdate(pai))
                                {
                                    await context.Response.WriteAsync("1");
                                }
                            }
                        }                        
                    }
                    else
                    {                        
                        await next();
                    }
                });

                #endregion                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
