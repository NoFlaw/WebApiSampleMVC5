// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap;
using WebApiSample.Areas.HelpPage.Controllers;
using WebApiSample.Models;
using WebApiSample.Services;

namespace WebApiSample.DependencyResolution {
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
	
    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.LookForRegistries();
                    scan.TheCallingAssembly();
                    scan.AssembliesFromApplicationBaseDirectory(assembly => !assembly.FullName.StartsWith("System.Web"));
                    scan.ExcludeNamespace("System.Web");
                    scan.AddAllTypesOf<IHttpModule>();       
                    scan.WithDefaultConventions();
                });

            For<IProductService>().Singleton().Use<ProductService>();
            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
            For<System.Data.Entity.DbContext>().Use(() => new ApplicationDbContext());
            //For<IProductRepository>().Use<ProductRepository>();
        }

        #endregion
    }
}