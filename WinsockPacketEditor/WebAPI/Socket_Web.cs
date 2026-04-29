using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web.Http;

namespace WinsockPacketEditor
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
                    var currentPath = context.Request.Path.Value;

                    //无需认证的路径，直接放行
                    bool isPublicPath = currentPath.StartsWith("/ProxyCap/", StringComparison.OrdinalIgnoreCase);
                    if (isPublicPath)
                    {
                        await next.Invoke();
                        return;
                    }

                    //需要认证的路径，进行 HTTP Basic Authentication 验证
                    var authHeader = context.Request.Headers["Authorization"];

                    if (authHeader != null && authHeader.StartsWith("Basic"))
                    {
                        var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

                        var encoding = Encoding.GetEncoding("iso-8859-1");
                        var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                        var username = usernamePassword.Split(':')[0];
                        var password = usernamePassword.Split(':')[1];

                        if (Operate.ProxyConfig.Account.IsValidAdmin(username, password))
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
                            string sReturn = CCProxy_Controller.QueryUserAll();

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
                                IsAdd = Operate.SystemConfig.StringToBool(body["add"].ToString());
                            }

                            bool IsDel = false;
                            if (body["delete"] != null)
                            {
                                IsDel = Operate.SystemConfig.StringToBool(body["delete"].ToString());
                            }

                            bool IsEdit = false;
                            if (body["edit"] != null)
                            {
                                IsEdit = Operate.SystemConfig.StringToBool(body["edit"].ToString());
                            }

                            AccountInfo pai = new AccountInfo();

                            if (body["enable"] != null)
                            {
                                pai.IsEnable = Operate.SystemConfig.StringToBool(body["enable"].ToString());
                            }

                            if (body["username"] != null)
                            {
                                pai.UserName = body["username"].ToString();
                            }

                            if (body["password"] != null)
                            {
                                pai.Password = body["password"].ToString();
                            }

                            if (body["autodisable"] != null)
                            {
                                pai.IsExpiry = Operate.SystemConfig.StringToBool(body["autodisable"].ToString());
                            }

                            if (body["disabledate"] != null && body["disabletime"] != null)
                            {
                                pai.ExpiryTime = Operate.SystemConfig.StringToDateTime(body["disabledate"].ToString(), body["disabletime"].ToString());
                            }

                            if (IsAdd)
                            {
                                if (CCProxy_Controller.AddUser(pai))
                                {
                                    await context.Response.WriteAsync("1");
                                }
                            }

                            if (IsDel)
                            {
                                if (body["userid"] != null)
                                {
                                    string UserName = body["userid"].ToString();
                                    if (CCProxy_Controller.DelUser(UserName))
                                    {
                                        await context.Response.WriteAsync("1");
                                    }
                                }
                            }

                            if (IsEdit)
                            {
                                if (CCProxy_Controller.UserUpdate(pai))
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
                Operate.DoLog(nameof(Configuration), ex);
            }
        }
    }
}
