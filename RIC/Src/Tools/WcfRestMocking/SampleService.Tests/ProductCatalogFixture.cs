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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using WCFMock.SampleService;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SampleService.Tests
{
	[TestClass]
	public class ProductCatalogFixture
	{
		[TestMethod]
		public void ShouldGetProductsFeed()
		{
			ProductCatalog catalog = new ProductCatalog(
				new InMemoryProductRepository(
					new List<Product>{ 
					new Product { Id = "1", Category = "foo", Name = "Foo1", UnitPrice = 1 },
					new Product { Id = "2", Category = "bar", Name = "bar2", UnitPrice = 2 }
				}));

			Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
			IEnumerable<SyndicationItem> items;
			using (new MockedWebOperationContext(mockContext.Object))
			{
				var formatter = catalog.GetProducts("foo");
				items = formatter.Feed.Items;
			}

			mockContext.VerifySet(c => c.OutgoingResponse.ContentType, "application/atom+xml");

			Assert.AreEqual(1, items.Count());
			Assert.IsTrue(items.Any(i => i.Id == "http://products/1" && i.Title.Text == "Foo1"));
		}
	}
}
