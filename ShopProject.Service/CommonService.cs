using System;
using ShopProject.Common;
using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;
using System.Collections.Generic;

namespace ShopProject.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
        IEnumerable<Slide> GetSlide();
    }
    public class CommonService : ICommonService
    {
        ISlideRepository _slideRepository;
        IFooterRepository _footerRepository;
        IUnitOfWork _unitOfWork;
        public CommonService(IFooterRepository footerRepository, ISlideRepository slideRepository, IUnitOfWork unitOfWork)
        {
            _slideRepository = slideRepository;
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
        }
        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x=>x.ID == CommonConstants.DefaultFooterId);
        }

        public IEnumerable<Slide> GetSlide()
        {
            return _slideRepository.GetMulti(x => x.Status);
        }
    }
}
