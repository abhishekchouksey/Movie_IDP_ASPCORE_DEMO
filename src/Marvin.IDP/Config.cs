using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Marvin.IDP
{
    public static class Config
    {
        public static List<TestUser> Getusers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Frank",
                    Password = "Password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Frank"),
                        new Claim("family_name", "Underwood"),
                    }
                },


                new TestUser
                {
                    SubjectId = "2",
                    Username = "Clair",
                    Password = "Password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Clair"),
                        new Claim("family_name", "Underwood"),
                    }
                },

            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }


        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "Movie",
                    ClientId = "movieClient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44378/signin-oidc"
                    },

                    PostLogoutRedirectUris =
                    {
                          "https://localhost:44378/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "movieapi"

                    },

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    //AlwaysIncludeUserClaimsInIdToken = true
                    
                }
            };
        }


        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("movieapi", "Movie API")
            };
        }
    }

}

    

