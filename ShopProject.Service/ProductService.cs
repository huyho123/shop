using System;
using System.Collections.Generic;
using System.Linq;
using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;

namespace ShopProject.Service
{
    public interface IProductService
    {
        Product Add(Product Product);

        void Update(Product Product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetLastest(int top);

        IEnumerable<Product> GetHotProduct(int top);

        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> GetListProduct(string keyword);

        IEnumerable<Product> GetReatedProducts(int id, int top);

        IEnumerable<string> GetListProductByName(string name);

        Product GetById(int id);

        void Save();

        IEnumerable<Tag> GetListTagByProductId(int id);

        Tag GetTag(string tagId);

        void IncreaseView(int id);

        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pagesize, out int totalRow);

        bool SellProduct(int productId, int quantity);
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;

        private IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository,
            ITagRepository _tagRepository, IUnitOfWork unitOfWork)
        {
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._tagRepository = _tagRepository;
            this._unitOfWork = unitOfWork;
        }

        public Product Add(Product Product)
        {
            throw new NotImplementedException();
        }

        public void Update(Product Product)
        {
            throw new NotImplementedException();
        }

        public Product Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetLastest(int top)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetListProduct(string keyword)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetReatedProducts(int id, int top)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            throw new NotImplementedException();
        }

        public Tag GetTag(string tagId)
        {
            throw new NotImplementedException();
        }

        public void IncreaseView(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pagesize, out int totalRow)
        {
            throw new NotImplementedException();
        }

        public bool SellProduct(int productId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}