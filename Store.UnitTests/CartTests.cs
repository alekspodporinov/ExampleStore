using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Store.Domain.Abstract;
using Store.Domain.Entities;
using Store.WebUI.Controllers;
using Store.WebUI.Models;

namespace Store.UnitTests
{
    [TestFixture]
    public class CartTests
    {
        [Test]
        public void CanAddNewLines()
        {
            // Организация - создание нескольких тестовых игр
            Game game1 = new Game {GameId = 1, Name = "Игра1"};
            Game game2 = new Game {GameId = 2, Name = "Игра2"};

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
        }

        [Test]
        public void CanAddQuantityForExistingLines()
        {
            // Организация - создание нескольких тестовых игр
            Game game1 = new Game {GameId = 1, Name = "Игра1"};
            Game game2 = new Game {GameId = 2, Name = "Игра2"};

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6); // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [Test]
        public void CanRemoveLine()
        {
            // Организация - создание нескольких тестовых игр
            Game game1 = new Game {GameId = 1, Name = "Игра1"};
            Game game2 = new Game {GameId = 2, Name = "Игра2"};
            Game game3 = new Game {GameId = 3, Name = "Игра3"};

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 4);
            cart.AddItem(game3, 2);
            cart.AddItem(game2, 1);

            // Действие
            cart.RemoveLine(game2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(c => c.Game == game2), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [Test]
        public void CalculateCartTotal()
        {
            // Организация - создание нескольких тестовых игр
            Game game1 = new Game {GameId = 1, Name = "Игра1", Price = 100};
            Game game2 = new Game {GameId = 2, Name = "Игра2", Price = 55};

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }

        [Test]
        public void CanClearContents()
        {
            // Организация - создание нескольких тестовых игр
            Game game1 = new Game {GameId = 1, Name = "Игра1", Price = 100};
            Game game2 = new Game {GameId = 2, Name = "Игра2", Price = 55};

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [Test]
        public void CanAddToCart()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }

        [Test]
        public void AddingGameToCartGoesToCartScreen()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [Test]
        public void CanViewCartContents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel) target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}
