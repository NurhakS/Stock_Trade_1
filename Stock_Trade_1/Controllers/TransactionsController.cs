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
    public class TransactionsController : Controller
    {
        private Stock_TradeEntities2 db = new Stock_TradeEntities2();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Stock).Include(t => t.Trader);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "Stock_name");
            ViewBag.Trader_id = new SelectList(db.Traders, "Trader_id", "Trader_id");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Transaction_id,Trader_id,Stock_id,Transaction_value,Stock_quantity")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "Stock_name", transaction.Stock_id);
            ViewBag.Trader_id = new SelectList(db.Traders, "Trader_id", "Trader_id", transaction.Trader_id);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "Stock_name", transaction.Stock_id);
            ViewBag.Trader_id = new SelectList(db.Traders, "Trader_id", "Trader_id", transaction.Trader_id);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Transaction_id,Trader_id,Stock_id,Transaction_value,Stock_quantity")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "Stock_name", transaction.Stock_id);
            ViewBag.Trader_id = new SelectList(db.Traders, "Trader_id", "Trader_id", transaction.Trader_id);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
