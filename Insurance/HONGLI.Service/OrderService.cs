using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HONGLI.Entity;
using HONGLI.Repository;

namespace HONGLI.Service
{
    public class OrderService
    {
        public OrderRepository _orderRepository = new OrderRepository();
        public string AddOrder(Order_Base order)
        {
            string code = _orderRepository.AddOrder(order);
            return code;
        }

        public Order_Base GetOrderByCode(string code)
        {
            return _orderRepository.GetOrderByCode(code);
        }

        /// <summary>
        /// 更新支付状态
        /// </summary>
        /// <param name="ordercode"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(string ordercode, int status)
        {
            return _orderRepository.UpdateOrderStatus(ordercode, status);
        }

        /// <summary>
        /// 支付后更新数据库
        /// </summary>
        /// <param name="ordercode"></param>
        /// <param name="tradeNo"></param>
        /// <param name="paidAmount"></param>
        /// <param name="payTime"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdatePay(string ordercode, string tradeNo, string paidAmount, DateTime payTime, byte status)
        {
            return _orderRepository.UpdatePay(ordercode, tradeNo, paidAmount, payTime, status);
        }

    }
}
