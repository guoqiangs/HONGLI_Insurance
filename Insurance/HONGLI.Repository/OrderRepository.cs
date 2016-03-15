using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HONGLI.Entity;

namespace HONGLI.Repository
{
    public class OrderRepository
    {
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Order_Base GetOrderByCode(string code)
        {
            Order_Base order = null;
            try
            {
                using (var context = new E2JOINDB())
                {

                    var query = context.Order_Base                        
                        .Include("Order_Item")
                        .Include("Order_Deliver")
                        .Include("Order_Pay")
                        .Include("Order_PolicyHolder")
                        .Where(c => c.OrderCode == code)
                        .FirstOrDefault();

                    order = query;
                }
            }
            catch (Exception error)
            {
                //LogHelper.AppError(error.Message);
            }
            return order;
        }


        /// <summary>
        /// 下单操作
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string AddOrder(Order_Base order)
        {
            string code = "";
            try
            {
                using (var context = new E2JOINDB())
                {
                    context.Order_Base.Add(order);
                    context.SaveChanges();
                    code = order.OrderCode;
                }              
            }
            catch (Exception error)
            {
                //LogHelper.AppError(error.Message);
            }
            return code;
        }

        /// <summary>
        /// 更新支付状态
        /// </summary>
        /// <param name="ordercode"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(string ordercode, int status)
        {
            int result = 0;
            using (var context = new E2JOINDB())
            {
                var query = context.Order_Base.Where(c => c.OrderCode == ordercode).FirstOrDefault();
                if (query != null)
                {
                    query.Status = status;
                    result = context.SaveChanges();
                }
            }
            return result;
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
            int result = 0;
            using (var context = new E2JOINDB())
            {
                var query = context.Order_Base.Where(c => c.OrderCode == ordercode).FirstOrDefault();
                if (query != null && query.Status == (int)EnumOrderStatus.Paid || query.Status == (int)EnumOrderStatus.Success)
                {
                    result = 1;
                    return result;
                }

                if (query != null)
                {
                    query.TradeNo = tradeNo;
                    query.PaidAmount = Convert.ToDecimal(paidAmount);
                    query.PayTime = payTime;
                    query.Status = status;
                    result = context.SaveChanges();
                }
            }
            return result;
        }


    }
}
