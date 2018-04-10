using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Extensions.Caching;
using Couchbase.Extensions.Session;

namespace MVCMovie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MVCMovieContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MVCMovieContext")));

            services.AddScoped<MVCMovie.Infrastructure.ILogger, MVCMovie.Infrastructure.Logger_Test_Class_1>();
            
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
                .AddOpenIdConnect(options => {
                    options.Authority = "https://localhost:44331/";    // it should change to localhost current port.
                    options.RequireHttpsMetadata = true;
                    options.ClientId = "movieClient";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("movieapi");
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    options.SignInScheme = "Cookies";
                    options.SaveTokens  = true;
                    options.ClientSecret = "secret";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SignedOutCallbackPath = new PathString("/");
                    options.SecurityTokenValidator = new JwtSecurityTokenHandler
                    {
                        InboundClaimTypeMap = new Dictionary<string, string>()
                    };
                    options.Events = new OpenIdConnectEvents()

                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.HandleResponse();

                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            if (true)
                            {
                                // Debug only, in production do not share exceptions with the remote host.
                                return c.Response.WriteAsync(c.Exception.ToString());
                            }

                        }, 

                        OnTokenValidated = tokenValidatedContext =>
                        {

                            var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;

                            return Task.FromResult(0);
                        },
                        OnUserInformationReceived= userInformationReceivedContext =>
                        {
                            // Add new claims here from system.
                            // user object available here with User details.
                            return Task.FromResult(0);
                        }
                    };
                });


            //services.AddDistributedMemoryCache();
            //services.AddSession();

            //https://dzone.com/articles/distributed-session-with-aspnet-core-and-couchbase
            //https://github.com/brantburnett/Couchbase.Extensions 
            //This is being added through dependecy injection.
            //services.AddCouchbase(options => {

            //    options.Servers = new List<Uri> { new Uri ("https://localhost:44339")}; // list of all distributed serves.

            //});

            ////Need Couchbase Server cluster  and bucket, we will also need to create a user with Data Reader and Data Writer permission on the bucket
            //services.AddDistributedCouchbaseCache("", "", options => { options.LifeSpan = new TimeSpan(100000000); });


            //services.AddCouchbaseSession(opt =>
            //{
            //    opt.CookieName = "sampleCookies";
            //    // all other settings here, these setting can be in configuration and injected based on envoirnment.

            //});

            //services.AddAuthentication(options =>
            //{
            //  options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //  options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;



            //})
            //.AddCookie()
            //.AddOpenIdConnect(options =>
            //{
            //    options.Authority = "";
            //    options.ClientId = "";
            //    options.ClientSecret = "";
            //    options.UseTokenLifetime = false;
            //    //soptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.ResponseType = "id_token token";
            //    options.Scope.Add("openid api");
            //    options.CallbackPath = new PathString("/");
            //    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "given_name",
            //        RoleClaimType = "role"
            //    };
            //    options.RequireHttpsMetadata = true;
            //    //options.Events.OnTicketReceived = async n => 

            //    //{
            //    //    var nid = new ClaimsIdentity(
            //    //            n.HttpContext.User.Identity.AuthenticationType,
            //    //            "given_name",
            //    //            "role");

            //    //    Debug.WriteLine(n.ReturnUri);


            //    //};

            //    //options.Events.OnTokenValidated = async n =>
            //    //{
            //    //    Debug.WriteLine(n.SecurityToken.ToString());
            //    //};


            //});


            //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters 
            services.AddMvc(options=> {

                //All  global filters to be here, with order of executions too.
                //options.Filters.Add()
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            loggerfactory.AddConsole();
            loggerfactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            //app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseAuthentication();

          //  app.UseSession();

            app.UseStaticFiles();
            // app.UseOpenIdConnectAuthentication()
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
