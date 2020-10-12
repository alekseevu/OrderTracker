using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderTracker.Models;


namespace OrderTracker.Services
{
	public class InMemoryOrdersRepository : IOrdersRepository
	{
		private readonly List<Order> _orders = new List<Order>();
		private int count = 0;
		private object _ordersLock = new object();

		public InMemoryOrdersRepository()
		{
		}

		public List<Order> GetAll() =>
			_orders;

		public Order Get(int orderNumber) =>
			_orders.First(order => order.Number == orderNumber);

		public Order Create()
		{
			Order order = new Order();
			lock (_ordersLock)
			{
				order.Number = ++count;

				_orders.Add(order);
			}
			return order;
		}

		public Order ChangeOrderStatus(int orderNumber, OrderStatus status, string comment)
		{
			lock (_ordersLock)
			{
				Order order = _orders.First(_order => _order.Number == orderNumber);

				order.ChangeStatus(status, comment);

				return order;
			}
		}

		public void Delete(int orderNumber)
		{
			lock (_ordersLock)
			{
				Order order = _orders.First(_order => _order.Number == orderNumber);
				_orders.Remove(order);
			}
		}
	}
}
