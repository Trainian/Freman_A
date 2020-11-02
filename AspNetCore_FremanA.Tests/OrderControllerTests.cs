using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore_FremanA.Controllers;
using AspNetCore_FremanA.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AspNetCore_FremanA.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация -создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Органзация - соаздение пустой корзины
            Cart cart = new Cart();

            // Организация - создание заказа
            Order order = new Order();

            // Организация - создание экземпляра контроллера
            OrderController target = new OrderController(mock.Object, cart);

            // Действие
            ViewResult result = target.Checkout(order) as ViewResult;

            // Утверждение - проверка, что заказ был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // Утверждение - проверка, что метод возвращает стандартное предстваление
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            // Утверждение - проврка, что представлению передана недопустимая модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Организация - создание корзины с одним элементом
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Организация - создание жкземпляра контроллера
            OrderController target = new OrderController(mock.Object, cart);

            // Организация - добавление ошибки в модель
            target.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            // Утверждение - проверка, что заказ не был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // Утверждение - проверка, что метод возвращает стандартное представление
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            // Утверждение - проверка, что представлению передаётся недопустимая модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Организация - создание корзины с одним элементом
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Организация - создание экземпляра контроллера
            OrderController target = new OrderController(mock.Object, cart);

            // Действие - попытка перехода к оплате
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            // Утверждение - проверка, что заказ был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

            // Утверждение - проверка, что метод перенаправляется на действие Completed
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
