using AutoMapper;
using ShopProject.Common;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using ShopProject.Web.Infrastructure.Extensions;
using ShopProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopProject.Web.API
{
    [Authorize]
    [RoutePrefix("api/product")]
    public class ProductController : ApiControllerBase
    {
        #region
        private IProductService _productService;

        public ProductController(IErrorService errorService, IProductService productService) : base(errorService)
        {
            this._productService = productService;
        }

        #endregion
        [Authorize]
        [Route("getparent")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var lstProduct = _productService.GetAll();

                var lstProductVm = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProduct);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, lstProductVm);

                return response;
            });
        }
        [Authorize]
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;

                var lstProduct = _productService.GetAll(keyword);

                foreach (var item in lstProduct)
                {
                    if (!String.IsNullOrEmpty(item.Image))
                    {
                        item.Image = ConvertData.ImageToBase64String(item.Image,CommonConstants.PathProduct);
                    }

                }

                totalRow = lstProduct.Count();

                var query = lstProduct.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var lstProductVm = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<ProductViewModel>()
                {
                    Items = lstProductVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Authorize]
        [Route("getbyid/{productID:int}")]
        [HttpGet]
        public HttpResponseMessage GetByID(HttpRequestMessage request, int productID)
        {
            return CreateHttpResponse(request, () =>
            {
                var productItem = _productService.GetById(productID);
                if (!String.IsNullOrEmpty(productItem.Image))
                {
                    // Convert image to base 64 string.
                    productItem.Image = ConvertData.ImageToBase64String(productItem.Image,CommonConstants.PathProduct);
                }


                var productItemVm = Mapper.Map<Product, ProductViewModel>(productItem);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, productItemVm);

                return response;
            });

        }

        [Authorize]
        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {

                    var product = new Product();
                    product.UpdateProduct(productVm);


                    if (!String.IsNullOrEmpty(productVm.Image))
                    {

                        var image = ConvertData.Base64ToImage(productVm.Image);

                        product.Image = product.ID + "_" + product.Name + ".jpg";

                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                            + CommonConstants.PathProduct + "/" + product.Image);

                        image.Save(filePath, ImageFormat.Jpeg);
                        //File.WriteAllBytes(string path,byte[] bytes);
                    }


                    product.CreatedDate = DateTime.Now;

                    product.Status = true;

                    _productService.Add(product);

                    _productService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json

                    var responseData = Mapper.Map<Product, ProductViewModel>(product);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                return response;
            });
        }

        [Authorize]
        [Route("edit")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                }
                else
                {

               
                var productDb = new Product();

                productDb.UpdateProduct(productVm);

                if (!String.IsNullOrEmpty(productVm.Image))
                {
                    var image = ConvertData.Base64ToImage(productVm.Image);

                    productDb.Image = productDb.ID + "_" + productDb.Name + ".jpg";

                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                            + CommonConstants.PathProduct + "/" + productDb.Image);

                    image.Save(filePath, ImageFormat.Jpeg);
                    //File.WriteAllBytes(string path,byte[] bytes);
                }

                productDb.UpdatedDate = DateTime.Now;

                _productService.Update(productDb);

                _productService.Save();

                //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json

                var responseData = Mapper.Map<Product, ProductViewModel>(productDb);
                response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                return response;
            });
        }

        [Authorize]
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int productID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var product = _productService.Delete(productID);
                    _productService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Product, ProductViewModel>(product);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Authorize]
        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstProductID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstProductID);
                    foreach (var item in lstItemID)
                    {
                        _productService.Delete(item);
                    }
                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
                }
                return response;
            });
        }
    }
}