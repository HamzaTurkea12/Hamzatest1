using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HamzaTest.Data;
using HamzaTest.Models;

namespace HamzaTest.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HamzaCustomerCRUDContext _context;


        public CustomerController(HamzaCustomerCRUDContext context)
        {
            _context = context;
        }
        public IActionResult Index( int pg =1)
        {
            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.Customers.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            List<Customer> customers = _context.Customers.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(customers);
        }
        public IActionResult Details(int Id)
        {
            Customer customer = _context.Customers.Where(p => p.CustomerId == Id).FirstOrDefault();
            return View(customer);
        }
        [HttpGet]
        public IActionResult EDit(int Id)
        {
            Customer customer = _context.Customers.Where(p => p.CustomerId == Id).FirstOrDefault();
            return View(customer);
        }
        [HttpPost]
        public IActionResult EDIT (Customer customer)
        {
            _context.Attach(customer);
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Delete (int Id)
        {
            Customer customer = _context.Customers.Where(p => p.CustomerId == Id).FirstOrDefault();
            return View(customer);

        }
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            _context.Attach(customer);
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            Customer customer = new Customer();
            return View(customer);
        }
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            var customerid = _context.Customers.Max(custid => custid.CustomerId);
            customer.CustomerId = customerid;
            _context.Attach(customer);
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.SaveChanges();
            return RedirectToAction("index");
        }


    }
}
