﻿if (typeof global == "undefined")
{ var global = {}; }

var global = {

    domain: "",
    expire: 1,
    cookie: {
        order: {
            invoice: "HONGLI.order.invoice",
            deliver: "HONGLI.order.deliver",
            payAndDeliver: "HONGLI.order.payAndDeliver",
            policyHolder: "HONGLI.order.policyHolder",
            product: "HONGLI.order.product"
        }
    },
    order: {
        //发票
        invoice: function (invoiceType, invoiceTitle) {
            this.invoiceType = invoiceType;
            this.invoiceTitle = invoiceTitle;
        },
        //支付配送
        payAndDeliver: function (payType, deliverType,deliverPrice, selfTakeAddress, selfTakeDate) {
            this.payType = payType;//支付类型 1在线配送
            this.deliverType = deliverType;//配送类型 //1配送，2自取
            this.deliverPrice = deliverPrice;//配送价格
            this.selfTakeAddress = selfTakeAddress;//自取地址
            this.selfTakeDate = selfTakeDate;//自取时间
        },
        //配送信息
        deliver: function (deliverName, deliverMobile, deliverDistrictCode, deliverAddress) {            
            this.deliverName = deliverName;
            this.deliverMobile = deliverMobile;
            this.deliverDistrictCode = deliverDistrictCode;
            this.deliverAddress = deliverAddress;            
        },
        //被保人信息
        policyHolder: function (name, idcardType, idcard, idcardFacePicUrl, idcardBackPicUrl) {
            this.name = name;
            this.idcardType = idcardType;
            this.idcard = idcard;
            this.idcardFacePicUrl = idcardFacePicUrl;
            this.idcardBackPicUrl = idcardBackPicUrl;
        },
        //产品
        product: function (channel, productId, isuranceLogo, name, title, originalPrice, dealPrice, LicenseNo) {
            this.channel = channel;//渠道
            this.productId = productId;//产品id
            this.isuranceLogo = isuranceLogo;//保险logo,有一个保险的枚举类别，识别是哪个枚举值，我在订单页面显示不同的logo图片
            this.name = name;//产品名
            this.title = title;//产品展示文本
            this.originalPrice = originalPrice;//原始价格
            this.dealPrice = dealPrice;//成交价格
            this.LicenseNo = LicenseNo;//车牌号
        }
    }
}

//注意：下面模拟代码，要提走放到自己的页面中去

//模拟宏伟写入cookie
var order_deliver = new global.order.deliver("尚国强", "15210000000", "100090", "北京海淀");
var order_deliver_str = JSON.stringify(order_deliver);
common.cookie.setCookie(global.domain, global.cookie.order.deliver, order_deliver_str, global.expire);

var payType = 1;//1在线支付
var deliverType = 1;//1配送，2自取
var deliverPrice = 0;//自取，配送价格 //TODO by guoqiangs 去确认
var order_payAndDeliver = new global.order.payAndDeliver(payType, deliverType, deliverPrice, "自取地址-北京海淀区", "2016-3-16");
var order_payAndDeliver_str = JSON.stringify(order_payAndDeliver);
common.cookie.setCookie(global.domain, global.cookie.order.payAndDeliver, order_payAndDeliver_str, global.expire);

var order_invoice = new global.order.invoice(1, "北京宏利");
var order_invoice_str = JSON.stringify(order_invoice);
common.cookie.setCookie(global.domain, global.cookie.order.invoice, order_invoice_str, global.expire);

var order_policyHolder = new global.order.policyHolder("尚国强", 1, "410423100000000000", "face.jpg", "back.jpg");
var order_policyHolder_str = JSON.stringify(order_policyHolder);
common.cookie.setCookie(global.domain, global.cookie.order.policyHolder, order_policyHolder_str, global.expire);

//模拟赞军写入cookie
var channel = 1;
var isuranceLogo = 1;//1PICC,2PINGAN,3TAIPING
var order_product = new global.order.product(channel, "1000", isuranceLogo, "平安", "平安平安平安", 5000, 3999, "京N91888");
var order_product_str = JSON.stringify(order_product);
common.cookie.setCookie(global.domain, global.cookie.order.product, order_product_str, global.expire);

