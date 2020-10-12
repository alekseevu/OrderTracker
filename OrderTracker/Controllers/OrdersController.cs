using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderTracker.Services;
using OrderTracker.Models;

namespace OrderTracker.Controllers
{
	//	[Route("[controller]")]
	//	[ApiController]
	public class OrdersController : ControllerBase
	{
		private IOrdersRepository _ordersRepo;

		public OrdersController(IOrdersRepository ordersRepo)
		{
			_ordersRepo = ordersRepo;
		}

		[Route("[controller]/[action]")]
		[HttpGet]
		public ActionResult<IEnumerable<Order>> Get() =>
			GetAll();

		[Route("[controller]/[action]")]
		[HttpGet]
		public ActionResult<IEnumerable<Order>> GetAll()
		{
			try
			{
				return _ordersRepo.GetAll();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Route("[controller]/[action]/{number}")]
		[HttpGet]
		public ActionResult<Order> Get(int number)
		{
			try
			{
				return _ordersRepo.Get(number);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Route("[controller]/[action]")]
		[HttpGet]
		public ActionResult<Order> Create()
		{
			try
			{
				return _ordersRepo.Create();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Route("[controller]/[action]/{number}")]
		[HttpGet]
		public ActionResult Delete(int number)
		{
			try
			{
				_ordersRepo.Delete(number);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Route("[controller]/[action]/{number}")]
		[HttpGet]
		public ActionResult<Order> SetStatus(int number, [FromQuery] OrderStatus status,[FromQuery] string comment)
		{
			try
			{
				return _ordersRepo.ChangeOrderStatus(number, status, comment);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
