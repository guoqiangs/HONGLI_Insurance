using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HONGLI.Entity;
using HONGLI.Service;
using HONGLI.Web.Models;
using X3;
namespace HONGLI.Web.Controllers
{
    public class OrderController : Controller
    {
        
        public OrderService _orderService = new OrderService();

        public PayService _payService = new PayService();

        // GET: Order/index
        public ActionResult Index()
        {          
            return View();            
        }

        public ActionResult Detail(string code)
        {

            var order = _orderService.GetOrderByCode(code);
            var model = new OrderDetailModels()
            {
                OrderCode = order.OrderCode,
                DeliverType = order.Order_Deliver.FirstOrDefault().DeliverType,
                DeliverName = order.Order_Deliver.FirstOrDefault().DeliverName,
                DeliverMobile = order.Order_Deliver.FirstOrDefault().DeliverMobile,
                DeliverAddress = order.Order_Deliver.FirstOrDefault().DeliverAddress,
                DeliverPrice = order.Order_Deliver.FirstOrDefault().DeliverPrice,
                ProductId = order.Order_Item.FirstOrDefault().ProductId,
                ProductName = order.Order_Item.FirstOrDefault().ProductName,
                ProductTitle = order.Order_Item.FirstOrDefault().ProductTitle,
                ProductOriginalPrice = order.Order_Item.FirstOrDefault().ProductOriginalPrice,
                ProductDealPrice = order.Order_Item.FirstOrDefault().ProductDealPrice,
                InvoiceTitle = order.InvoiceTitle,
                InvoiceType = order.InvoiceType,
                PolicyHolderName = order.Order_PolicyHolder.FirstOrDefault().Name,
                PolicyHolderIdcard = order.Order_PolicyHolder.FirstOrDefault().IdCard,
                PayType = order.Order_Pay.FirstOrDefault().PayType,
                InsuranceLogo = order.Order_Item.FirstOrDefault().InsuranceLogo,
                AmountPayable = order.AmountPayable,
                CreateDate = order.CreateDate

            };
            return View(model);
        }

        /// <summary>
        /// 下订单
        /// </summary>
        /// <returns></returns>
        public ActionResult Do()
        {
            return View();
        }        

        /// <summary>
        /// 订单提交
        /// </summary>
        /// <param name="product"></param>
        /// <param name="invoice"></param>
        /// <param name="deliver"></param>
        /// <param name="policyHolder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(ProductEntity product, InvoiceEntity invoice,PayAndDeliverEntity payAndDeliver, DeliverEntity deliver,PolicyHolderEntity policyHolder)
        {
            ResultData resultData = new ResultData();
                      
            var orderCode = AddOrder(product, invoice,payAndDeliver, deliver, policyHolder);
            if (!string.IsNullOrEmpty(orderCode))
            {
                resultData.Status = 1;
                resultData.Data = orderCode;
            }
            else
            {
                resultData.Status = 0;
            }
            return this.Json(resultData);            
        }

        public ActionResult PayConfirm(string ordercode)
        {            
            var order = _orderService.GetOrderByCode(ordercode);
            var product = order.Order_Item.FirstOrDefault();

            if (product != null)
            {
                var model = new PayConfirmModel()
                {
                    OrderCode = order.OrderCode,
                    LicenseNo = product.LicenseNo,
                    DealPrice = product.ProductDealPrice
                };
                return View(model);
            }
            else
            {
                throw new Exception("产品信息异常");
            }                        
        }

        public ActionResult Pay(string ordercode)
        {
            var order = _orderService.GetOrderByCode(ordercode);
            
            var response = _payService.CreateRequest(order);
            Response.Write(response);
            return null;           
        }
        

        /// <summary>
        /// 下订单
        /// </summary>
        /// <param name="product"></param>
        /// <param name="invoice"></param>
        /// <param name="deliver"></param>
        /// <param name="policyHolder"></param>
        /// <returns></returns>
        public string AddOrder(ProductEntity product,InvoiceEntity invoice, PayAndDeliverEntity payAndDeliver, DeliverEntity deliver, PolicyHolderEntity policyHolder)
        {
            //TODO by guoqiangs 
            string userId = "user001";
            string orderCode = "HLR00001" + DateTime.Now.ToString("yyyyMMddHmmss");

            Order_Deliver order_deliver = new Order_Deliver();
            if (payAndDeliver.DeliverType == 1)
            {
                order_deliver.DeliverDistrictCode = deliver.deliverDistrictCode;
                order_deliver.DeliverMobile = deliver.deliverMobile;
                order_deliver.DeliverAddress = deliver.deliverAddress;
                order_deliver.DeliverName = deliver.deliverName;
                order_deliver.DeliverPrice = payAndDeliver.DeliverPrice;
                order_deliver.DeliverType = payAndDeliver.DeliverType;
                order_deliver.DeliverTime = DateTime.Now;
                order_deliver.CreateDate = DateTime.Now;
            }
            else
            {
                order_deliver.DeliverType = 2;                
                order_deliver.DeliverAddress = payAndDeliver.SelfTakeAddress;
                order_deliver.DeliverTime = payAndDeliver.SelfTakeDate;
                order_deliver.DeliverPrice = payAndDeliver.DeliverPrice;
                order_deliver.CreateDate = DateTime.Now;
            }
           

            Order_PolicyHolder order_policyHolder = new Order_PolicyHolder()
            {
                Name = policyHolder.Name,
                IdCard = policyHolder.Idcard,
                IdCardType = policyHolder.IdcardType,
                IdCardFacePicUrl = policyHolder.IdcardFacePicUrl,
                IdCardBackPicUrl = policyHolder.IdcardBackPicUrl,
                CreateDate = DateTime.Now
            };

            Order_Item order_item = new Order_Item()
            {                
                Channel = product.Channel,
                ProductId = product.ProductId,
                LicenseNo = product.LicenseNo,
                InsuranceLogo = product.IsuranceLogo,
                ProductName = product.Name,
                ProductDealPrice = product.DealPrice,
                ProductOriginalPrice = product.OriginalPrice,
                ProductTitle = product.Title,
                CreateDate = DateTime.Now
            };

            Order_Pay order_pay = new Order_Pay()
            {
                PayBank = 1,
                PayType = 1,
                CreateDate = DateTime.Now
            };

            Order_Base order_base = new Order_Base()
            {
                OrderCode = orderCode,
                Channel = product.Channel,                
                AmountPayable = product.DealPrice + order_deliver.DeliverPrice,//订单应付价格 = 产品价格+配送价格                
                UserId = userId,
                InvoiceType = invoice.InvoiceType,
                InvoiceTitle = invoice.InvoiceTitle,
                Status = (int)EnumOrderStatus.UnPay,
                CreateDate = DateTime.Now,
                Order_Deliver = new List<Order_Deliver>() { order_deliver },
                Order_Item = new List<Order_Item>() { order_item },
                Order_Pay = new List<Order_Pay>() { order_pay },
                Order_PolicyHolder = new List<Order_PolicyHolder>() { order_policyHolder }
            };

            var result = _orderService.AddOrder(order_base);            
            return result;
            
        }
        


    }
}