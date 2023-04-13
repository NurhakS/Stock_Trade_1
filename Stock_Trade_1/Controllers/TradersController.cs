using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Stock_Trade_1.Models;

namespace Stock_Trade_1.Controllers
{
    public class TradersController : Controller
    {
        private Stock_TradeEntities2 db = new Stock_TradeEntities2();

        // GET: Traders
        public ActionResult Index()
        {
            var traders = db.Traders.Include(t => t.Customer);
            return View(traders.ToList());
        }

        // GET: Traders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = db.Traders.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            return View(trader);
        }

        // GET: Traders/Create
        public ActionResult Create()
        {
            ViewBag.Customer_id = new SelectList(db.Customers, "Customer_id", "Customer_user_name");
            return View();
        }

        // POST: Traders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
       
       // [ValidateAntiForgeryToken]
        public ActionResult Createtrader([Bind(Include = "Trader_id,Customer_id,Total_value,Balance")] Trader trader)
        {
            if (ModelState.IsValid)
            {
                db.Traders.Add(trader);
                db.SaveChanges();
                var _stock_transaction = ((Transaction)Session["Transaction"]);
                return RedirectToAction("Create_transaction","Transactions",
               new
               {
                   Trader_id = _stock_transaction.Trader_id,
                   Stocks_id = _stock_transaction.Stock_id,
                   Transaction_Value = _stock_transaction.Transaction_value,
                   Stock_quantity = _stock_transaction.Stock_quantity
               });
            }

            ViewBag.Customer_id = new SelectList(db.Customers, "Customer_id", "Customer_user_name", trader.Customer_id);
            return View(trader);
        }

        // GET: Traders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = db.Traders.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customer_id = new SelectList(db.Customers, "Customer_id", "Customer_user_name", trader.Customer_id);
            return View(trader);
        }

        // POST: Traders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Trader_id,Customer_id,Total_value,Balance")] Trader trader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customer_id = new SelectList(db.Customers, "Customer_id", "Customer_user_name", trader.Customer_id);
            return View(trader);
        }

        // GET: Traders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trader trader = db.Traders.Find(id);
            if (trader == null)
            {
                return HttpNotFound();
            }
            return View(trader);
        }

        // POST: Traders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trader trader = db.Traders.Find(id);
            db.Traders.Remove(trader);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
