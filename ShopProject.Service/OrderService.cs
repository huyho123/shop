using ShopProject.Common.ViewModels;
using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Service
{
    public interface IOrderService
    {
        bool Create(Order order, List<OrderDetail> orderDetails);
        void UpdateStatus(int orderId);
        void Save();
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }
        public bool Create(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
