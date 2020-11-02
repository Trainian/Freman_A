using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore_FremanA.Controllers;
using AspNetCore_FremanA.Models;
using AspNetCore_FremanA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AspNetCore_FremanA.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Действие
            ProductListViewModel result = (controller.List(null,2) as ViewResult)?.ViewData.Model as ProductListViewModel;

            //Утверждение
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product(){ProductID = 1, Name = "P1"},
                new Product(){ProductID = 2, Name = "P2"},
                new Product(){ProductID = 3, Name = "P3"},
                new Product(){ProductID = 4, Name = "P4"},
                new Product(){ProductID = 5, Name = "P5"},
                
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object){PageSize = 3};

            //Действие
            ProductListViewModel result = (controller.List(null,2) as ViewResult)?.ViewData.Model as ProductListViewModel;

            //Утверждение
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerRage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);

        }

        [Fact]
        public void Can_Filter_Products()
        {
            //Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product() {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product() {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product() {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product() {ProductID = 5, Name = "P5", Category = "Cat3"},
            }).AsQueryable<Product>);

            //Организация - создание контроллера и установка размера страницы
            //в три элемента
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Действие
            Product[] result = ((controller.List("Cat2", 1) as ViewResult)?
                    .Model as ProductListViewModel)?
                    .Products.ToArray();

            //Утверждение
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[0].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product() {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product() {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product() {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product() {ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable<Product>());

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            Func<ViewResult, ProductListViewModel> GetModel = result =>
                result?.ViewData.Model as ProductListViewModel;

            // Действие
            int? res1 = GetModel((ViewResult)target.List("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel((ViewResult)target.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel((ViewResult)target.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel((ViewResult)target.List(null))?.PagingInfo.TotalItems;

            // Утверждение
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
