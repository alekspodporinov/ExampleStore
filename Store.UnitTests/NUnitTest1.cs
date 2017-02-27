using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Store.Domain.Abstract;
using Store.Domain.Entities;
using Store.WebUI.Controllers;
//using Store.WebUI.HtmlHelpers;
using Store.WebUI.Models;

namespace Store.UnitTests
{
    [TestFixture]
    public class NUnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name = "Игра1"},
                new Game {GameId = 2, Name = "Игра2"},
                new Game {GameId = 3, Name = "Игра3"},
                new Game {GameId = 4, Name = "Игра4"},
                new Game {GameId = 5, Name = "Игра5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            GamesListViewModel result = (GamesListViewModel) controller.List(2).Model;

            // Утверждение
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");
        }

        //[Test]
        //public void Can_Generate_Page_Links()
        //{

        //    // Организация - определение вспомогательного метода HTML - это необходимо
        //    // для применения расширяющего метода
        //    HtmlHelper myHelper = null;

        //    // Организация - создание объекта PagingInfo
        //    PagingInfo pagingInfo = new PagingInfo
        //    {
        //        CurrentPage = 2,
        //        TotalItems = 28,
        //        ItemsPerPage = 10
        //    };

        //    // Организация - настройка делегата с помощью лямбда-выражения
        //    Func<int, string> pageUrlDelegate = i => "Page" + i;

        //    // Действие
        //    MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

        //    // Утверждение
        //    Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
        //                    + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
        //                    + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
        //        result.ToString());
        //}

        [Test]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name = "Игра1"},
                new Game {GameId = 2, Name = "Игра2"},
                new Game {GameId = 3, Name = "Игра3"},
                new Game {GameId = 4, Name = "Игра4"},
                new Game {GameId = 5, Name = "Игра5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Act
            GamesListViewModel result
                = (GamesListViewModel) controller.List(2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
    }
}   