using Xero.Api.Core;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Infrastructure.RateLimiter;
using Xero.Api.Serialization;
using Xero.Api.Core.Model;
using System;
using System.Configuration;

namespace Xero.Api.Applications.Private
{
    public class Core : XeroCoreApi
    {
        private static readonly DefaultMapper Mapper = new DefaultMapper();
        private static readonly Settings ApplicationSettings = new Settings();

        public Core(bool includeRateLimiter = false) :
            base(ApplicationSettings.Uri,
                new PrivateAuthenticator(ApplicationSettings.SigningCertificatePath, ApplicationSettings.SigningCertificatePassword),
                new Consumer(ApplicationSettings.Key, ApplicationSettings.Secret),
                null,
                Mapper,
                Mapper,
                includeRateLimiter ? new RateLimiter() : null)
        {
        }
    }

    public class XeroConnector
    {
        private IXeroCoreApi _api;

        protected IXeroCoreApi Api
        {
            get { return _api ?? (_api = CreateCoreApi()); }
        }

        private static IXeroCoreApi CreateCoreApi()
        {
            return new Xero.Api.Applications.Private.Core
            {
                UserAgent = "X"
            };
        }

        public Guid create_contact(Contact _contact_to_create)
        {
            var _response = Api.Contacts.Create(_contact_to_create);
            return _response.Id;
        }

        public Guid create_invoice(Invoice _inv_to_create)
        {
            var _response = Api.Invoices.Create(_inv_to_create);
            return _response.Id;
        }

    }


    public class Settings
        {
            public string Uri
            {
                get { return ConfigurationManager.AppSettings["BaseUrl"]; }
            }

            public string SigningCertificatePath
            {
                get { return ConfigurationManager.AppSettings["SigningCertificate"]; }
            }

            public string SigningCertificatePassword
            {
                get { return ConfigurationManager.AppSettings["SigningCertificatePassword"]; }
            }

            public string Key
            {
                get { return ConfigurationManager.AppSettings["ConsumerKey"]; }
            }

            public string Secret
            {
                get { return ConfigurationManager.AppSettings["ConsumerSecret"]; }
            }
        }    
}
