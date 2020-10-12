using OrderTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTracker.Services
{
	public interface IOrdersRepository
	{
		List<Order> GetAll();

		Order Get(int orderNumber);

		Order Create();

		Order ChangeOrderStatus(int orderNumber, OrderStatus status, string comment);

		void Delete(int orderNumber);
	}
}
