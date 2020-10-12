using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderTracker.Models;
using MongoDB.Driver;

namespace OrderTracker.Services
{
	public class MongoDBOrdersRepository : IOrdersRepository
	{
		private readonly IMongoCollection<Order> _orders;

		public MongoDBOrdersRepository(IMongoDBSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var db = client.GetDatabase(settings.DatabaseName);

			_orders = db.GetCollection<Order>(settings.OrdersCollectionName);
		}

		public List<Order> GetAll() =>
			_orders.Find(order => true).ToList();

		public Order Get(int orderNumber) =>
			_orders.Find(order => order.Number == orderNumber).First();

		public Order Create()
		{
			Order order = new Order();
			Order lastOrder = _orders.Find(o => true).SortByDescending(o => o.Number).First();
			int newNumber = lastOrder != null ? lastOrder.Number + 1 : 1;
			order.Number = newNumber;

			_orders.InsertOne(order);

			return order;
		}

		public Order ChangeOrderStatus(int orderNumber, OrderStatus status, string comment)
		{
			Order order = _orders.Find(_order => _order.Number == orderNumber).First();

			order.ChangeStatus(status, comment);

			_orders.ReplaceOne(_order => _order.Number == order.Number, order);

			return order;
		}

		public void Delete(int orderNumber)
		{
			_orders.DeleteOne(_orders => _orders.Number == orderNumber);
		}
	}
}
