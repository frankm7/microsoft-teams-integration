extern alias BetaLib;
using Beta = BetaLib.Microsoft.Graph;
using GraphApiTest.Helpers;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace GraphApiTest.Controllers
{
    public class ClassesController : BaseController
    {
        // Load configuration settings from PrivateSettings.config
        private static string appId = ConfigurationManager.AppSettings["applicationId"];
        private static string appSecret = ConfigurationManager.AppSettings["applicationSecret"];
        private static string tenantID = ConfigurationManager.AppSettings["tenantId"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static List<string> graphScopes =
            new List<string>(ConfigurationManager.AppSettings["ida:AppScopes"].Split(' '));

        // GET: Classes
        [Authorize]
        public async Task<ActionResult> Index()
        {
            //IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
            //    .Create(appId)
            //    .WithRedirectUri(redirectUri)
            //    .WithClientSecret(appSecret) // or .WithCertificate(certificate)
            //    .Build();

            //OnBehalfOfProvider authProvider = new OnBehalfOfProvider(confidentialClientApplication);

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(appId)
                .WithRedirectUri(redirectUri)
                .WithClientSecret(appSecret) // or .WithCertificate(certificate)
                .Build();

            //AuthorizationCodeProvider authProvider = new AuthorizationCodeProvider(confidentialClientApplication, graphScopes);

            //IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
            //    .Create(appId)
            //    .WithTenantId(tenantID)
            //    .WithClientSecret(appSecret)
            //    .Build();

            //ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);

            //IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder
            //.Create(appId)
            //.Build();

            //Func<DeviceCodeResult, Task> deviceCodeReadyCallback = async dcr => await Console.Out.WriteLineAsync(dcr.Message);

            //DeviceCodeProvider authProvider = new DeviceCodeProvider(publicClientApplication, graphScopes, deviceCodeReadyCallback);

            IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder
            .Create(appId)
            .Build();

            InteractiveAuthenticationProvider authProvider = new InteractiveAuthenticationProvider(publicClientApplication, graphScopes);

            var contact = new Beta.Contact();
            //Beta.GraphServiceClient betaClient = new Beta.GraphServiceClient();

            Beta.GraphServiceClient betaClient = new Beta.GraphServiceClient(authProvider);

            var classes = await betaClient
            .Education
            .Me
            .Classes
            .Request()
            .GetAsync();

            return View(classes);
        }
    }
}