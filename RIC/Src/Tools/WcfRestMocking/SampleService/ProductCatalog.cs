#region License

// The MIT License
//
// Copyright (c) 2009 Pablo Mariano Cibraro.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;

#if DEBUG
using WebOperationContext = System.ServiceModel.Web.MockedWebOperationContext;
#endif

namespace WCFMock.SampleService
{
	public class ProductCatalog : IProductCatalog
	{
		IProductRepository repository;

		public ProductCatalog()
		{
			repository = new InMemoryProductRepository(
				new List<Product>{ 
					new Product { Id = "1", Category = "foo", Name = "Foo1", UnitPrice = 1 },
					new Product { Id = "2", Category = "bar", Name = "bar2", UnitPrice = 2 }
				}
			);
		}

		public ProductCatalog(IProductRepository repository)
		{
			this.repository = repository;
		}
		
		public Atom10FeedFormatter GetProducts(string category)
		{
			var items = new List<SyndicationItem>();
			foreach(var product in repository.GetProducts(category))
			{
				items.Add(new SyndicationItem()
				{
					Id = String.Format(CultureInfo.InvariantCulture, "http://products/{0}", product.Id),
					Title = new TextSyndicationContent(product.Name),
					LastUpdatedTime = new DateTime(2008, 7, 1, 0, 0, 0, DateTimeKind.Utc),
					Authors = 
					{ 
						new SyndicationPerson() 
						{
							Name = "cibrax"
						}
					},
					Content = new TextSyndicationContent(string.Format("Category Id {0} - Price {1}",
						product.Category, product.UnitPrice))
				});
			}
										
			var feed = new SyndicationFeed()
			{
				Id = "http://Products",
				Title = new TextSyndicationContent("Product catalog"),
				Items = items
			};

			WebOperationContext.Current.OutgoingResponse.ContentType = "application/atom+xml";
			return feed.GetAtom10Formatter();
		}
	}
}
