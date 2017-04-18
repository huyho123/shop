using AutoMapper;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using ShopProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopProject.Web.API
{
    [RoutePrefix("api/productcategory")]
    public class ProductCategoryController : ApiControllerBase
    {
        IProductCategoryService _productCategoryService;
        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base(errorService)
        {
            _productCategoryService = productCategoryService;
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request,string keyword,int page, int pageSize = 20)
        {
            return CreateHttpResponse(request,()=> 
            {
                int totalRow = 0;

                var listproductCategory = _productCategoryService.GetAll(keyword);

                totalRow = listproductCategory.Count();

                // lay so ban ghi su dung de phan trang tu du lieu cua model.
                var query = listproductCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                // map chuyen doi du lieu tu model toi du lieu view model
                var productCategoryVm = Mapper.Map<IEnumerable<ProductCategory>,IEnumerable<ProductCategoryViewModel>>(query);

                // Khai bao paginationSet de the hien du lieu json tren web api voi cac thong tin ve page,totalcount,totalpage,ienumerable<model> 
                var paginationSet = new PaginationSet<ProductCategoryViewModel>()
                {
                    Items = productCategoryVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                // hien thi du lieu pageinationSet json
                HttpResponseMessage response =  request.CreateResponse(HttpStatusCode.OK, paginationSet);

                return response;
            });
        }
    }
}