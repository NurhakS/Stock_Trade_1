using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using PagedList;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stock_Trade_1.Models;
using System.Windows;
using System.Runtime.Remoting.Messaging;
using CrystalDecisions.CrystalReports.Engine;

namespace Stock_Trade_1.Controllers
{
    public class HomeController : Controller
    {
        private Stock_TradeEntities2 db = new Stock_TradeEntities2();

        public ActionResult Index()
        {
         
            return View();
        }

        public ActionResult Stock_Market(int? page)
        {
            /*Stock_Market ActionResult gets Stocks list from database and 
              If you are logged in  
            but didn'T have Trader  account system will automaticly create trader account and stock will be shown. */ 
            var page_no = page ?? 1;
            var stocks = db.Stocks.Include(s => s.Stock_Value);
            var name_customer = ((Customer)Session["customer"]);

            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == name_customer.Customer_id);

            ViewBag.Message = "Your application description page.";
           var stocks_= stocks.ToList().ToPagedList(page_no, 3);
            if (Session["customer"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            if (_trader == null)
            {
                return RedirectToAction("Createtrader",
                    new
                    {
                        Balance = 0,
                        Total_value = 0,
                        Customer_id = name_customer.Customer_id
                    });

            }
            return View(stocks_);
        }

        public ActionResult Stock_Sell_Market(int? page)
        {
            /*Stock_Sell_Market ActionResult gets Stocks list from your stocks you bought  before 
              If you are logged in  
            and bought stocks will be listed . */
            var transactions = db.Transactions.Include(t => t.Stock).Include(t => t.Trader);

            var _customer = ((Customer)Session["customer"]).Customer_id;
            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == _customer);
            var _transaction = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id);


            if (_trader == null ) { 
            MessageBox.Show("You can't sell stocks without having any.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            return RedirectToAction("Stock_Market");

            }
             var transactions1 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy").Count(); ;
            if (transactions1 == 0)
            {
                MessageBox.Show("You can't sell stocks without having any.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return RedirectToAction("Stock_Market");

            }
            transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy"); ;

            Session["Trader"] = _trader;
            Session["Transaction"] = _transaction;
            /*This part of code will  look is there any sell transaction made with your 
             * Trader_id and delete stock quantity from list if there ise none left delete entirely. */
            var Sell_transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell").Count();
            var list = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id).GroupBy(x => x.Stock_id).Select(s=> s.OrderByDescending(a=> a.Stock_id).FirstOrDefault()).ToList();
            var transactions2= transactions.GroupBy(x => x.Stock_id);
            var Sell_transactions2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell");
            var stok_id_Transac = Sell_transactions2.Select(x => x.Stock_id);
            var stok_id_Transac2 = transactions.Select(x => x.Stock_id);

            var page_no = page ?? 1;
            var stocks = db.Stocks.Include(s => s.Stock_Value);

            var stocks_ = transactions2.SelectMany(grup => grup.Take(1)).ToList();
            var stok_length=  stocks_.Count();
            var Stock_Quan = transactions2.SelectMany(grup => grup.Take(1)).Sum(x=> x.Stock_quantity);
            /*If you bought stocks and sold all of them this part of code will be delete the stokcs you sold entirely. */
            for (int i = 0; i < stok_length; i++)
            {
                foreach (var item in stocks_)
            {
                
                var transactions_Buy = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy" && x.Stock_id == item.Stock_id);
                var Sell_transactions_Sell = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == item.Stock_id).Count();
                var Sell_transactions_Sell2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == item.Stock_id);
               

                if (Sell_transactions_Sell>=1)
                {
                    var Stock_Quantity = transactions_Buy.Sum(x => x.Stock_quantity);
                    var Sell_Stock_Quan = Sell_transactions_Sell2.Sum(x => x.Stock_quantity);
                    int? Total_Stock_Quan = Stock_Quantity - Sell_Stock_Quan;
                    if (Total_Stock_Quan <= 0)
                    {

                        ViewBag.ıtem = item;

                      


                    }
                
                   
               
                }
            }
          
           
                var ıtem = ViewBag.ıtem;
                stocks_.Remove(ıtem);

            }

            var name_trader = ((Trader)Session["Trader"]);
          
               var deger = stocks_.ToPagedList(page_no, 3);

            if (Session["customer"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            return View(deger);
        }
        public ActionResult Account()
        {

            var transactions = db.Transactions.Include(t => t.Stock).Include(t => t.Trader);

            var _customer = ((Customer)Session["customer"]).Customer_id;
            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == _customer);
            var _transaction = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id);


            transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy"); ;
            Session["Trader"] = _trader;
           
            Session["Transaction"] = _transaction;
            if (_trader == null)
            {
                MessageBox.Show("You can't access account details without making a trade.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return RedirectToAction("Stock_Market");
            }
            /*This part of code will  do exact job like Stock Sell Market and listed 
             * if you sold entirely that stock will be deleted entriely. */ 
            var list = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id).GroupBy(x => x.Stock_id).Select(s => s.OrderByDescending(a => a.Stock_id).FirstOrDefault()).ToList();
            var transactions2 = transactions.GroupBy(x => x.Stock_id);
            var Sell_transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell").Count();
            var Sell_transactions2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell");
            var stok_id_Transac = Sell_transactions2.Select(x => x.Stock_id);
            var stok_id_Transac2 = transactions.Select(x => x.Stock_id);

           
            var stocks = db.Stocks.Include(s => s.Stock_Value);

            var stocks_ = transactions2.SelectMany(grup => grup.Take(1)).ToList();
            var stok_length = stocks_.Count();
            var Stock_Quan = transactions2.SelectMany(grup => grup.Take(1)).Sum(x => x.Stock_quantity);
          
            for (int i = 0; i < stok_length; i++)
            {
                foreach (var item in stocks_)
                {

                    var transactions_Buy = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy" && x.Stock_id == item.Stock_id);
                    var Sell_transactions_Sell = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == item.Stock_id).Count();
                    var Sell_transactions_Sell2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == item.Stock_id);


                    if (Sell_transactions_Sell >= 1)
                    {
                        var Stock_Quantity = transactions_Buy.Sum(x => x.Stock_quantity);
                        var Sell_Stock_Quan = Sell_transactions_Sell2.Sum(x => x.Stock_quantity);
                        int? Total_Stock_Quan = Stock_Quantity - Sell_Stock_Quan;
                        if (Total_Stock_Quan <= 0)
                        {

                            ViewBag.ıtem = item;




                        }



                    }
                }


                var ıtem = ViewBag.ıtem;
                stocks_.Remove(ıtem);

            }
            /*This code will run if you didn't sold any stocks. */
            if(Sell_transactions == 0)
            {
                ViewBag.stoklist = stocks_;
                var total_Value = stocks_.Sum(x => x.Transaction_value);
                var Net_worth = total_Value;
                ViewBag.net_worth = Net_worth;
                ViewBag.total_value = total_Value;
                return View(stocks_);
            }
            /*This code will run if you  sold any stocks. */

            else
            {
                ViewBag.stoklist = stocks_;
                var balance = Sell_transactions2.Sum(x => x.Transaction_value);
                var total_Value = stocks_.Sum(x => x.Transaction_value);
                var Net_worth = balance + total_Value;
                ViewBag.net_worth = Net_worth;
                ViewBag.total_value = total_Value;
                return View(stocks_);
            }
          
            var name_trader = ((Trader)Session["Trader"]);
           

            
            if (Session["customer"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            
            return View(stocks_);
        }
        public ActionResult Account_Bridge()
        {

            var transactions = db.Transactions.Include(t => t.Stock).Include(t => t.Trader);

            var _customer = ((Customer)Session["customer"]).Customer_id;
            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == _customer);
            var _transaction = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id);
            /*If you didn't bought any stocks this code will run. */
            if (_trader == null)
            {
                MessageBox.Show("You can't access account details without making a trade.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return RedirectToAction("Stock_Market");
            }
            var _transaction2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id).Count();
            transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy"); ;
            Session["Trader"] = _trader;

            Session["Transaction"] = _transaction;
            /*If you didn't bought any stocks this code will run. */

            if (_transaction2 == 0)
            {
                MessageBox.Show("You can't access account details without making a trade.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return RedirectToAction("Stock_Market");
            }
            var list = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id).GroupBy(x => x.Stock_id).Select(s => s.OrderByDescending(a => a.Stock_id).FirstOrDefault()).ToList();
            var transactions2 = transactions.GroupBy(x => x.Stock_id);
            var Sell_transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell").Count();
            var Sell_transactions2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell");
            var stok_id_Transac = Sell_transactions2.Select(x => x.Stock_id);
            var stok_id_Transac2 = transactions.Select(x => x.Stock_id);


            var stocks = db.Stocks.Include(s => s.Stock_Value);

            var stocks_ = transactions2.SelectMany(grup => grup.Take(1)).ToList();
            var stok_length = stocks_.Count();
            var Stock_Quan = transactions2.SelectMany(grup => grup.Take(1)).Sum(x => x.Stock_quantity);
            /*This part of code will  do exact job like Stock Sell Market and listed 
              * if you sold entirely that stock will be deleted entriely. */
            for (int i = 0; i < stok_length; i++)
            {
                foreach (var item in stocks_)
                {

                    var transactions_Buy = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy" && x.Stock_id == item.Stock_id);
                    var Sell_transactions_Sell = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == item.Stock_id).Count();
                    var Sell_transactions_Sell2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == item.Stock_id);


                    if (Sell_transactions_Sell >= 1)
                    {
                        var Stock_Quantity = transactions_Buy.Sum(x => x.Stock_quantity);
                        var Sell_Stock_Quan = Sell_transactions_Sell2.Sum(x => x.Stock_quantity);
                        int? Total_Stock_Quan = Stock_Quantity - Sell_Stock_Quan;
                        if (Total_Stock_Quan <= 0)
                        {

                            ViewBag.ıtem = item;




                        }



                    }
                }


                var ıtem = ViewBag.ıtem;
                stocks_.Remove(ıtem);

            }
            /*This code will run if you didn't sold any stocks. */

            if (Sell_transactions == 0)
            {
                var total_Value = stocks_.Sum(x => x.Transaction_value);
                var Net_worth =  total_Value;
                ViewBag.stoklist = stocks_;

                ViewBag.net_worth = Net_worth;
                ViewBag.total_value = total_Value;
                var name_trader = ((Trader)Session["Trader"]);
                return RedirectToAction("EditTrader", new
                {
                    Trader_id = name_trader.Trader_id,
                    Customer_id = name_trader.Customer_id,
                    Total_value = total_Value,
                    Balance = Net_worth,


                });

            }
            /*This code will run if you sold any stocks. */

            else
            {
                var balance = Sell_transactions2.Sum(x => x.Transaction_value);
                var total_Value = stocks_.Sum(x => x.Transaction_value);
                var Net_worth = balance + total_Value;
                ViewBag.net_worth = Net_worth;
                ViewBag.total_value = total_Value;
                //stocks_.Remove(removes);
                var name_trader = ((Trader)Session["Trader"]);
                return RedirectToAction("EditTrader", new
                {
                    Trader_id = name_trader.Trader_id,
                    Customer_id = name_trader.Customer_id,
                    Total_value = total_Value,
                    Balance = Net_worth,


                });
            }
           
            ViewBag.stoklist = stocks_;

           


            if (Session["customer"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }


            return View(stocks_);
        }
        public ActionResult Learn()
        {
            return View();
        }
        public ActionResult Consultancy()
        {
            var name_customer = ((Customer)Session["customer"]);
            var _customer = db.Customers.FirstOrDefault(x => x.Customer_id == name_customer.Customer_id);

            if (_customer.Customer_type.Equals("Standard"))
            {
                ViewBag.number = 1;
            };
            if (_customer.Customer_type.Equals("Premium"))
            {
                ViewBag.number = 2;
            };
            if (_customer.Customer_type.Equals("Business"))
            {
                ViewBag.number = 3;
            };
            return View();
        }
        public ActionResult EditTrader([Bind(Include = "Trader_id,Customer_id,Total_value,Balance")] Trader trader)
        {
            /*When you bought or sold stocks your trader account will change with this code.  */
            if (ModelState.IsValid)
            {
                db.Entry(trader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Account");
            }
            ViewBag.Customer_id = new SelectList(db.Customers, "Customer_id", "Customer_user_name", trader.Customer_id);
            return RedirectToAction("Account");
        }
        public ActionResult Edit([Bind(Include = "Customer_id,Customer_user_name,Customer_name,Customer_password,Customer_mail,Customer_tel,Customer_type")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        
        public ActionResult Consultancy_Buy(int? Customer_id,int? id)
        {
            var name_customer = ((Customer)Session["customer"]);

            var _customer = db.Customers.FirstOrDefault(x => x.Customer_id == name_customer.Customer_id);
            /*If you purchase any of subscriptions with this part of code 
             * your customer account details will be edited. 
             * Every Customer is Standard subscribe when they registered. */
            if (id == 1)
            {
                return RedirectToAction("Edit", new
                {
                    Customer_id = name_customer.Customer_id,
                    Customer_user_name = name_customer.Customer_user_name,
                    Customer_name = name_customer.Customer_name,
                    Customer_password = name_customer.Customer_password,
                    Customer_mail = name_customer.Customer_mail,
                    Customer_tel = name_customer.Customer_tel,
                    Customer_type ="Standard"

                });
                
              
            }
            if (id == 2)
            {
                return RedirectToAction("Edit", new
                {
                    Customer_id = name_customer.Customer_id,
                    Customer_user_name = name_customer.Customer_user_name,
                    Customer_name = name_customer.Customer_name,
                    Customer_password = name_customer.Customer_password,
                    Customer_mail = name_customer.Customer_mail,
                    Customer_tel = name_customer.Customer_tel,
                    Customer_type = "Premium"

                });


            }
            if (id == 3)
            {
                return RedirectToAction("Edit", new
                {
                    Customer_id = name_customer.Customer_id,
                    Customer_user_name = name_customer.Customer_user_name,
                    Customer_name = name_customer.Customer_name,
                    Customer_password = name_customer.Customer_password,
                    Customer_mail = name_customer.Customer_mail,
                    Customer_tel = name_customer.Customer_tel,
                    Customer_type = "Business"

                });


            }
            return View();
        }

        public ActionResult Stock_Sell(int? Stock_id, int? Customer_id, int? Trader_id)
        {
            /* When trade button clicked on  Stock_id  taken from Stock Sell Market page   Stock will be shown  .*/ 
            var name_customer = ((Customer)Session["customer"]);
            var name_trader = ((Trader)Session["Trader"]);
            ViewBag.transaction = db.Transactions.ToList();
            var _customer = ((Customer)Session["customer"]).Customer_id;
            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == name_customer.Customer_id);
            var _transaction = db.Transactions.FirstOrDefault(s => s.Stock_id == Stock_id);
            var transaction = db.Transactions.ToList();

            var stocks = db.Stocks.Where(s => s.Stock_id == Stock_id);

            var _stocks = db.Stocks.FirstOrDefault(s => s.Stock_id == Stock_id);
           

            var transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy" && x.Stock_id==Stock_id);
            var Sell_transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == Stock_id).Count();
            var Sell_transactions2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == Stock_id);
            ViewBag.Transaction = transactions;
           var  Stock_Quan=  transactions.Sum(x => x.Stock_quantity);
            /*If you sold any stocks  stock quantity  calculated. */
            if (Sell_transactions >= 1)
            {
                var Stock_Value = transactions.Sum(x => x.Transaction_value);
                var Sell_Stock_Value = Sell_transactions2.Sum(x => x.Transaction_value);

                var Sell_Stock_Quan = Sell_transactions2.Sum(x => x.Stock_quantity);
                double Total_Stock_Value = Stock_Value - Sell_Stock_Value;
                ViewBag.Stocks_Value = Total_Stock_Value;
                int? Total_Stock_Quan = Stock_Quan - Sell_Stock_Quan;
                ViewBag.Stocks_Quantity = Total_Stock_Quan;
               
                if (Total_Stock_Quan <= 0)
                {
                    return RedirectToAction("Stock_Buy_Sell", new
                    {
                        Stock_id = Stock_id
                    });
                      
                }
            }
            else
            {

                var Total_Stock_Quan = Stock_Quan;
                ViewBag.Stocks_Quantity = Total_Stock_Quan;
                var Stock_Value = transactions.Sum(x => x.Transaction_value);
                ViewBag.Stocks_Value = Stock_Value;


            }

            ViewBag.Stocks = db.Stocks.Where(s => s.Stock_id == Stock_id);


            if (_trader == null)
            {
                return RedirectToAction("Createtrader",
                    new
                    {
                        Balance = 0,
                        Total_value = 0,
                        Customer_id = name_customer.Customer_id
                    });

            }
            else
            {
                Session["Stock"] = _stocks;
                Session["Trader"] = _trader;
                return View();
            }

        }
        [HttpPost]
        public ActionResult Stock_Sell(int? Stock_id, int? Trader_id, int? Customer_id, int? Stock_quantity, double? Stock_price, int? id)
        {

            var stocks = db.Stocks.Where(s => s.Stock_id == Stock_id);
            var Trader = db.Traders.Where(s => s.Trader_id == Trader_id);
            var Trader_Customer = db.Traders.Where(s => s.Customer_id == Customer_id);

            double? Transaction_value = Stock_quantity * Stock_price;
            var _stock = ((Stock)Session["Stock"]);
            var Stock_Price = _stock.Stock_Value.Stock_price;
            var transaction_val = Stock_Price * Stock_quantity;
            var name_customer = ((Customer)Session["customer"]);
            var name_trader = ((Trader)Session["Trader"]);
            var name_trader_balance = name_trader.Balance;
            var name_trader_total_value = name_trader.Total_value;
            var _stock_transaction = db.Transactions.Select(s => s.Trader_id == name_trader.Trader_id && s.Stock_id == Stock_id && s.Transaction_value == Transaction_value && s.Stock_quantity == Stock_quantity);
            Session["Transaction"] = _stock_transaction;
            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == name_customer.Customer_id);


            var transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Buy" && x.Stock_id == Stock_id);
            var Sell_transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == Stock_id).Count();
            var Sell_transactions2 = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id && x.Transaction_Type == "Sell" && x.Stock_id == Stock_id);

            var Stock_Quan = transactions.Sum(x => x.Stock_quantity);

            if (name_customer.Customer_id == name_trader.Customer_id)
            {

                if (Sell_transactions >= 1)
                {
                    var Sell_Stock_Quan = Sell_transactions2.Sum(x => x.Stock_quantity);
                    int? Total_Stock_Quan1 = Stock_Quan - Sell_Stock_Quan;

                    int? Total_Stock_Quan = Total_Stock_Quan1 - Stock_quantity;
                    ViewBag.Stocks_Quantity = Total_Stock_Quan;
                    /*If you don't have enough stocks  system will warn and sends to buy more stocks. */
                    if (Total_Stock_Quan < 0)
                    {
                        MessageBox.Show("You can't sell more stocks than you have","Warning", MessageBoxButton.OK ,MessageBoxImage.Warning);
                        
                        return RedirectToAction("Stock_Buy_Sell", new
                        {
                            Stock_id = Stock_id
                        });

                    }
                    /*If you  have enough stocks   will gives  info  and Transactions inserted to database.. */

                    else
                    {
                        MessageBox.Show("Transfer succesful.", "Succesful", MessageBoxButton.OK, MessageBoxImage.Information);

                        return RedirectToAction("Create_transaction",
                   new
                   {
                       Trader_id = name_trader.Trader_id,
                       Stock_id = _stock.Stock_id,
                       Transaction_value = transaction_val,
                       Stock_quantity = Stock_quantity,
                       Transaction_Type = "Sell"
                   });
                    }
                }
                else
                {

                    int? Total_Stock_Quan = Stock_Quan - Stock_quantity;
                    ViewBag.Stocks_Quantity = Total_Stock_Quan;

                    if (Total_Stock_Quan < 0)
                    {
                        MessageBox.Show("You can't sell more stocks than you have");
                        return RedirectToAction("Stock_Buy_Sell", new
                        {
                            Stock_id = Stock_id
                        });

                    }
                    else { 
                    return RedirectToAction("Create_transaction",
               new
               {
                   Trader_id = name_trader.Trader_id,
                   Stock_id = _stock.Stock_id,
                   Transaction_value = transaction_val,
                   Stock_quantity = Stock_quantity,
                   Transaction_Type = "Sell"
               });
                    }

                }

               


                
            }



            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }
      
        public ActionResult Stock_Buy_Sell(int? Stock_id ,int? Customer_id, int? Trader_id)
        {
            /* When trade button clicked on  Stock_id  taken from Stock Market page   Stock will be shown  .*/


            ViewBag.transaction = db.Transactions.ToList();
            var transaction=    db.Transactions.ToList();
            var stocks = db.Stocks.Where(s => s.Stock_id == Stock_id);
            ViewBag.Stocks = db.Stocks.Where(s => s.Stock_id == Stock_id);
            var name_customer = ((Customer)Session["customer"]);
            var name_trader = ((Trader)Session["Trader"]);
            var _stocks = db.Stocks.FirstOrDefault(s => s.Stock_id == Stock_id);

            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == name_customer.Customer_id);
            var _transaction = db.Transactions.FirstOrDefault(s => s.Stock_id == Stock_id);
            
             if (_trader == null)
             {
                 return RedirectToAction("Createtrader",
                     new
                     {
                         Balance = 0,
                         Total_value = 0,
                         Customer_id = name_customer.Customer_id
                     });

             }
             else
             {
                Session["Stock"] = _stocks;
                Session["Trader"] = _trader;
                 return View();
             }

        }
      [HttpPost]
         public ActionResult Stock_Buy_Sell (int? Stock_id,int? Trader_id, int? Customer_id,int? Stock_quantity,double? Stock_price,int? id)
        {
           
                   var stocks = db.Stocks.Where(s => s.Stock_id == Stock_id);
                   var Trader = db.Traders.Where(s => s.Trader_id == Trader_id);
                   var Trader_Customer = db.Traders.Where(s => s.Customer_id == Customer_id);
         
            double? Transaction_value = Stock_quantity * Stock_price;
            var _stock = ((Stock)Session["Stock"]);
            var Stock_Price = _stock.Stock_Value.Stock_price;
            var transaction_val = Stock_Price * Stock_quantity;
                    var name_customer = ((Customer)Session["customer"]);
                      var name_trader = ((Trader)Session["Trader"]);
            var name_trader_balance = name_trader.Balance;
            var name_trader_total_value = name_trader.Total_value;
            var _stock_transaction = db.Transactions.Select(s => s.Trader_id == name_trader.Trader_id && s.Stock_id == Stock_id && s.Transaction_value == Transaction_value && s.Stock_quantity == Stock_quantity);
            Session["Transaction"] = _stock_transaction;

         

                if (name_customer.Customer_id != name_trader.Customer_id) 
                {
                    return RedirectToAction("Createtrader",
                        new
                      {
                               Balance = Transaction_value,
                              Total_value = Transaction_value,
                               Customer_id = name_customer.Customer_id
                        }) ;
                  }

                if (name_customer.Customer_id==name_trader.Customer_id && Stock_quantity>0)
                {

                MessageBox.Show("Transfer succesful.", "Succesful", MessageBoxButton.OK, MessageBoxImage.Information);

                return RedirectToAction("Create_transaction",
                           new
                           {
                               Trader_id = name_trader.Trader_id,
                               Stock_id = _stock.Stock_id,
                               Transaction_value = transaction_val,
                               Stock_quantity = Stock_quantity,
                               Transaction_Type = "Buy"
                           });
                
               
                }
            else 
            {

                MessageBox.Show("Stock Quantity can't be negative.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return RedirectToAction("Stock_Buy_Sell", new
                {
                    Stock_id = Stock_id
                });
            }
            
           

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

       
        public ActionResult Createtrader([Bind(Include = "Trader_id,Customer_id,Total_value,Balance")] Trader trader)
        {
         /*Trader Account  created first time. */
            if (ModelState.IsValid)
            {

                db.Traders.Add(trader);
                db.SaveChanges();


            }
            
            return RedirectToAction("Stock_Market");


        }
        public ActionResult Updatetrader([Bind(Include = "Trader_id,Customer_id,Total_value,Balance")] Trader trader)
        {
            /*Trader Account  updated depend on sell or buy Transaction. */

            if (ModelState.IsValid)
            {

                db.Traders.Add(trader);
                db.SaveChanges();


            }
            
            return RedirectToAction("Stock_Buy_Sell");


        }
        public ActionResult Create_transaction([Bind(Include = "Transaction_id,Trader_id,Stock_id,Transaction_value,Stock_quantity,Transaction_type")] Transaction transaction)
        {
            /* Transaction record created depends on buy/sell . */
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "Stock_name", transaction.Stock_id);
            ViewBag.Trader_id = new SelectList(db.Traders, "Trader_id", "Trader_id", transaction.Trader_id);
            return RedirectToAction("Index");
        }
        public ActionResult About()
          
        {
            return View();
        }
        public ActionResult Login()
            
        {
                  ViewBag.Message = "Your contact page.";

                  return View();
         }

              [HttpPost]
              public ActionResult Login(Customer customer)
              {
            var admin = db.Customers.FirstOrDefault(x => customer.Customer_user_name == "admin" && customer.Customer_password == "admin");
                  var _customer = db.Customers.FirstOrDefault(x => x.Customer_user_name == customer.Customer_user_name && x.Customer_password==customer.Customer_password);
                  if (_customer != null)
                  {
                      Session["customer"] = _customer;

                  }
                  if (admin != null)
                  {
                  Session["admin"] = admin;
                 }
                  return RedirectToAction("index");
              }
        public ActionResult Login_After_Register(Customer customer)
        {/*When you register system will automatically  login and sends you homepage.
         */
            var admin = db.Customers.FirstOrDefault(x => customer.Customer_user_name == "admin" && customer.Customer_password == "admin");
            var _customer = db.Customers.FirstOrDefault(x => x.Customer_user_name == customer.Customer_user_name && x.Customer_password == customer.Customer_password);
            if (_customer != null)
            {
                Session["customer"] = _customer;

            }
            if (admin != null)
            {
                Session["admin"] = admin;
            }
            return RedirectToAction("index");
        }
        public ActionResult Register()
              {
                  ViewBag.Message = "Your contact page.";

                  return View();
              }


            // POST: Customers/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Customer_id,Customer_user_name,Customer_name,Customer_password,Customer_mail,Customer_tel,Customer_type")] Customer customer)
        {
            customer.Customer_type = "Standard";
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
               
                return RedirectToAction("Login_After_Register", customer);
            }

            return View();
        }
        public ActionResult Account_Details(int? Customer_id)
        {
            var transactions = db.Transactions.Include(t => t.Stock).Include(t => t.Trader);

            var _customer = 2;
            var _trader = db.Traders.FirstOrDefault(x => x.Customer_id == _customer);
            var _transaction = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id);
            transactions = db.Transactions.Where(x => x.Trader_id == _trader.Trader_id);
            ViewBag.Data = transactions.ToList();
            return View();
        }
       
        public ActionResult Exit_Account()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index");
        }
        public ActionResult Admin_page_Transactions()
        {
            /* Transactions taken from database and sended to view.*/
            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var transactions = db.Transactions.Include(t => t.Stock).Include(t => t.Trader);
            var _transaction = db.Transactions.Count();
            var asset = db.Traders.Sum(x => x.Balance);
            var members = db.Customers.Count();
            var stocks = db.Stocks.Count();
            ViewBag.transaction = _transaction;
            ViewBag.asset = asset;
            ViewBag.members = members;
            ViewBag.stocks = stocks;
            Session["Transaction"] = _transaction;
            ViewBag.Message = "Your contact page.";
           
            return View(transactions);
        }
        public ActionResult Admin_page_Traders()
        {
            /* Traders taken from database(who made trade ) and sended to view.*/

            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var trader = db.Traders.Include(t => t.Transactions).Include(t => t.Customer);
           
          var ımportant_trader = trader.Where(x => x.Balance > 0 || x.Total_value > 0);
           var group_ımportant=  ımportant_trader.GroupBy(x => x.Trader_id);

            var _transaction = db.Transactions.Count();
            var asset = db.Traders.Sum(x => x.Balance);
            var members = db.Customers.Count();
            var stocks = db.Stocks.Count();
           
          

            var Traders = group_ımportant.SelectMany(grup => grup.Take(1));

            ViewBag.transaction = _transaction;
            ViewBag.asset = asset;
            ViewBag.members = members;
            ViewBag.stocks = stocks;
            Session["Transaction"] = _transaction;
            ViewBag.Message = "Your contact page.";

            return View(Traders);
        }
        public ActionResult Admin_page_Customers()
        {
            /*Customers taken from database and sended to view.*/

            if (Session["admin"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var Customers = db.Transactions.Include(t => t.Stock).Include(t => t.Trader);
            var asset = db.Traders.Sum(x => x.Balance);
            var members = db.Customers.Count();
            var stocks = db.Stocks.Count();
            var _transaction = db.Transactions.Count();
            ViewBag.transaction = _transaction;
            ViewBag.asset = asset;
            ViewBag.members = members;
            ViewBag.stocks = stocks;
            Session["Transaction"] = _transaction;
            ViewBag.Message = "Your contact page.";

            return View(db.Customers.ToList());
        }
        public ActionResult Transactions_Report(byte? id)
        {
            /* Transactions lişt taken from database and exported as pdf or excel file..*/

            var transacs = from trans in db.Transactions
                           from customer in db.Customers
                           from trader in db.Traders
                           from stock in db.Stocks
                           where trans.Trader_id == trader.Trader_id && customer.Customer_id == trader.Customer_id && trans.Stock_id == stock.Stock_id
                           select new
                           {
                               trans.Transaction_id,
                               customer.Customer_user_name,
                               stock.Stock_name,
                               trans.Transaction_value,
                               trans.Transaction_Type
                           };
            ReportDocument rapor = new ReportDocument();
            rapor.Load(Server.MapPath("~/Crystal_Reports/Transactions_Report.rpt"));
            rapor.SetDataSource(transacs);
            Response.Buffer = false;
            Response.ClearContent();
            if (id == 1)
            {
                Stream stream = rapor.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Transactions_Report.pdf");
            }
            else
            {
                Stream stream = rapor.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/xls", "Transactions.xls");
            }
        }
        public ActionResult Traders_Report(byte? id)
        {
            /* Traders lişt taken from database and exported as pdf or excel file..*/

            var transacs = from customer in db.Customers
                           from trader in db.Traders
                           where  customer.Customer_id == trader.Customer_id  && trader.Total_value >0 
                           select new
                           {
                               customer.Customer_type,
                               trader.Customer.Customer_name,
                               trader.Total_value ,
                                trader.Balance
                               
                           };
            ReportDocument rapor = new ReportDocument();
            rapor.Load(Server.MapPath("~/Crystal_Reports/Traders.rpt"));
          rapor.SetDataSource(transacs);
            Response.Buffer = false;
            Response.ClearContent();
            if (id == 1)
            {
                Stream stream = rapor.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Traders_Report.pdf");
            }
            else
            {
                Stream stream = rapor.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/xls", "Traders.xls");
            }
        }
        public ActionResult Customers_Report(byte? id)
        {
            /*Customers lişt taken from database and exported as pdf or excel file..*/

            var customers = from customer in db.Customers
                           from trader in db.Traders
                       
                           select new
                           {
                               customer.Customer_name,
                                customer.Customer_type,
                               customer.Customer_mail,
                              
                               
                           };
            ReportDocument rapor = new ReportDocument();
            rapor.Load(Server.MapPath("~/Crystal_Reports/Customers_Report.rpt"));
            rapor.SetDataSource(customers);
            Response.Buffer = false;
            Response.ClearContent();
            if (id == 1)
            {
                Stream stream = rapor.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Customers_Report.pdf");
            }
            else
            {
                Stream stream = rapor.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/xls","Customers_Report.xls");
            }
        }

        public ActionResult Delete_Transactions(int? Transaction_id)
        {
            if (Transaction_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(Transaction_id);
            
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Admin_page_Transactions");
        }

    }
}
