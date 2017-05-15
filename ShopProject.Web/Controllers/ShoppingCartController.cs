using Shopproject.Common;
using ShopProject.Service;
using ShopProject.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopProject.Common;
using ShopProject.Web.Models;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using ShopProject.Model.Models;
using AutoMapper;
using ShopProject.Web.Infrastructure.Extensions;

namespace ShopProject.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        IOrderService _orderService;
        private ApplicationUserManager _userManager;
        public ShoppingCartController(IProductService productService, IOrderService orderService, ApplicationUserManager userManager)
        {
            _productService = productService;
            _userManager = userManager;
            _orderService = orderService;
        }

        public List<ShoppingCartViewModel> GetCart()
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
                Session[CommonConstants.SessionCart] = cart;
            }
            return cart;
        }
        public ActionResult Index()
        {
            var cart = GetCart();

            return View();
        }

        public ActionResult CheckOut()
        {
            if (Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/gio-hang.html");
            }
            return View();
        }

        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public ActionResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);

            var orderNew = new Order();

            orderNew.UpdateOrder(order);

            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = GetCart();
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.Price;
                orderDetails.Add(detail);

                isEnough = _productService.SellProduct(item.ProductId,item.Quantity);
                break;

            }
            if (isEnough)
            {
                _orderService.Create(orderNew, orderDetails);
                _orderService.Save();
                return Json(new
                {
                    status = true,
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message= "Sản phẩm này hiện đang hết hàng"
                });
            }         
        }

        #region CRUD and Get ShoppingCart
        public JsonResult GetAll()
        {
            var cart = GetCart();
            return Json(new
            {
                data = cart,
                status = true,

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = GetCart();

            var product = _productService.GetById(productId);

            if (product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }
            Session[CommonConstants.SessionCart] = cart;


            return Json(new
            {
                status = true,
                count = cart
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = GetCart();
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }
            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = GetCart();
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false,
                data = Session[CommonConstants.SessionCart]
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }
        #endregion


    }
}