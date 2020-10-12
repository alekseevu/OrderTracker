using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderTracker.Models;
using System;

namespace OrderTrackerTests
{
	[TestClass]
	public class OrderTests
	{
		private static Order order;

		[TestMethod]
		public void Test00_CreateOrder_History_Is_Not_Empty()
		{
			Order order = new Order();

			Assert.IsNotNull(order.History);
			Assert.IsTrue(order.History.Count == 1);
			Assert.IsTrue(order.History[0].Status == OrderStatus.Picking);
		}

		[TestMethod]
		public void Test01_ChangeStatus_And_History_Flow()
		{
			order = new Order();

			order.ChangeStatus(OrderStatus.ReadyToSend, "message");

			Assert.IsTrue(order.History.Count == 2);
			Assert.IsTrue(order.History[1].Status == OrderStatus.ReadyToSend);
			Assert.IsTrue(order.History[1].StatusComment == "message");
		}

		[TestMethod]
		public void Test02_ChangeStatus_And_History_Flow()
		{
			order.ChangeStatus(OrderStatus.Sent, "message2");

			Assert.IsTrue(order.History.Count == 3);
			Assert.IsTrue(order.History[2].Status == OrderStatus.Sent);
			Assert.IsTrue(order.History[2].StatusComment == "message2");
		}

		[TestMethod]
		public void Test03_ChangeStatus_And_History_Flow()
		{
			order.ChangeStatus(OrderStatus.InTransit, "message3");

			Assert.IsTrue(order.History.Count == 4);
			Assert.IsTrue(order.History[3].Status == OrderStatus.InTransit);
			Assert.IsTrue(order.History[3].StatusComment == "message3");
		}

		[TestMethod]
		public void Test04_ChangeStatus_And_History_Flow()
		{
			order.ChangeStatus(OrderStatus.InTransit, "message4");

			Assert.IsTrue(order.History.Count == 5);
			Assert.IsTrue(order.History[4].Status == OrderStatus.InTransit);
			Assert.IsTrue(order.History[4].StatusComment == "message4");
		}

		[TestMethod]
		public void Test05_ChangeStatus_And_History_Flow()
		{
			order.ChangeStatus(OrderStatus.ReadyToPickUp, "message5");

			Assert.IsTrue(order.History.Count == 6);
			Assert.IsTrue(order.History[5].Status == OrderStatus.ReadyToPickUp);
			Assert.IsTrue(order.History[5].StatusComment == "message5");
		}

		[TestMethod]
		public void Test06_ChangeStatus_And_History_Flow()
		{
			order.ChangeStatus(OrderStatus.PickedUp, "message6");

			Assert.IsTrue(order.History.Count == 7);
			Assert.IsTrue(order.History[6].Status == OrderStatus.PickedUp);
			Assert.IsTrue(order.History[6].StatusComment == "message6");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ChangeStatus_On_Completed_Order_Not_Allowed()
		{
			order.ChangeStatus(OrderStatus.ReadyToSend, null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ChangeStatus_Cant_Jump_Over_Status()
		{
			Order order = new Order();

			order.ChangeStatus(OrderStatus.PickedUp, null);
		}

		[TestMethod]
		public void ChangeStatus_Can_Cancel_On_Any_Stage()
		{
			Order order = new Order();

			order.ChangeStatus(OrderStatus.Cancelled, null);

			Assert.IsTrue(order.History[1].Status == OrderStatus.Cancelled);
		}

	}
}
